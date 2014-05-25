using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Risk.Models.Continents
{
    public class Asia
    {
        public Country country1 { get; set; }
        public Country country2 { get; set; }
        public Country country3 { get; set; }
        public Country country4 { get; set; }
        public Country country5 { get; set; }
        public Country country6 { get; set; }
        public Country country7 { get; set; }
        public Country country8 { get; set; }
        public Country country9 { get; set; }
        public Country country10 { get; set; }
        public Country country11 { get; set; }
        public Country country12 { get; set; }

        public Asia()
        {
            country1 = new Country();
            country2 = new Country();
            country3 = new Country();
            country4 = new Country();
            country5 = new Country();
            country6 = new Country();
            country7 = new Country();
            country8 = new Country();
            country9 = new Country();
            country10 = new Country();
            country11 = new Country();
            country12 = new Country();
        }
    }
}