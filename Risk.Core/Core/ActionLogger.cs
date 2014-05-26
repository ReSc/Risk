using System;
using System.Linq;
using Risk.Models;
using Risk.Models.Actions;

namespace Risk.Core
{
    public class ActionLogger
    {
        private readonly GameManager gameManager;

        public ActionLogger(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void AddDeployAction(Country country, int addedTroops)
        {
            var existingDeployActions =
                gameManager.Actions.Where(x => x is DeployAction).Cast<DeployAction>();
            var existingActionThisTurn =
                existingDeployActions.FirstOrDefault(x => x.CountryToDeploy == country && x.ShowOnMap);

            if (existingActionThisTurn != null)
            {
                existingActionThisTurn.NumberOfAddedTroops += addedTroops;
            }
            else
            {
                gameManager.Actions.Add(new DeployAction
                    {
                        CountryToDeploy = country,
                        NumberOfAddedTroops = addedTroops,
                        Player = gameManager.CurrentPlayer,
                        ShowOnMap = true
                    });
            }
        }

        public void AddMoveAction(Country country, Country countryToMoveTo, int numberOfTroops)
        {
            gameManager.Actions.Add(new MoveAction
                {
                    Country = country,
                    CountryToMoveTo = countryToMoveTo,
                    NumberOfTroopsToMove = numberOfTroops,
                    Player = gameManager.CurrentPlayer
                });
        }

        public void AddGameInfo(EPhase phase, int troopsToDeploy = 0)
        {
            gameManager.Actions.Add(new GameAction
                {
                    Player = gameManager.CurrentPlayer,
                    Phase = phase,
                    TroopsToDeploy = troopsToDeploy,
                    Turn = gameManager.Turn
                });
        }

        public void AddException(Exception e)
        {
            AddException(e, gameManager.CurrentPlayer);
        }

        public void AddException(Exception e, IPlayer player)
        {
            gameManager.Actions.Add(new InfoAction
                {
                    Player = player,
                    Message =
                        string.Format("@@@ {0} caused the following exception: {1}", gameManager.CurrentPlayer.Name,
                                      e.Message)
                });
        }

        public void AddMessage(string message)
        {
            gameManager.Actions.Add(new InfoAction
                {
                    Player = gameManager.CurrentPlayer,
                    Message = message
                });
        }
    }
}