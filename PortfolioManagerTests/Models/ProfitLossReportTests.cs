using System;
using System.Collections.Generic;
using NUnit.Framework;
using PortfolioManager.Models;

namespace PortfolioManagerTests.Models
{
    [TestFixture]
    public class ProfitLossReportTests
    {
        private ProfitLossReport _profitLossReport;

        [SetUp]
        public void Init()
        {
            _profitLossReport = new ProfitLossReport("GOOG", new List<EquityTransaction>
            {
                new EquityTransaction
                {
                    Quantity = 10,
                    Action = "Buy",
                    Price = 2
                },
                new EquityTransaction
                {
                    Quantity = 20,
                    Action = "Buy",
                    Price = 4
                }
            }, new LatestTwoTickerPrices
            {
                LatestDate = new DateTime(2021, 03, 29),
                LatestQuote = new Quote { Close = 3 },
                PreviousQuote = new Quote { Close = 4 }
            });
        }

        [Test]
        public void Ticker_Should_Return_CorrectTicker()
        {
            Assert.AreEqual("GOOG", _profitLossReport.Ticker);
        }

        [Test]
        public void AsOfDate_Should_Return_LatestDateFrom_TickerPrices()
        {
            Assert.AreEqual("29/03/2021", _profitLossReport.AsOfDate);
        }

        [Test]
        public void PriceShould_Return_LatestPriceFrom_TickerPrices()
        {
            Assert.AreEqual(3m, _profitLossReport.Price);
        }

        [Test]
        public void PrevPrice_Should_Return_PrevPriceFrom_TickerPrices()
        {
            Assert.AreEqual(4m, _profitLossReport.PrevPrice);
        }

        [Test]
        public void Quantity_Should_Return_SumOfUnitsBought_WhenBothActionsAreBuy()
        {
            Assert.AreEqual(30, _profitLossReport.Quantity);
        }

        [Test]
        public void Cost_Should_Return_SumOfCostIncurred()
        {
            Assert.AreEqual(100m, _profitLossReport.Cost);
        }

        [Test]
        public void MarketValue_Should_Return_LatestPrice_Times_Quantity()
        {
            Assert.AreEqual(90m, _profitLossReport.MarketValue);
        }

        [Test]
        public void DailyPandL_Should_Return_DailyPriceDiff_Times_Quantity()
        {
           
            Assert.AreEqual(-30m, _profitLossReport.DailyPandL);
        }

        [Test]
        public void InceptionPandL_Should_Return_PurchaseCost_Minus_CurrentMarketValue()
        {
            Assert.AreEqual(-10m, _profitLossReport.InceptionPandL);
        }

        [Test]
        public void Quantity_Should_Return_SumOfUnitsBought_WhenSomeAreSell()
        {
            //Arrange
            var profitLossReport = new ProfitLossReport("GOOG", new List<EquityTransaction>
            {
                new EquityTransaction
                {
                    Quantity = 10,
                    Action = "Buy"
                },
                new EquityTransaction
                {
                    Quantity = 4,
                    Action = "Sell"
                }
            }, new LatestTwoTickerPrices
            {
                LatestDate = new DateTime(2021, 03, 29)
            });

            //Assert
            Assert.AreEqual(6, profitLossReport.Quantity);
        }
    }
}
