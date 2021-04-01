using System.Collections.Generic;
using System.Linq;

namespace PortfolioManager.Models
{
    public class ProfitLossReport
    {
        private readonly List<EquityTransaction> _transactions;
        private readonly LatestTwoTickerPrices _latest2Prices;

        public ProfitLossReport(string tickerId, List<EquityTransaction> transactions, LatestTwoTickerPrices latest2Prices)
        {
            Ticker = tickerId;
            _transactions = transactions;
            _latest2Prices = latest2Prices;
        }

        public string Ticker { get; }

        public string AsOfDate => _latest2Prices.LatestDate.ToString("dd/MM/yyyy");
        public int Quantity => _transactions.Sum(a => a.NettQuantity);
        public decimal Price => _latest2Prices.LatestPrice;
        public decimal PrevPrice => _latest2Prices.PreviousPrice;
        public decimal Cost => _transactions.Sum(a => a.Cost);
        public decimal MarketValue => _latest2Prices.LatestPrice * Quantity;

        public decimal DailyPandL => (_latest2Prices.LatestPrice - _latest2Prices.PreviousPrice) * Quantity;
        public decimal InceptionPandL => MarketValue - Cost;
    }
}