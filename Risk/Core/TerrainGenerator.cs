using Risk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Risk.Core
{
    public class TerrainGenerator
    {
        List<Country> countries; 

        public List<Country> Generate()
        {
            countries = new List<Country>();

            countries.Add(new Country() { Continent = EContinent.NorthAmerica, Number = 1, XPositionOnBoard = 80, YPositionOnBoard = 75 });
            countries.Add(new Country() { Continent = EContinent.NorthAmerica, Number = 2, XPositionOnBoard = 140, YPositionOnBoard = 112});
            countries.Add(new Country() { Continent = EContinent.NorthAmerica, Number = 3, XPositionOnBoard = 135, YPositionOnBoard = 250 });
            countries.Add(new Country() { Continent = EContinent.NorthAmerica, Number = 4, XPositionOnBoard = 206, YPositionOnBoard = 185 });
            countries.Add(new Country() { Continent = EContinent.NorthAmerica, Number = 5, XPositionOnBoard = 428, YPositionOnBoard = 36 });
            countries.Add(new Country() { Continent = EContinent.NorthAmerica, Number = 6, XPositionOnBoard = 176, YPositionOnBoard = 66 });
            countries.Add(new Country() { Continent = EContinent.NorthAmerica, Number = 7, XPositionOnBoard = 226, YPositionOnBoard = 118 });
            countries.Add(new Country() { Continent = EContinent.NorthAmerica, Number = 8, XPositionOnBoard = 303, YPositionOnBoard = 117 });
            countries.Add(new Country() { Continent = EContinent.NorthAmerica, Number = 9, XPositionOnBoard = 122, YPositionOnBoard = 171 });

            countries.Add(new Country() { Continent = EContinent.SouthAmerica, Number = 1, XPositionOnBoard = 292, YPositionOnBoard = 506 });
            countries.Add(new Country() { Continent = EContinent.SouthAmerica, Number = 2, XPositionOnBoard = 337, YPositionOnBoard = 408 });
            countries.Add(new Country() { Continent = EContinent.SouthAmerica, Number = 3, XPositionOnBoard = 262, YPositionOnBoard = 428 });
            countries.Add(new Country() { Continent = EContinent.SouthAmerica, Number = 4, XPositionOnBoard = 260, YPositionOnBoard = 328 });

            countries.Add(new Country() { Continent = EContinent.Africa, Number = 1, XPositionOnBoard = 630, YPositionOnBoard = 357 });
            countries.Add(new Country() { Continent = EContinent.Africa, Number = 2, XPositionOnBoard = 705, YPositionOnBoard = 320 });
            countries.Add(new Country() { Continent = EContinent.Africa, Number = 3, XPositionOnBoard = 627, YPositionOnBoard = 231 });
            countries.Add(new Country() { Continent = EContinent.Africa, Number = 4, XPositionOnBoard = 738, YPositionOnBoard = 444 });
            countries.Add(new Country() { Continent = EContinent.Africa, Number = 5, XPositionOnBoard = 540, YPositionOnBoard = 271 });
            countries.Add(new Country() { Continent = EContinent.Africa, Number = 6, XPositionOnBoard = 637, YPositionOnBoard = 446 });

            countries.Add(new Country() { Continent = EContinent.Europe, Number = 1, XPositionOnBoard = 516, YPositionOnBoard = 115 });
            countries.Add(new Country() { Continent = EContinent.Europe, Number = 2, XPositionOnBoard = 475, YPositionOnBoard = 80 });
            countries.Add(new Country() { Continent = EContinent.Europe, Number = 3, XPositionOnBoard = 594, YPositionOnBoard = 100 }); //124
            countries.Add(new Country() { Continent = EContinent.Europe, Number = 4, XPositionOnBoard = 603, YPositionOnBoard = 50 }); //73
            countries.Add(new Country() { Continent = EContinent.Europe, Number = 5, XPositionOnBoard = 624, YPositionOnBoard = 160 });
            countries.Add(new Country() { Continent = EContinent.Europe, Number = 6, XPositionOnBoard = 676, YPositionOnBoard = 110 });
            countries.Add(new Country() { Continent = EContinent.Europe, Number = 7, XPositionOnBoard = 547, YPositionOnBoard = 157 });

            countries.Add(new Country() { Continent = EContinent.Asia, Number = 1, XPositionOnBoard = 790, YPositionOnBoard = 152 });
            countries.Add(new Country() { Continent = EContinent.Asia, Number = 2, XPositionOnBoard = 938, YPositionOnBoard = 204 });
            countries.Add(new Country() { Continent = EContinent.Asia, Number = 3, XPositionOnBoard = 860, YPositionOnBoard = 248 });
            countries.Add(new Country() { Continent = EContinent.Asia, Number = 4, XPositionOnBoard = 935, YPositionOnBoard = 110 });
            countries.Add(new Country() { Continent = EContinent.Asia, Number = 5, XPositionOnBoard = 1100, YPositionOnBoard = 190 });
            countries.Add(new Country() { Continent = EContinent.Asia, Number = 6, XPositionOnBoard = 1063, YPositionOnBoard = 70 });
            countries.Add(new Country() { Continent = EContinent.Asia, Number = 7, XPositionOnBoard = 715, YPositionOnBoard = 200 });
            countries.Add(new Country() { Continent = EContinent.Asia, Number = 8, XPositionOnBoard = 953, YPositionOnBoard = 153 });
            countries.Add(new Country() { Continent = EContinent.Asia, Number = 9, XPositionOnBoard = 967, YPositionOnBoard = 281 });
            countries.Add(new Country() { Continent = EContinent.Asia, Number = 10, XPositionOnBoard = 853, YPositionOnBoard = 67 });
            countries.Add(new Country() { Continent = EContinent.Asia, Number = 11, XPositionOnBoard = 782, YPositionOnBoard = 86 });
            countries.Add(new Country() { Continent = EContinent.Asia, Number = 12, XPositionOnBoard = 948, YPositionOnBoard = 65 });

            countries.Add(new Country() { Continent = EContinent.Australia, Number = 1, XPositionOnBoard = 1130, YPositionOnBoard = 474 });
            countries.Add(new Country() { Continent = EContinent.Australia, Number = 2, XPositionOnBoard = 1017, YPositionOnBoard = 355 });
            countries.Add(new Country() { Continent = EContinent.Australia, Number = 3, XPositionOnBoard = 1135, YPositionOnBoard = 380 });
            countries.Add(new Country() { Continent = EContinent.Australia, Number = 4, XPositionOnBoard = 1035, YPositionOnBoard = 480 });


            GetCountry(EContinent.NorthAmerica, 1).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 2));
            GetCountry(EContinent.NorthAmerica, 1).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 6));
            GetCountry(EContinent.NorthAmerica, 1).AdjacentCountries.Add(GetCountry(EContinent.Asia, 6));

            GetCountry(EContinent.NorthAmerica, 2).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 1));
            GetCountry(EContinent.NorthAmerica, 2).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 3));
            GetCountry(EContinent.NorthAmerica, 2).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 7));
            GetCountry(EContinent.NorthAmerica, 2).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 9));

            GetCountry(EContinent.NorthAmerica, 3).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 9));
            GetCountry(EContinent.NorthAmerica, 3).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 4));
            GetCountry(EContinent.NorthAmerica, 3).AdjacentCountries.Add(GetCountry(EContinent.SouthAmerica, 4));

            GetCountry(EContinent.NorthAmerica, 4).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 3));
            GetCountry(EContinent.NorthAmerica, 4).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 9));
            GetCountry(EContinent.NorthAmerica, 4).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 7));
            GetCountry(EContinent.NorthAmerica, 4).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 8));

            GetCountry(EContinent.NorthAmerica, 5).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 6));
            GetCountry(EContinent.NorthAmerica, 5).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 7));
            GetCountry(EContinent.NorthAmerica, 5).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 8));
            GetCountry(EContinent.NorthAmerica, 5).AdjacentCountries.Add(GetCountry(EContinent.Europe, 2));

            GetCountry(EContinent.NorthAmerica, 6).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 1));
            GetCountry(EContinent.NorthAmerica, 6).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 2));
            GetCountry(EContinent.NorthAmerica, 6).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 7));
            GetCountry(EContinent.NorthAmerica, 6).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 5));

            GetCountry(EContinent.NorthAmerica, 7).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 5));
            GetCountry(EContinent.NorthAmerica, 7).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 6));
            GetCountry(EContinent.NorthAmerica, 7).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 2));
            GetCountry(EContinent.NorthAmerica, 7).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 9));
            GetCountry(EContinent.NorthAmerica, 7).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 4));
            GetCountry(EContinent.NorthAmerica, 7).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 8));

            GetCountry(EContinent.NorthAmerica, 8).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 5));
            GetCountry(EContinent.NorthAmerica, 8).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 7));
            GetCountry(EContinent.NorthAmerica, 8).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 4));

            GetCountry(EContinent.NorthAmerica, 9).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 2));
            GetCountry(EContinent.NorthAmerica, 9).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 7));
            GetCountry(EContinent.NorthAmerica, 9).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 4));
            GetCountry(EContinent.NorthAmerica, 9).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 3));
            
            //--

            GetCountry(EContinent.SouthAmerica, 1).AdjacentCountries.Add(GetCountry(EContinent.SouthAmerica, 2));
            GetCountry(EContinent.SouthAmerica, 1).AdjacentCountries.Add(GetCountry(EContinent.SouthAmerica, 3));

            GetCountry(EContinent.SouthAmerica, 2).AdjacentCountries.Add(GetCountry(EContinent.SouthAmerica, 1));
            GetCountry(EContinent.SouthAmerica, 2).AdjacentCountries.Add(GetCountry(EContinent.SouthAmerica, 3));
            GetCountry(EContinent.SouthAmerica, 2).AdjacentCountries.Add(GetCountry(EContinent.SouthAmerica, 4));
            GetCountry(EContinent.SouthAmerica, 2).AdjacentCountries.Add(GetCountry(EContinent.Africa, 5));

            GetCountry(EContinent.SouthAmerica, 3).AdjacentCountries.Add(GetCountry(EContinent.SouthAmerica, 1));
            GetCountry(EContinent.SouthAmerica, 3).AdjacentCountries.Add(GetCountry(EContinent.SouthAmerica, 2));
            GetCountry(EContinent.SouthAmerica, 3).AdjacentCountries.Add(GetCountry(EContinent.SouthAmerica, 4));

            GetCountry(EContinent.SouthAmerica, 4).AdjacentCountries.Add(GetCountry(EContinent.SouthAmerica, 2));
            GetCountry(EContinent.SouthAmerica, 4).AdjacentCountries.Add(GetCountry(EContinent.SouthAmerica, 3));
            GetCountry(EContinent.SouthAmerica, 4).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 3));

            //--

            GetCountry(EContinent.Africa, 1).AdjacentCountries.Add(GetCountry(EContinent.Africa, 5));
            GetCountry(EContinent.Africa, 1).AdjacentCountries.Add(GetCountry(EContinent.Africa, 2));
            GetCountry(EContinent.Africa, 1).AdjacentCountries.Add(GetCountry(EContinent.Africa, 6));

            GetCountry(EContinent.Africa, 2).AdjacentCountries.Add(GetCountry(EContinent.Africa, 4));
            GetCountry(EContinent.Africa, 2).AdjacentCountries.Add(GetCountry(EContinent.Africa, 6));
            GetCountry(EContinent.Africa, 2).AdjacentCountries.Add(GetCountry(EContinent.Africa, 1));
            GetCountry(EContinent.Africa, 2).AdjacentCountries.Add(GetCountry(EContinent.Africa, 5));
            GetCountry(EContinent.Africa, 2).AdjacentCountries.Add(GetCountry(EContinent.Africa, 3));
            GetCountry(EContinent.Africa, 2).AdjacentCountries.Add(GetCountry(EContinent.Asia, 7));

            GetCountry(EContinent.Africa, 3).AdjacentCountries.Add(GetCountry(EContinent.Africa, 2));
            GetCountry(EContinent.Africa, 3).AdjacentCountries.Add(GetCountry(EContinent.Africa, 5));
            GetCountry(EContinent.Africa, 3).AdjacentCountries.Add(GetCountry(EContinent.Europe, 5));
            GetCountry(EContinent.Africa, 3).AdjacentCountries.Add(GetCountry(EContinent.Asia, 7));
            
            GetCountry(EContinent.Africa, 4).AdjacentCountries.Add(GetCountry(EContinent.Africa, 2));
            GetCountry(EContinent.Africa, 4).AdjacentCountries.Add(GetCountry(EContinent.Africa, 6));

            GetCountry(EContinent.Africa, 5).AdjacentCountries.Add(GetCountry(EContinent.Africa, 3));
            GetCountry(EContinent.Africa, 5).AdjacentCountries.Add(GetCountry(EContinent.Africa, 2));
            GetCountry(EContinent.Africa, 5).AdjacentCountries.Add(GetCountry(EContinent.Africa, 1));
            GetCountry(EContinent.Africa, 5).AdjacentCountries.Add(GetCountry(EContinent.SouthAmerica, 2));
            GetCountry(EContinent.Africa, 5).AdjacentCountries.Add(GetCountry(EContinent.Europe, 7));
            GetCountry(EContinent.Africa, 5).AdjacentCountries.Add(GetCountry(EContinent.Europe, 5));

            GetCountry(EContinent.Africa, 6).AdjacentCountries.Add(GetCountry(EContinent.Africa, 1));
            GetCountry(EContinent.Africa, 6).AdjacentCountries.Add(GetCountry(EContinent.Africa, 2));
            GetCountry(EContinent.Africa, 6).AdjacentCountries.Add(GetCountry(EContinent.Africa, 4));

            //--

            GetCountry(EContinent.Europe, 1).AdjacentCountries.Add(GetCountry(EContinent.Europe, 2));
            GetCountry(EContinent.Europe, 1).AdjacentCountries.Add(GetCountry(EContinent.Europe, 4));
            GetCountry(EContinent.Europe, 1).AdjacentCountries.Add(GetCountry(EContinent.Europe, 3));
            GetCountry(EContinent.Europe, 1).AdjacentCountries.Add(GetCountry(EContinent.Europe, 7));

            GetCountry(EContinent.Europe, 2).AdjacentCountries.Add(GetCountry(EContinent.Europe, 4));
            GetCountry(EContinent.Europe, 2).AdjacentCountries.Add(GetCountry(EContinent.Europe, 1));
            GetCountry(EContinent.Europe, 2).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 5));
            
            GetCountry(EContinent.Europe, 3).AdjacentCountries.Add(GetCountry(EContinent.Europe, 6));
            GetCountry(EContinent.Europe, 3).AdjacentCountries.Add(GetCountry(EContinent.Europe, 5));
            GetCountry(EContinent.Europe, 3).AdjacentCountries.Add(GetCountry(EContinent.Europe, 7));
            GetCountry(EContinent.Europe, 3).AdjacentCountries.Add(GetCountry(EContinent.Europe, 1));
            GetCountry(EContinent.Europe, 3).AdjacentCountries.Add(GetCountry(EContinent.Europe, 4));

            GetCountry(EContinent.Europe, 4).AdjacentCountries.Add(GetCountry(EContinent.Europe, 6));
            GetCountry(EContinent.Europe, 4).AdjacentCountries.Add(GetCountry(EContinent.Europe, 3));
            GetCountry(EContinent.Europe, 4).AdjacentCountries.Add(GetCountry(EContinent.Europe, 1));
            GetCountry(EContinent.Europe, 4).AdjacentCountries.Add(GetCountry(EContinent.Europe, 2));

            GetCountry(EContinent.Europe, 5).AdjacentCountries.Add(GetCountry(EContinent.Europe, 7));
            GetCountry(EContinent.Europe, 5).AdjacentCountries.Add(GetCountry(EContinent.Europe, 3));
            GetCountry(EContinent.Europe, 5).AdjacentCountries.Add(GetCountry(EContinent.Europe, 6));
            GetCountry(EContinent.Europe, 5).AdjacentCountries.Add(GetCountry(EContinent.Asia, 7));
            GetCountry(EContinent.Europe, 5).AdjacentCountries.Add(GetCountry(EContinent.Africa, 3));
            GetCountry(EContinent.Europe, 5).AdjacentCountries.Add(GetCountry(EContinent.Africa, 5));

            GetCountry(EContinent.Europe, 6).AdjacentCountries.Add(GetCountry(EContinent.Europe, 5));
            GetCountry(EContinent.Europe, 6).AdjacentCountries.Add(GetCountry(EContinent.Europe, 3));
            GetCountry(EContinent.Europe, 6).AdjacentCountries.Add(GetCountry(EContinent.Europe, 4));
            GetCountry(EContinent.Europe, 6).AdjacentCountries.Add(GetCountry(EContinent.Asia, 11));
            GetCountry(EContinent.Europe, 6).AdjacentCountries.Add(GetCountry(EContinent.Asia, 1));
            GetCountry(EContinent.Europe, 6).AdjacentCountries.Add(GetCountry(EContinent.Asia, 7));

            GetCountry(EContinent.Europe, 7).AdjacentCountries.Add(GetCountry(EContinent.Europe, 1));
            GetCountry(EContinent.Europe, 7).AdjacentCountries.Add(GetCountry(EContinent.Europe, 3));
            GetCountry(EContinent.Europe, 7).AdjacentCountries.Add(GetCountry(EContinent.Europe, 5));
            GetCountry(EContinent.Europe, 7).AdjacentCountries.Add(GetCountry(EContinent.Africa, 5)); 

            //--
            GetCountry(EContinent.Asia, 1).AdjacentCountries.Add(GetCountry(EContinent.Asia, 11));
            GetCountry(EContinent.Asia, 1).AdjacentCountries.Add(GetCountry(EContinent.Asia, 2));
            GetCountry(EContinent.Asia, 1).AdjacentCountries.Add(GetCountry(EContinent.Asia, 3));
            GetCountry(EContinent.Asia, 1).AdjacentCountries.Add(GetCountry(EContinent.Asia, 7));
            GetCountry(EContinent.Asia, 1).AdjacentCountries.Add(GetCountry(EContinent.Europe, 6));

            GetCountry(EContinent.Asia, 2).AdjacentCountries.Add(GetCountry(EContinent.Asia, 9));
            GetCountry(EContinent.Asia, 2).AdjacentCountries.Add(GetCountry(EContinent.Asia, 3));
            GetCountry(EContinent.Asia, 2).AdjacentCountries.Add(GetCountry(EContinent.Asia, 1));
            GetCountry(EContinent.Asia, 2).AdjacentCountries.Add(GetCountry(EContinent.Asia, 11));
            GetCountry(EContinent.Asia, 2).AdjacentCountries.Add(GetCountry(EContinent.Asia, 10));
            GetCountry(EContinent.Asia, 2).AdjacentCountries.Add(GetCountry(EContinent.Asia, 8));

            GetCountry(EContinent.Asia, 3).AdjacentCountries.Add(GetCountry(EContinent.Asia, 7));
            GetCountry(EContinent.Asia, 3).AdjacentCountries.Add(GetCountry(EContinent.Asia, 1));
            GetCountry(EContinent.Asia, 3).AdjacentCountries.Add(GetCountry(EContinent.Asia, 2));
            GetCountry(EContinent.Asia, 3).AdjacentCountries.Add(GetCountry(EContinent.Asia, 9));

            GetCountry(EContinent.Asia, 4).AdjacentCountries.Add(GetCountry(EContinent.Asia, 12));
            GetCountry(EContinent.Asia, 4).AdjacentCountries.Add(GetCountry(EContinent.Asia, 6));
            GetCountry(EContinent.Asia, 4).AdjacentCountries.Add(GetCountry(EContinent.Asia, 8));
            GetCountry(EContinent.Asia, 4).AdjacentCountries.Add(GetCountry(EContinent.Asia, 10));

            GetCountry(EContinent.Asia, 5).AdjacentCountries.Add(GetCountry(EContinent.Asia, 6));
            GetCountry(EContinent.Asia, 5).AdjacentCountries.Add(GetCountry(EContinent.Asia, 8));

            GetCountry(EContinent.Asia, 6).AdjacentCountries.Add(GetCountry(EContinent.Asia, 5));
            GetCountry(EContinent.Asia, 6).AdjacentCountries.Add(GetCountry(EContinent.Asia, 8));
            GetCountry(EContinent.Asia, 6).AdjacentCountries.Add(GetCountry(EContinent.Asia, 4));
            GetCountry(EContinent.Asia, 6).AdjacentCountries.Add(GetCountry(EContinent.Asia, 12));
            GetCountry(EContinent.Asia, 6).AdjacentCountries.Add(GetCountry(EContinent.NorthAmerica, 1));

            GetCountry(EContinent.Asia, 7).AdjacentCountries.Add(GetCountry(EContinent.Asia, 1));
            GetCountry(EContinent.Asia, 7).AdjacentCountries.Add(GetCountry(EContinent.Asia, 3));
            GetCountry(EContinent.Asia, 7).AdjacentCountries.Add(GetCountry(EContinent.Africa, 2));
            GetCountry(EContinent.Asia, 7).AdjacentCountries.Add(GetCountry(EContinent.Africa, 3));
            GetCountry(EContinent.Asia, 7).AdjacentCountries.Add(GetCountry(EContinent.Europe, 5));
            GetCountry(EContinent.Asia, 7).AdjacentCountries.Add(GetCountry(EContinent.Europe, 6));

            GetCountry(EContinent.Asia, 8).AdjacentCountries.Add(GetCountry(EContinent.Asia, 5));
            GetCountry(EContinent.Asia, 8).AdjacentCountries.Add(GetCountry(EContinent.Asia, 2));
            GetCountry(EContinent.Asia, 8).AdjacentCountries.Add(GetCountry(EContinent.Asia, 10));
            GetCountry(EContinent.Asia, 8).AdjacentCountries.Add(GetCountry(EContinent.Asia, 4));
            GetCountry(EContinent.Asia, 8).AdjacentCountries.Add(GetCountry(EContinent.Asia, 6));

            GetCountry(EContinent.Asia, 9).AdjacentCountries.Add(GetCountry(EContinent.Asia, 3));
            GetCountry(EContinent.Asia, 9).AdjacentCountries.Add(GetCountry(EContinent.Asia, 2));
            GetCountry(EContinent.Asia, 9).AdjacentCountries.Add(GetCountry(EContinent.Australia, 2));

            GetCountry(EContinent.Asia, 10).AdjacentCountries.Add(GetCountry(EContinent.Asia, 12));
            GetCountry(EContinent.Asia, 10).AdjacentCountries.Add(GetCountry(EContinent.Asia, 4));
            GetCountry(EContinent.Asia, 10).AdjacentCountries.Add(GetCountry(EContinent.Asia, 8));
            GetCountry(EContinent.Asia, 10).AdjacentCountries.Add(GetCountry(EContinent.Asia, 2));
            GetCountry(EContinent.Asia, 10).AdjacentCountries.Add(GetCountry(EContinent.Asia, 11));

            GetCountry(EContinent.Asia, 11).AdjacentCountries.Add(GetCountry(EContinent.Asia, 10));
            GetCountry(EContinent.Asia, 11).AdjacentCountries.Add(GetCountry(EContinent.Asia, 2));
            GetCountry(EContinent.Asia, 11).AdjacentCountries.Add(GetCountry(EContinent.Asia, 1));
            GetCountry(EContinent.Asia, 11).AdjacentCountries.Add(GetCountry(EContinent.Europe, 6));

            GetCountry(EContinent.Asia, 12).AdjacentCountries.Add(GetCountry(EContinent.Asia, 6));
            GetCountry(EContinent.Asia, 12).AdjacentCountries.Add(GetCountry(EContinent.Asia, 4));
            GetCountry(EContinent.Asia, 12).AdjacentCountries.Add(GetCountry(EContinent.Asia, 10));

            //--

            GetCountry(EContinent.Australia, 1).AdjacentCountries.Add(GetCountry(EContinent.Australia, 3));
            GetCountry(EContinent.Australia, 1).AdjacentCountries.Add(GetCountry(EContinent.Australia, 4));

            GetCountry(EContinent.Australia, 2).AdjacentCountries.Add(GetCountry(EContinent.Australia, 3));
            GetCountry(EContinent.Australia, 2).AdjacentCountries.Add(GetCountry(EContinent.Australia, 4));
            GetCountry(EContinent.Australia, 2).AdjacentCountries.Add(GetCountry(EContinent.Asia, 9));

            GetCountry(EContinent.Australia, 3).AdjacentCountries.Add(GetCountry(EContinent.Australia, 2));
            GetCountry(EContinent.Australia, 3).AdjacentCountries.Add(GetCountry(EContinent.Australia, 1));
            GetCountry(EContinent.Australia, 3).AdjacentCountries.Add(GetCountry(EContinent.Australia, 4));

            GetCountry(EContinent.Australia, 4).AdjacentCountries.Add(GetCountry(EContinent.Australia, 1));
            GetCountry(EContinent.Australia, 4).AdjacentCountries.Add(GetCountry(EContinent.Australia, 3));
            GetCountry(EContinent.Australia, 4).AdjacentCountries.Add(GetCountry(EContinent.Australia, 2));

            return countries;
        }

        private Country GetCountry(EContinent continent, int number)
        {
            return countries.Single(country => country.Continent == continent && country.Number == number);
        }


    }
}