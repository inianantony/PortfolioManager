using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using PortfolioManager.Cache;
using PortfolioManager.Models;
using PortfolioManager.Services;

namespace PortfolioManagerTests.Services
{
    [TestFixture]
    class ProfitLossReportBuilderTests
    {
        private EquityPrices _equityPrices;
        private Quote _firstQuote;
        private Quote _secondQuote;
        private Quote _thirdQuote;
        private DateTime _firstDate;
        private DateTime _secondDate;
        private DateTime _thirdDate;
        private Mock<IEquitiesPriceCache> _cacheMock;

        [SetUp]
        public void Init()
        {
            _firstQuote = new Quote { Close = 10 };
            _secondQuote = new Quote { Close = 9 };
            _thirdQuote = new Quote { Close = 8 };
            _firstDate = new DateTime(2021, 03, 29);
            _secondDate = _firstDate.AddDays(-1);
            _thirdDate = _firstDate.AddDays(-2);
            _equityPrices = new EquityPrices
            {
                TimeSeriesDaily = new Dictionary<DateTime, Quote>
                {
                    {_firstDate, _firstQuote },
                    {_secondDate, _secondQuote },
                    {_thirdDate, _thirdQuote },
                    {_thirdDate.AddDays(-1), _firstQuote },
                    {_thirdDate.AddDays(-2), _secondQuote },
                    {_thirdDate.AddDays(-3), _thirdQuote },
                    {_thirdDate.AddDays(-4), _firstQuote },
                    {_thirdDate.AddDays(-5), _secondQuote },
                    {_thirdDate.AddDays(-6), _thirdQuote },
                    {_thirdDate.AddDays(-7), _firstQuote },
                    {_thirdDate.AddDays(-8), _secondQuote },
                    {_thirdDate.AddDays(-9), _thirdQuote },
                    {_thirdDate.AddDays(-10), _firstQuote },
                    {_thirdDate.AddDays(-11), _secondQuote },
                    {_thirdDate.AddDays(-12), _thirdQuote },
                }
            };
            _cacheMock = new Mock<IEquitiesPriceCache>();
            _cacheMock.Setup(a => a.GetEquityPrice(
                It.IsAny<string>())).Returns(_equityPrices);
        }

        [Test]
        public void GetProfitLossReport_Tests()
        {
            var reportBuilder = new ProfitLossReportBuilder(_cacheMock.Object);

            var transactionGroups = new Dictionary<string, List<EquityTransaction>>
            {
                {
                    "GOOG", new List<EquityTransaction>
                    {
                        new EquityTransaction
                        {
                            Quantity = 10,
                            Action = "Buy",
                            Price = 2,
                            TradeDate = _firstDate
                        },
                        new EquityTransaction
                        {
                            Quantity = 20,
                            Action = "Buy",
                            Price = 4,
                            TradeDate = _secondDate
                        },
                        new EquityTransaction
                        {
                            Quantity = 20,
                            Action = "Buy",
                            Price = 4,
                            TradeDate = _thirdDate
                        }
                    }
                }
            };

            var profitLossReport = reportBuilder.GetProfitLossReport(transactionGroups);

            var expected = new List<object>
            {
                new
                {
                    AsOfDate = "29/03/2021",
                    Cost = 180,
                    DailyPandL = 50M,
                    InceptionPandL = 320M,
                    MarketValue = 500M,
                    PrevPrice = 9M,
                    Price = 10M,
                    Quantity = 50,
                    Ticker = "GOOG"
                }
            };

            profitLossReport.Should().NotBeEmpty()
                .And.HaveCount(1)
                .And.BeEquivalentTo(expected);
        }
    }
}
