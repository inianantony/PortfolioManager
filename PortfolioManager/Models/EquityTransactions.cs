using System.Collections.Generic;
using System.Linq;

namespace PortfolioManager.Models
{
    public class EquityTransactions 
    {
        public List<EquityTransaction> Transactions { get; set; }

        public Dictionary<string, List<EquityTransaction>> GetTransactionByTicker()
        {
            var groupBy = this.Transactions.GroupBy(a => a.Ticker);
            var transactionGroups = groupBy.ToDictionary(a => a.Key, b => b.ToList());
            return transactionGroups;
        }

        public List<EquityTransaction> GetOrderedTransaction()
        {
            return this.Transactions.OrderByDescending(a => a.TradeDate).ToList();
        }
    }
}