namespace Risk.Models.Continents
{
    public class SouthAmerica
    {
        public SouthAmerica()
        {
            country1 = new Country();
            country2 = new Country();
            country3 = new Country();
            country4 = new Country();
        }

        public Country country1 { get; set; }
        public Country country2 { get; set; }
        public Country country3 { get; set; }
        public Country country4 { get; set; }
    }
}