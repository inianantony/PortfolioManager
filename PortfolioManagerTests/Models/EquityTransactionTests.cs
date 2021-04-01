using NUnit.Framework;
using PortfolioManager.Models;

namespace PortfolioManagerTests.Models
{
    [TestFixture]
    class EquityTransactionTests
    {
        private EquityTransaction _equityTransaction;

        [SetUp]
        public void Init()
        {
            _equityTransaction = new EquityTransaction { Quantity = 10, Action = "Buy",Price = 3};

        }

        [Test]
        public void NettQuantity_Should_Be_Postive_If_Action_Is_Buy()
        {
            Assert.IsTrue(_equityTransaction.NettQuantity > 0);
        }

        [Test]
        public void NettQuantity_Should_Be_Negative_If_Action_Is_Sell()
        {
            _equityTransaction = new EquityTransaction { Quantity = 10, Action = "Sell" };
            Assert.IsTrue(_equityTransaction.NettQuantity < 0);
        }

        [Test]
        public void Cost_Should_Be_Price_Times_Quantity()
        {
            Assert.AreEqual(30, _equityTransaction.Cost);
        }
    }
}
