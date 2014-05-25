using System;
using System.Collections.Generic;
using System.Linq;
using Risk.Models;

namespace Risk.Players.RS
{
    /// <summary>
    ///     RiskAnalysis contains the analyzed state of the board,
    ///     and the Deployments, Attacks and Moves to be executed
    /// </summary>
    /// <remarks>
    ///     It's mostly lists for now, if we need better datastructures they can be changed...
    /// </remarks>
    public class RiskAnalysis
    {
        public RiskAnalysis()
        {
            Moves = new List<Move>();
            Attacks = new List<Attack>();
            Deployments = new List<Deployment>();
            Continents = new List<Continent>();
            ImportantCountries = new HashSet<Country>();
        }

        public List<Move> Moves { get; private set; }
        public List<Attack> Attacks { get; private set; }
        public List<Continent> Continents { get; private set; }
        public List<Deployment> Deployments { get; private set; }

        public HashSet<Country> ImportantCountries { get; private set; }


        public void ExecuteMoves()
        {
            foreach (var move in Moves)
                move.Execute();
        }

        public void ExecuteDeployments(int numberOfTroops)
        {
            var availableTroops = numberOfTroops;
            foreach (var deployment in Deployments)
            {
                if (availableTroops <= 0)
                {
                    break;
                }

                if (availableTroops < deployment.Troops)
                {
                    // not so polite, but hey...
                    deployment.Troops = availableTroops;
                }

                deployment.Execute();
                availableTroops -= deployment.Troops;
            }
        }

        public void ExecuteAttacks()
        {
            var losses = 0;
            foreach (var attack in Attacks)
            {
                attack.Execute();
                if (attack.Succeeded == false)
                    losses++;

                if (losses >= 2)
                    break;
            }
        }

        public int GetDefenceRolls(List<int> attackRolls, Country countryToDefend)
        {
            const int minNumberOfDefenders = 1;
            var maxNumberofDefenders = Math.Min(countryToDefend.NumberOfTroops, 2);

            if (IsImportant(countryToDefend))
            {
                return maxNumberofDefenders;
            }

            // more defence rolls than attack rolls increase the chances for a succesfull defence
            if (attackRolls.Count < countryToDefend.NumberOfTroops)
            {
                return maxNumberofDefenders;
            }

            // make defeat take longer by dragging out the attack.
            return minNumberOfDefenders;
        }

        /// <summary>
        ///     Returns true if the country is important to defend.
        ///     for example :
        ///     - losing it would destroy a continent troop bonus.
        ///     - it's an attack launch site
        ///     - it's a deployment site
        ///     - it's involved in a move.
        ///     The analyzer determines which countries are important.
        /// </summary>
        public bool IsImportant(Country country)
        {
            return ImportantCountries.Contains(country);
        }

        /// <summary>
        ///     Returns the continents sorted by descending dominance.
        /// </summary>
        public IEnumerable<Continent> GetContinentsByDominance()
        {
            return Continents.OrderByDescending(x => x.Dominance).ThenByDescending(x => x.MyCountries.Sum(c => c.NumberOfTroops));
        }
    }
}