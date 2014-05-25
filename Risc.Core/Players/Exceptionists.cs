using System;
using System.Collections.Generic;
using System.Linq;
using Risk.Core;
using Risk.Models;

namespace Risk.Players
{
    public class Exceptionists : IPlayer
    {
        private readonly Random r = new Random();
        private double DEFENSIVE_THRESHOLD_FOR_ATTACK = .4;
        private double DEFENSIVE_THRESHOLD_FOR_DEPLOYMENT = .5;
        private double MAX_TROOPS_FOR_DEPLOYMENT = 10;
        private double OFFENSIVE_THRESHOLD_FOR_ATTACK = 1;

        private GameInformation gameInformation;

        public string Name
        {
            get { return "Exceptionists"; }
        }

        public string Color
        {
            get { return "Pink"; }
        }

        public void Deploy(TurnManager turnManager, int numberOfTroops)
        {
            gameInformation = turnManager.GetGameInfo();

            //exclude all isolated countries (all countries with only enemy neighbours)
            var countriesToAimAt =
                gameInformation.GetAllCountriesOwnedByPlayer(this)
                               .Where(x => gameInformation.GetAdjacentCountriesWithSameOwner(x).Any())
                               .ToArray();

            //Determine defensiveRate.
            //While there are countries with low defensiveRate (number of own troops / number of surrounding troups), make those stronger.
            var countriesWithEnemyNeighbours =
                countriesToAimAt.Where(x => gameInformation.GetAdjacentCountriesWithDifferentOwner(x).Count > 0)
                                .ToArray();
            var defensiveRates =
                GetDefensiveRates(countriesWithEnemyNeighbours).OrderBy(x => x.Value);

            while (defensiveRates.First().Value < DEFENSIVE_THRESHOLD_FOR_DEPLOYMENT && numberOfTroops > 0)
            {
                turnManager.DeployTroops(defensiveRates.First().Key, 1);
                defensiveRates = GetDefensiveRates(countriesWithEnemyNeighbours).OrderByDescending(x => x.Value);
                numberOfTroops--;
            }

            //Get the continent with the biggest presence.
            //Determine what country has the highest defensiveRate (number of own troops / number of surrounding troups), make those even stronger.
            //Make sure this ratio is not too big.
            var biggestContinent = GetBiggestOwnedContinentWithRemainingEnemyCountries();

            var allOwnCountriesInBiggestContinent =
                countriesToAimAt.Where(x => x.Continent == biggestContinent.Key).ToArray();
            var countriesWithRates =
                GetDefensiveRates(allOwnCountriesInBiggestContinent)
                    .Where(x => x.Key.NumberOfTroops < MAX_TROOPS_FOR_DEPLOYMENT)
                    .OrderByDescending(x => x.Value);

            while (countriesWithRates.Any() && numberOfTroops > 0)
            {
                turnManager.DeployTroops(countriesWithRates.First().Key, 1);
                countriesWithRates =
                    GetDefensiveRates(allOwnCountriesInBiggestContinent)
                        .Where(x => x.Key.NumberOfTroops < MAX_TROOPS_FOR_DEPLOYMENT)
                        .OrderByDescending(x => x.Value);
                numberOfTroops--;
            }

            while (numberOfTroops > 0)
            {
                turnManager.DeployTroops(GetRandomOwnedCountry(turnManager), 1);
                numberOfTroops--;
            }
        }

        public void Attack(TurnManager turnManager)
        {
            gameInformation = turnManager.GetGameInfo();

            var countryToAttackWith = GetCountryToAttackWith();
            while (countryToAttackWith != null)
            {
                turnManager.Attack(countryToAttackWith.Item1, countryToAttackWith.Item2,
                                                          countryToAttackWith.Item1.NumberOfTroops - 1);
                countryToAttackWith = GetCountryToAttackWith();
            }
        }

        public void Move(TurnManager turnManager)
        {
            var countriesToAimAt =
                gameInformation.GetAllCountriesOwnedByPlayer(this)
                               .Where(x => gameInformation.GetAdjacentCountriesWithSameOwner(x).Any())
                               .ToArray();

            if (countriesToAimAt.Any())
            {
                //Determine defensiveRate.
                //While there are countries with low defensiveRate (number of own troops / number of surrounding troups), make those stronger.
                var countriesWithEnemyNeighbours =
                    countriesToAimAt.Where(x => gameInformation.GetAdjacentCountriesWithDifferentOwner(x).Count > 0)
                                    .ToArray();
                var defensiveRates =
                    GetDefensiveRates(countriesWithEnemyNeighbours).OrderBy(x => x.Value);

                var moveCounter = 0;

                while (defensiveRates.Any(x => x.Value < DEFENSIVE_THRESHOLD_FOR_DEPLOYMENT) && moveCounter++ < 250)
                {
                    var first = defensiveRates.First();

                    var adjacentCountriesWithSameOwner =
                        gameInformation.GetAdjacentCountriesWithSameOwner(first.Key)
                                       .Where(
                                           x =>
                                           defensiveRates.Single(y => y.Key == x).Value >
                                           DEFENSIVE_THRESHOLD_FOR_DEPLOYMENT)
                                       .ToArray();
                    if (adjacentCountriesWithSameOwner.Any())
                    {
                        turnManager.MoveTroops(first.Key, adjacentCountriesWithSameOwner.First(),
                                                                      adjacentCountriesWithSameOwner.First()
                                                                                                    .NumberOfTroops - 1);
                    }

                    defensiveRates = GetDefensiveRates(countriesWithEnemyNeighbours).OrderBy(x => x.Value);
                }
            }
        }

        public int Defend(TurnManager turnManager, List<int> attackRolls, Country countryToDefend)
        {
            if (attackRolls.Count(x => x > 3) > 1)
                return 1;

            return 2;
        }

        private Tuple<Country, Country, double> GetCountryToAttackWith()
        {
            //when to attack: If defensiverate is high enough and there is an attackable country with a ratio > 1.5
            var countriesToAimAt =
                gameInformation.GetAllCountriesOwnedByPlayer(this)
                               .Where(x => gameInformation.GetAdjacentCountriesWithSameOwner(x).Any())
                               .ToArray();
            var defensiveRates = GetDefensiveRates(countriesToAimAt);

            var countriesWithHighEnoughDefenseRate =
                defensiveRates.Where(x => x.Value > DEFENSIVE_THRESHOLD_FOR_ATTACK).ToArray();

            var countriesToAttackWith = new List<Tuple<Country, Country, double>>();
            foreach (var country in countriesWithHighEnoughDefenseRate)
            {
                foreach (var enemyCountry in gameInformation.GetAdjacentCountriesWithDifferentOwner(country.Key))
                {
                    var offenceRate = country.Key.NumberOfTroops/enemyCountry.NumberOfTroops;
                    if (offenceRate > OFFENSIVE_THRESHOLD_FOR_ATTACK)
                    {
                        countriesToAttackWith.Add(new Tuple<Country, Country, double>(country.Key, enemyCountry,
                                                                                      offenceRate));
                    }
                }
            }

            return countriesToAttackWith.OrderByDescending(x => x.Item3).First();
        }

        private Country GetRandomOwnedCountry(TurnManager turnManager)
        {
            var ownedCountries = turnManager.GetGameInfo().GetAllCountriesOwnedByPlayer(this);
            return ownedCountries[r.Next(0, ownedCountries.Count - 1)];
        }

        private KeyValuePair<EContinent, double> GetBiggestOwnedContinentWithRemainingEnemyCountries()
        {
            var continents = gameInformation.GetAllCountries().Select(x => x.Continent).Distinct();

            var numberOfCountriesPerContinent = new Dictionary<EContinent, int>();
            foreach (var continent in continents)
            {
                numberOfCountriesPerContinent.Add(continent,
                                                  gameInformation.GetAllCountries().Count(x => x.Continent == continent));
            }

            var continentsWithOwnPresence =
                gameInformation.GetAllCountriesOwnedByPlayer(this).Select(x => x.Continent).Distinct();
            var numberOfOwnCountriesPerContinent = new Dictionary<EContinent, int>();
            foreach (var continent in continentsWithOwnPresence)
            {
                var currentNumber =
                    gameInformation.GetAllCountriesOwnedByPlayer(this).Count(x => x.Continent == continent);
                if (currentNumber == numberOfCountriesPerContinent[continent])
                {
                    continue;
                }
                numberOfOwnCountriesPerContinent.Add(continent, currentNumber);
            }

            var percentageOfOwnCountriesInContinent = new Dictionary<EContinent, double>();
            foreach (var continent in continentsWithOwnPresence)
            {
                percentageOfOwnCountriesInContinent.Add(continent,
                                                        numberOfOwnCountriesPerContinent.Single(x => x.Key == continent)
                                                                                        .Value/
                                                        (double)
                                                        numberOfCountriesPerContinent.Single(x => x.Key == continent)
                                                                                     .Value);
            }

            var biggestContinent = percentageOfOwnCountriesInContinent.First();
            foreach (var currentpercentage in percentageOfOwnCountriesInContinent)
            {
                if (currentpercentage.Value > percentageOfOwnCountriesInContinent[biggestContinent.Key])
                {
                    biggestContinent = currentpercentage;
                }
            }

            return biggestContinent;
        }

        private Dictionary<Country, double> GetDefensiveRates(IEnumerable<Country> countriesToUse)
        {
            var countriesWithOwnTroupEnemyTroupsRate = new Dictionary<Country, double>();
            foreach (var country in countriesToUse)
            {
                countriesWithOwnTroupEnemyTroupsRate.Add(country,
                                                         GetDefensiveRateForCountry(country,
                                                                                    GetNumberOfTotalEnemyTroupsForCountry
                                                                                        (country)));
            }
            return countriesWithOwnTroupEnemyTroupsRate;
        }

        private int GetNumberOfTotalEnemyTroupsForCountry(Country country)
        {
            var numberOfEnemies =
                gameInformation.GetAdjacentCountriesWithDifferentOwner(country).Sum(x => x.NumberOfTroops);
            return numberOfEnemies;
        }

        private double GetDefensiveRateForCountry(Country country, int numberOfTotalEnemyTroopsForCountry)
        {
            return (double) country.NumberOfTroops/numberOfTotalEnemyTroopsForCountry;
        }
    }
}