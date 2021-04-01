using NUnit.Framework;
using PortfolioManager.Models;

namespace PortfolioManagerTests.Models
{
    [TestFixture]
    class LatestTwoTickerPricesTests
    {
        private LatestTwoTickerPrices _latestTwoTickerPrices;

        [SetUp]
        public void Init()
        {
            _latestTwoTickerPrices = new LatestTwoTickerPrices { LatestQuote = new Quote { Close = 10 }, PreviousQuote = new Quote{Close = 20}};
        }

        [Test]
        public void LatestPrice_Should_Return_Latest_Closing_Price()
        {
            Assert.AreEqual(10,_latestTwoTickerPrices.LatestPrice);
        }

        [Test]
        public void PreviousPrice_Should_Return_Previous_Closing_Price()
        {
            Assert.AreEqual(20, _latestTwoTickerPrices.PreviousPrice);
        }
    }
}
