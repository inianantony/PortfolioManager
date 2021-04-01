using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using PortfolioManager.Models;

namespace PortfolioManagerTests.Models
{
    [TestFixture]
    class EquityTransactionsTests
    {
        private EquityTransactions _equityTransactions;

        [SetUp]
        public void Init()
        {
            _equityTransactions = new EquityTransactions
            {
                Transactions = new List<EquityTransaction>
                {
                    new EquityTransaction
                        {Action = "Buy", Price = 3, Quantity = 10, Ticker = "GOOG",TradeDate = new DateTime(2021,03,30)},
                    new EquityTransaction
                        {Action = "Buy", Price = 3, Quantity = 10, Ticker = "IBM",TradeDate = new DateTime(2021,03,29)},
                    new EquityTransaction
                        {Action = "Buy", Price = 4, Quantity = 10, Ticker = "GOOG",TradeDate = new DateTime(2021,03,31)},
                }
            };
        }

        [Test]
        public void GetTransactionByTicker_Should_Group_Transactions_By_Ticker()
        {
            var expected = new Dictionary<string, List<EquityTransaction>>
            {
                {"IBM",new List<EquityTransaction>{new EquityTransaction
                    {Action = "Buy", Price = 3, Quantity = 10, Ticker = "IBM",TradeDate = new DateTime(2021,03,29)}
                }},
                {"GOOG",new List<EquityTransaction>{new EquityTransaction
                    {Action = "Buy", Price = 3, Quantity = 10, Ticker = "GOOG",TradeDate = new DateTime(2021,03,30)},
                    new EquityTransaction
                        {Action = "Buy", Price = 4, Quantity = 10, Ticker = "GOOG",TradeDate = new DateTime(2021,03,31)},
                }}
            };

            _equityTransactions.GetTransactionByTicker().Should().NotBeEmpty()
                .And.HaveCount(2)
                .And.BeEquivalentTo(expected);
        }

        [Test]
        public void GetOrderedTransaction_Should_Order_Transactions_By_TradeDate()
        {
            var expected = new List<EquityTransaction>
            {
                new EquityTransaction
                    {Action = "Buy", Price = 3, Quantity = 10, Ticker = "IBM", TradeDate = new DateTime(2021, 03, 29)},
                new EquityTransaction
                    {Action = "Buy", Price = 3, Quantity = 10, Ticker = "GOOG", TradeDate = new DateTime(2021, 03, 30)},
                new EquityTransaction
                    {Action = "Buy", Price = 4, Quantity = 10, Ticker = "GOOG", TradeDate = new DateTime(2021, 03, 31)},
            };

            _equityTransactions.GetOrderedTransaction().Should().NotBeEmpty()
                    .And.HaveCount(3)
                    .And.BeEquivalentTo(expected);
        }
    }
}
