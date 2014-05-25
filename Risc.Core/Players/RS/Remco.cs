using System.Collections.Generic;
using System.Linq;
using Risk.Core;
using Risk.Models;

namespace Risk.Players.RS
{
    /// <summary>
    ///     Player implementation by Remco Schoeman.
    /// </summary>
    /// <remarks>
    ///     - Thread safety
    ///     Due to the use of a singleton with no locking or other access control for the global game state
    ///     and the potential of multiple thread/requests running at the same time
    ///     this project is not thread-safe. Some form of paralellism control should be implemented.
    ///     During my "monkey-test" this resulted in exceptions by clicking Next Turn rapidly.
    ///     I would recommend a combination of an eventloop commands and result futures and run the entire game on the eventloop.
    ///     The resulting game state should be deep-copied before returning it to the UI for display.
    ///     Using explicit locking increases complexity more.
    ///     - Testing
    ///     Due to the use of a singleton for GameManager and the way TurnManager and GameInformation are constructed and used
    ///     unit-testing is unneccessarily difficult.
    ///     - Game Mechanics
    ///     Because the IPlayer.Defend(...) method does not include the attacking country as argument
    /// </remarks>
    public class Remco : IPlayer
    {
        private readonly RiskAnalyzer _riskAnalyzer;

        public Remco()
        {
            Name = "Remco";
            Color = "Purple";
            _riskAnalyzer = new RiskAnalyzer(this);
        }

        public string Name { get; private set; }
        public string Color { get; private set; }

        public void Deploy(GameManager gameManager, int numberOfTroops)
        {
            var analysis = _riskAnalyzer.Analyze(gameManager, new GameInformation(gameManager));
            analysis.ExecuteDeployments(numberOfTroops);
        }

        public void Attack(GameManager gameManager)
        {
            var losses = 0;

            while (losses <= 3)
            {
                var analysis = _riskAnalyzer.Analyze(gameManager, new GameInformation(gameManager));
                if (analysis.Attacks.Count == 0)
                    return;

                analysis.ExecuteAttacks();
                losses += analysis.Attacks.Count(x => x.Succeeded == false);
            }
        }

        public void Move(GameManager gameManager)
        {
            var analysis = _riskAnalyzer.Analyze(gameManager, new GameInformation(gameManager));
            analysis.ExecuteMoves();
        }

        public int Defend(GameManager gameManager, List<int> attackRolls, Country countryToDefend)
        {
            var analysis = _riskAnalyzer.Analyze(gameManager, new GameInformation(gameManager));
            return analysis.GetDefenceRolls(attackRolls, countryToDefend);
        }
    }
}