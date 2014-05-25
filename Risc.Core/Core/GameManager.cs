using System;
using System.Collections.Generic;
using System.Linq;
using Risk.Models;
using Risk.Models.Actions;

namespace Risk.Core
{
    public class GameManager
    {
        private  readonly Statistics statistics;

        public List<IAction> Actions { get; private set; }
        public List<Country> Countries { get; private set; }
        public EPhase CurrentPhase { get; private set; }
        public IPlayer CurrentPlayer { get; private set; }

        public EPhase LastPhase { get; private set; }
        public IPlayer LastPlayer { get; private set; }

        public int TroopsToDeploy { get; internal set; }
        public int TroopsToMove { get; internal set; }

        private readonly Random rnd = new Random();
        private Dictionary<IPlayer, int> playersWithTroopsInStartupPhase;
        private IPlayer startPlayer;



        public ActionLogger Log { get; private set; }
        public int MaximumAmountOfTurns { get; private set; }
        public int CountriesRequiredToWin { get; private set; }
        public IList<IPlayer> Players { get; private set; }
        public int Turn { get; private set; }
        public bool GameEnded { get;private  set; }

        public GameManager(Statistics statistics, Settings settings, bool init = true)
        {
            Turn = 0;
            GameEnded = false;
            CurrentPhase = EPhase.Deploy;
            Actions = new List<IAction>();
            Log = new ActionLogger(this);
            Countries = new TerrainGenerator().Generate();
            this.statistics = statistics;
            if (init) Initialize(settings);
        }

        public void DoStartupPhase()
        {
            Actions.ForEach(action => action.ShowOnMap = false);

            var troopsInStartupPhase = 0;

            do
            {
                troopsInStartupPhase = GetTroopsInStartupPhase();

                if (troopsInStartupPhase > 0)
                {
                    TroopsToDeploy = troopsInStartupPhase;

                    try
                    {
                        var turnManager = GetTurnManager(CurrentPlayer);
                        CurrentPlayer.Deploy(turnManager, troopsInStartupPhase);
                    }
                    catch (Exception e)
                    {
                        Log.AddException(e);
                    }

                    SetNextPlayer();
                }
            } while (troopsInStartupPhase != 0);
        }

        public void DoNextTurn()
        {
            var activePlayerName = CurrentPlayer.Name;
            ClearActions();

            do
            {
                DoNextPhase(false);
            } while (activePlayerName == CurrentPlayer.Name && !GameEnded);
        }

        public void DoNextPhase(bool eraseActions = true)
        {
            if (GameEnded) return;

            if (eraseActions)
            {
                ClearActions();
            }

            var troopsInStartupPhase = GetTroopsInStartupPhase();

            var turnManager = GetTurnManager(CurrentPlayer);
            if (troopsInStartupPhase > 0)
            {
                TroopsToDeploy = troopsInStartupPhase;
                try
                {
                    CurrentPlayer.Deploy(turnManager, troopsInStartupPhase);
                }
                catch (Exception e)
                {
                    Log.AddException(e);
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

                        if (Turn > MaximumAmountOfTurns)
                        {
                            var winner = GetPlayerWithMostCountries();
                            Log.AddMessage(
                                string.Format("----- {0} has won the game with {1} countries, turn limit reached",
                                              winner.Name,
                                              turnManager.GetGameInfo().GetAllCountriesOwnedByPlayer(winner).Count));
                            statistics.AddCurrentGameResults(winner);
                            GameEnded = true;
                            return;
                        }
                    }

                    var troopsToDeploy = new DeploymentCounter(CurrentPlayer, turnManager.GetGameInfo()).GetTroopsToDeploy();
                    Log.AddGameInfo(CurrentPhase, troopsToDeploy);
                    TroopsToDeploy = troopsToDeploy;

                    try
                    {
                        CurrentPlayer.Deploy(turnManager, troopsToDeploy);
                    }
                    catch (Exception e)
                    {
                        Log.AddException(e);
                    }
                }
                else if (CurrentPhase == EPhase.Attack)
                {
                    Log.AddGameInfo(CurrentPhase);

                    try
                    {
                        CurrentPlayer.Attack(turnManager);
                    }
                    catch (Exception e)
                    {
                        Log.AddException(e);
                    }

                    if (PlayerHasWon())
                    {
                        Log.AddMessage(string.Format("----- {0} has won the game with {1} countries", CurrentPlayer.Name,
                                                     turnManager.GetGameInfo().GetAllCountriesOwnedByPlayer(CurrentPlayer).Count));
                        statistics.AddCurrentGameResults(CurrentPlayer);
                        GameEnded = true;
                        return;
                    }
                }
                else if (CurrentPhase == EPhase.Move)
                {
                    Log.AddGameInfo(CurrentPhase);
                    TroopsToMove = 7;

                    try
                    {
                        CurrentPlayer.Move(turnManager);
                    }
                    catch (Exception e)
                    {
                        Log.AddException(e);
                    }

                    SetNextPlayer();
                }

                SetNextPhase();
            }
        }

        public List<int> GetDiceRolls(int number)
        {
            var rolls = new List<int>();

            for (var i = 0; i < number; i++)
            {
                rolls.Add(rnd.Next(1, 7));
            }

            return rolls;
        }

        internal TurnManager GetTurnManager(IPlayer player)
        {
            return new TurnManager(player, this);
        }

        internal GameInformation GetGameInfo()
        {
            return new GameInformation(this);
        }

        private void Initialize(Settings settings)
        {
            MaximumAmountOfTurns = settings.MaximumAmountOfTurns;
            CountriesRequiredToWin = settings.CountriesRequiredToWin;
            SetupPlayers(settings.Players);
            DivideCountries();
            SetRandomPlayer();
        }

        private void SetupPlayers(IEnumerable<IPlayer> players)
        {
            Players = players.ToList().AsReadOnly();
            var numberOfTroopsToDeploy = 50 - (Players.Count * 5);

            playersWithTroopsInStartupPhase = new Dictionary<IPlayer, int>();
            foreach (var player in Players)
            {
                playersWithTroopsInStartupPhase.Add(player, numberOfTroopsToDeploy);
            }
        }

        private int GetTroopsInStartupPhase()
        {
            var troops = playersWithTroopsInStartupPhase[CurrentPlayer];

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

        private void DivideCountries()
        {
            do
            {
                SetNextPlayer(true);
                var countriesWithoutOwner = Countries.Where(country => country.Owner == null).ToList();
                countriesWithoutOwner[rnd.Next(0, countriesWithoutOwner.Count - 1)].Owner = CurrentPlayer;
                playersWithTroopsInStartupPhase[CurrentPlayer]--;
            } while (Countries.Any(country => country.Owner == null));
        }

        private void SetRandomPlayer()
        {
            startPlayer = Players[rnd.Next(0, Players.Count - 1)];
            CurrentPlayer = startPlayer;
        }

        private void ClearActions()
        {
            Actions.ForEach(action => action.ShowOnMap = false);
        }

        private IPlayer GetPlayerWithMostCountries()
        {
            var playerWithMostCountries = CurrentPlayer;
            var count = 0;
            var gameInfo = GetGameInfo();

            foreach (var player in Players)
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

        private bool PlayerHasWon()
        {
            return GetGameInfo().GetAllCountriesOwnedByPlayer(CurrentPlayer).Count >=
                   CountriesRequiredToWin;
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
            var nextPlayer = CurrentPlayer;

            do
            {
                var currentPlayerIndex = Players.IndexOf(nextPlayer);
                nextPlayer = Players[currentPlayerIndex == Players.Count - 1 ? 0 : ++currentPlayerIndex];
            } while (!PlayerIsAlive(nextPlayer) && !init);

            LastPlayer = CurrentPlayer;
            CurrentPlayer = nextPlayer;
        }

        private bool PlayerIsAlive(IPlayer player)
        {
            return GetGameInfo().GetAllCountriesOwnedByPlayer(player).Any();
        }

    }
}