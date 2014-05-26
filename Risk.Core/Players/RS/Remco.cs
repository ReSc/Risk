using System.Collections.Generic;
using System.Linq;
using Risk.Core;
using Risk.Models;

namespace Risk.Players.RS
{
    /// <summary>
    ///     Player implementation by Remco Schoeman.
    /// </summary>
    public class Remco : IPlayer
    {
        private readonly RiskAnalyzer riskAnalyzer;

        public Remco()
        {
            Name = "Remco";
            Color = "Purple";
            riskAnalyzer = new RiskAnalyzer(this);
        }

        public string Name { get; private set; }
        public string Color { get; private set; }

        public void Deploy(TurnManager turnManager, int numberOfTroops)
        {
            var analysis = riskAnalyzer.Analyze(turnManager);
            analysis.ExecuteDeployments(numberOfTroops);
        }

        public void Attack(TurnManager turnManager)
        {
            var losses = 0;

            while (losses <= 3)
            {
                var analysis = riskAnalyzer.Analyze(turnManager);
                if (analysis.Attacks.Count == 0)
                    return;

                analysis.ExecuteAttacks();
                losses += analysis.Attacks.Count(x => x.Succeeded == false);
            }
        }

        public void Move(TurnManager turnManager)
        {
            var analysis = riskAnalyzer.Analyze(turnManager);
            analysis.ExecuteMoves();
        }

        public int Defend(TurnManager turnManager, List<int> attackRolls, Country countryToDefend)
        {
            var analysis = riskAnalyzer.Analyze(turnManager);
            return analysis.GetDefenceRolls(attackRolls, countryToDefend);
        }
    }
}