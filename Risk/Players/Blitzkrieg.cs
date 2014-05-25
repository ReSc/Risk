using System.Security.Cryptography;
using System.Text;
using Risk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Risk.Models;

namespace Risk.Players
{
    public class BlitzKrieg : IPlayer
    {
        public string Name
        {
            get { return "BlitzKrieg"; }
        }

        public string Color
        {
            get { return "BLUE"; }
        }


        private Random randomizer = new Random();

        public void Deploy(int numberOfTroops)
        {
            DetermineOwnershipPercentageForContinents();
            var preferencedContinent = GetPreferencedContinent();
            var countriesInPreferencedContinent = new GameInformation().GetAllCountriesOwnedByPlayer(this).Where(x => x.Continent.Equals(preferencedContinent)).ToList();

            var frontierCountries = (from frontierCountry in countriesInPreferencedContinent
                                     let enemyCountries = new GameInformation().GetAdjacentCountriesWithDifferentOwner(frontierCountry)
                                     where enemyCountries.Any()
                                     orderby enemyCountries.Count
                                     select frontierCountry).ToList();

            int troopsLeft = numberOfTroops;

            while (troopsLeft > 0)
            {
                var countryIndex = randomizer.Next(frontierCountries.Count());
                var country = frontierCountries[countryIndex];
                new TurnManager(this).DeployTroops(country, 1);

                troopsLeft--;
            }
        }

        private EContinent GetPreferencedContinent()
        {
            var continents = continentsOwnership.OrderByDescending(x => x.Value);
            var continent = continents.First(x => x.Value < 85).Key;
            if (continent == EContinent.Asia && continents.All(x => x.Value != 100))
            {
                return continents.ElementAt(1).Key;
            }
            return continent;
        }

        private Dictionary<EContinent, double> continentsOwnership = new Dictionary<EContinent, double>();

        private void DetermineOwnershipPercentageForContinents()
        {
            continentsOwnership = new Dictionary<EContinent, double>();
            foreach (var uniqueContinent in new GameInformation().GetAllCountries().Select(x => x.Continent).Distinct())
            {
                double numberOfCountriesInContinent =
                    new GameInformation().GetAllCountries().Count(x => x.Continent.Equals(uniqueContinent));
                double ourNumberOfCountriesInContinent =
                    new GameInformation().GetAllCountriesOwnedByPlayer(this).Count(x => x.Continent.Equals(uniqueContinent));
                double ownershipPercentage = (ourNumberOfCountriesInContinent / numberOfCountriesInContinent) * 100;

                continentsOwnership.Add(uniqueContinent, ownershipPercentage);
            }
        }

        private double DetermineVictoryChance(Country attacker, Country defender)
        {
            int minDefenders = 1;

            var attackers = attacker.NumberOfTroops - minDefenders;
            var defenders = defender.NumberOfTroops;
            return attackers / defenders;

        }

        private Country GetPossibleVictim(Country attacker, IEnumerable<Country> adjecentEnemies)
        {
            return adjecentEnemies.Where(x => DetermineVictoryChance(attacker, x) > 1.1).OrderByDescending(x => DetermineVictoryChance(attacker, x)).FirstOrDefault();
        }

        public void Attack()
        {
            /// [Wil je aanvallen?]
            /// 1. Als je maar net genoeg troepen hebt om je continent te verdedigen, niet of beperkt aanvallen.
            /// 2. Is er 1 of zijn er n landen om aan te vallen?
            /// 3. var countryToAttack = aanvalBareLanden.Select(x =>  { bepaal aanvalkans % o.b.v. onze troepen vs de troepen van de tegenstander }.OrderByDescending().FirstOrDefault()
            /// 4. Val land aan

            /// 5. Beoordeel resultaat, wil ik nogmaals aanvallen? Of Doel veranderen?

            try
            {
                var preferencedContinent = GetPreferencedContinent();
                var ourCountries =
                    new GameInformation().GetAllCountries()
                        .Where(x => x.Continent.Equals(preferencedContinent) && x.Owner == this);

                var adjecentEnemies = from ourCountry in ourCountries
                                      let enemyCountries = new GameInformation().GetAdjacentCountriesWithDifferentOwner(ourCountry)
                                      select new { OurCountry = ourCountry, EnemyCountry = GetPossibleVictim(ourCountry, enemyCountries) };

                bool victoryIsReachable = adjecentEnemies.Any(x => x.EnemyCountry != null);

                int roundsLost = 0;

                while (victoryIsReachable)
                {

                    foreach (var roundOfAttack in adjecentEnemies.Where(x => x.EnemyCountry != null))
                    {
                        if (!new TurnManager(this).Attack(roundOfAttack.OurCountry, roundOfAttack.EnemyCountry, roundOfAttack.OurCountry.NumberOfTroops - 1))
                        {
                            roundsLost++;
                        }

                        if (roundsLost >= 5)
                        {
                            return;
                        }
                    }

                    ourCountries = new GameInformation().GetAllCountries()
                        .Where(x => x.Continent.Equals(preferencedContinent) && x.Owner == this);

                    adjecentEnemies = from ourCountry in ourCountries
                                      let enemyCountries = new GameInformation().GetAdjacentCountriesWithDifferentOwner(ourCountry)
                                      select new { OurCountry = ourCountry, EnemyCountry = GetPossibleVictim(ourCountry, enemyCountries) };

                    victoryIsReachable = adjecentEnemies.Any(x => x.EnemyCountry != null);
                }

            }
            catch (RiskException)
            {
                //Attack();
            }
        }

        public void Move()
        {
            var countries = new GameInformation().GetAllCountriesOwnedByPlayer(this).OrderByDescending(x => x.NumberOfTroops);

            var frontierCountries = from frontierCountry in countries
                                    let enemyCountries = new GameInformation().GetAdjacentCountriesWithDifferentOwner(frontierCountry)
                                    where enemyCountries.Any()
                                    orderby enemyCountries.Count
                                    select frontierCountry;

            var frontierCountryFriendsWithoutEnemies = from frontierCountry in frontierCountries
                                                       let friendCountries = frontierCountry.AdjacentCountries.Where(x => x.Owner == this && x.NumberOfTroops > 2 && x.AdjacentCountries.All(x1 => x1.Owner == this))
                                                       select new { FrontierCountry = frontierCountry, Friends = friendCountries };

            foreach (var frontierCountryFriendsWithoutEnemy in frontierCountryFriendsWithoutEnemies)
            {
                var countryIndex = randomizer.Next(0, frontierCountryFriendsWithoutEnemy.Friends.Count());
                if (frontierCountryFriendsWithoutEnemy.Friends.Any())
                {
                    new TurnManager(this).MoveTroops(frontierCountryFriendsWithoutEnemy.Friends.ToList()[countryIndex],
                        frontierCountryFriendsWithoutEnemy.FrontierCountry, 1);
                }
            }
        }

        public int Defend(List<int> attackRolls, Country countryToDefend)
        {
            return attackRolls.Sum() > 10 ? 1 : 2;
        }
    }
}