using System.Web.Razor.Generator;
using Risk.Models;
using Risk.Models.Actions;
using Risk.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Risk.Core
{
    public class GameManager
    {
        public List<Country> Countries;
        public List<IAction> Actions;
        public IPlayer CurrentPlayer;
        public EPhase CurrentPhase;

        public IPlayer LastPlayer;
        public EPhase LastPhase;

        public int TroopsToDeploy;
        public int TroopsToMove;

        private Dictionary<IPlayer, int> playersWithTroopsInStartupPhase;
        private readonly Random r = new Random();

        public int Turn { get; set; }
        private IPlayer startPlayer;

        public bool GameEnded { get; set; }

        private GameManager()
        {
            Turn = 0;
            GameEnded = false;
            CurrentPhase = EPhase.Deploy;
            Actions = new List<IAction>();

            LoadPlayers();
            SetRandomPlayer();
            LoadCountries();
            DivideCountries();
        }

        private void LoadPlayers()
        {
            var numberOfTroopsToDeploy = 50 - (Settings.Players.Count * 5);

            playersWithTroopsInStartupPhase = new Dictionary<IPlayer, int>();
            foreach (var player in Settings.Players)
            {
                playersWithTroopsInStartupPhase.Add(player, numberOfTroopsToDeploy);
            }
        }

        private int GetTroopsInStartupPhase()
        {
            int troops = playersWithTroopsInStartupPhase[CurrentPlayer];

            if (troops > 1)
            {
                playersWithTroopsInStartupPhase[CurrentPlayer] -= 2;
                return 2;
            }

            if (troops == 1)
            {
                playersWithTroopsInStartupPhase[CurrentPlayer] -= 1;
                return 1;
            }

            return 0;
        }

        public static GameManager Get()
        {
            if (HttpContext.Current.Application["Game"] == null)
            {
                HttpContext.Current.Application["Game"] = new GameManager();
            }

            return (GameManager)HttpContext.Current.Application["Game"];
        }

        public static void Reset()
        {
            if (HttpContext.Current.Application["Game"] != null)
            {
                HttpContext.Current.Application.Remove("Game");
            }
        }

        private void LoadCountries()
        {
            Countries = new TerrainGenerator().Generate();
        }

        private void DivideCountries()
        {
            do
            {
                SetNextPlayer(true);
                var countriesWithoutOwner = Countries.Where(country => country.Owner == null).ToList();
                countriesWithoutOwner[r.Next(0, countriesWithoutOwner.Count - 1)].Owner = CurrentPlayer;
                playersWithTroopsInStartupPhase[CurrentPlayer]--;
            } while (Countries.Any(country => country.Owner == null));
        }

        private void SetRandomPlayer()
        {
            startPlayer = Settings.Players[r.Next(0, Settings.Players.Count - 1)];
            CurrentPlayer = startPlayer;
        }

        public void DoStartupPhase()
        {
            Actions.ForEach(action => action.ShowOnMap = false);

            int troopsInStartupPhase = 0;

            do
            {
                troopsInStartupPhase = GetTroopsInStartupPhase();

                if (troopsInStartupPhase > 0)
                {
                    TroopsToDeploy = troopsInStartupPhase;

                    try
                    {
                        CurrentPlayer.Deploy(troopsInStartupPhase);
                    }
                    catch (Exception e)
                    {
                        new ActionLogger().AddException(e);
                    }

                    SetNextPlayer();
                }
            } while (troopsInStartupPhase != 0);
        }

        public void DoNextTurn()
        {
            string activePlayerName = CurrentPlayer.Name;
            ClearActions();

            do
            {
                DoNextPhase(false);
            } while (activePlayerName == CurrentPlayer.Name && !GameEnded);
        }

        private void ClearActions()
        {
            Actions.ForEach(action => action.ShowOnMap = false);
        }

        private IPlayer GetPlayerWithMostCountries()
        {
            IPlayer playerWithMostCountries = CurrentPlayer;
            int count = 0;
            var gameInfo = new GameInformation(this);

            foreach (var player in Settings.Players)
            {
                var countries = gameInfo.GetAllCountriesOwnedByPlayer(player);

                if (countries.Count > count)
                {
                    playerWithMostCountries = player;
                    count = countries.Count;
                }
            }

            return playerWithMostCountries;
        }

        public void DoNextPhase(bool eraseActions = true)
        {
            if (GameEnded) return;

            if (eraseActions)
            {
                ClearActions();
            }

            int troopsInStartupPhase = GetTroopsInStartupPhase();

            if (troopsInStartupPhase > 0)
            {
                TroopsToDeploy = troopsInStartupPhase;
                try
                {
                    CurrentPlayer.Deploy(troopsInStartupPhase);
                }
                catch (Exception e)
                {
                    new ActionLogger().AddException(e);
                }
                SetNextPlayer();
            }
            else
            {
                if (CurrentPhase == EPhase.Deploy)
                {
                    if (CurrentPlayer == startPlayer)
                    {
                        Turn++;

                        if (Turn > Settings.MaximumAmountOfTurns)
                        {
                            IPlayer winner = GetPlayerWithMostCountries();
                            new ActionLogger().AddMessage(string.Format("----- {0} has won the game with {1} countries, turn limit reached", winner.Name, new GameInformation().GetAllCountriesOwnedByPlayer(winner).Count));
                            Statistics.Get().AddCurrentGameResults(winner);
                            GameEnded = true;
                            return;
                        }
                    }

                    int troopsToDeploy = new DeploymentCounter(CurrentPlayer).GetTroopsToDeploy();
                    new ActionLogger().AddGameInfo(CurrentPhase, troopsToDeploy);
                    TroopsToDeploy = troopsToDeploy;

                    try
                    {
                        CurrentPlayer.Deploy(troopsToDeploy);
                    }
                    catch (Exception e)
                    {
                        new ActionLogger().AddException(e);
                    }
                }
                else if (CurrentPhase == EPhase.Attack)
                {
                    new ActionLogger().AddGameInfo(CurrentPhase);

                    try
                    {
                        CurrentPlayer.Attack();
                    }
                    catch (Exception e)
                    {
                        new ActionLogger().AddException(e);
                    }

                    if (PlayerHasWon())
                    {
                        new ActionLogger().AddMessage(string.Format("----- {0} has won the game with {1} countries", CurrentPlayer.Name, new GameInformation().GetAllCountriesOwnedByPlayer(CurrentPlayer).Count));
                        Statistics.Get().AddCurrentGameResults(CurrentPlayer);
                        GameEnded = true;
                        return;
                    }
                }
                else if (CurrentPhase == EPhase.Move)
                {
                    new ActionLogger().AddGameInfo(CurrentPhase);
                    TroopsToMove = 7;

                    try
                    {
                        CurrentPlayer.Move();
                    }
                    catch (Exception e)
                    {
                        new ActionLogger().AddException(e);
                    }

                    SetNextPlayer();
                }

                SetNextPhase();
            }
        }

        private bool PlayerHasWon()
        {
            return new GameInformation(this).GetAllCountriesOwnedByPlayer(CurrentPlayer).Count >= Settings.CountriesRequiredToWin;
        }

        private void SetNextPhase()
        {
            LastPhase = CurrentPhase;

            switch (LastPhase)
            {
                case EPhase.Deploy:
                    CurrentPhase = EPhase.Attack;
                    break;
                case EPhase.Attack:
                    CurrentPhase = EPhase.Move;
                    break;
                case EPhase.Move:
                    CurrentPhase = EPhase.Deploy;
                    break;
            }
        }

        private void SetNextPlayer(bool init = false)
        {
            IPlayer nextPlayer = CurrentPlayer;

            do
            {
                int currentPlayerIndex = Settings.Players.IndexOf(nextPlayer);
                nextPlayer = Settings.Players[currentPlayerIndex == Settings.Players.Count - 1 ? 0 : ++currentPlayerIndex];
            } while (!PlayerIsAlive(nextPlayer) && !init);

            LastPlayer = CurrentPlayer;
            CurrentPlayer = nextPlayer;
        }

        private bool PlayerIsAlive(IPlayer player)
        {
            return new GameInformation(this).GetAllCountriesOwnedByPlayer(player).Any();
        }

        public List<int> GetDiceRolls(int number)
        {
            var rolls = new List<int>();

            for (int i = 0; i < number; i++)
            {
                rolls.Add(r.Next(1, 7));
            }

            return rolls;
        }

    }
}