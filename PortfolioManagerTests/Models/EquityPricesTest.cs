using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using PortfolioManager.Models;

namespace PortfolioManagerTests.Models
{
    [TestFixture]
    class EquityPricesTest
    {
        private EquityPrices _equityPrices;
        private Quote _firstQuote;
        private Quote _secondQuote;
        private Quote _thirdQuote;
        private DateTime _firstDate;
        private DateTime _secondDate;
        private DateTime _thirdDate;

        [SetUp]
        public void Init()
        {
            _firstQuote = new Quote{Close = 10};
            _secondQuote = new Quote{Close = 9};
            _thirdQuote = new Quote{Close = 8};
            _firstDate = new DateTime(2021,03,29);
            _secondDate = new DateTime(2021,03,28);
            _thirdDate = new DateTime(2021,03,27);
            _equityPrices = new EquityPrices { TimeSeriesDaily = new Dictionary<DateTime, Quote>
            {
                {_firstDate, _firstQuote },
                {_secondDate, _secondQuote },
                {_thirdDate, _thirdQuote },
            } };
        }

        [Test]
        public void GetTwoTickerPricesByRange_ShouldGive_First_Two_Quoutes_For_0_1_Range()
        {
            var expected = new LatestTwoTickerPrices
            {
               LatestQuote = _firstQuote,
               LatestDate = _firstDate,
               PreviousQuote = _secondQuote,
               PreviousDate = _secondDate
            };

            _equityPrices.GetTwoTickerPricesByRange(0,1).Should()
                .BeEquivalentTo(expected);
        }

        [Test]
        public void GetTwoTickerPricesByRange_ShouldGive_First_Quoutes_For_0_0_Range()
        {
            var ex = Assert.Throws<Exception>(() =>
            {
                _equityPrices.GetTwoTickerPricesByRange(0, 0);
            });

            Assert.AreEqual("From and To cannot be same", ex.Message);
        }

        [Test]
        public void GetTwoTickerPricesByRange_ShouldGive_Next_Two_Quoutes_For_1_2_Range()
        {
            var expected = new LatestTwoTickerPrices
            {
                LatestQuote = _secondQuote,
                LatestDate = _secondDate,
                PreviousQuote = _thirdQuote,
                PreviousDate = _thirdDate
            };

            _equityPrices.GetTwoTickerPricesByRange(1, 2).Should()
                .BeEquivalentTo(expected);
        }
    }
}
