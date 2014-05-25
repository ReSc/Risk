using NUnit.Framework;
using Risk.Players;
using Risk.Players.RS;

namespace Risk.Core.Tests
{
    [TestFixture]
    public class RiskAnalyzerTests
    {
        private GameManager game;

        [SetUp]
        public void setup()
        {
            // only play against pro's, we're competitive like that ;-)

            var statistics = new Statistics();
            var settings = new Settings(new IPlayer[]
                {
                    new Remco(),
                    new Pros(),
                    new Pros(),
                    new Pros(),
                    new Pros()
                }, 25, 100);

            game = new GameManager(statistics, settings, init: false);


        }
    }
}
