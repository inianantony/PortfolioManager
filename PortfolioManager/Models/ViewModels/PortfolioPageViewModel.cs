using System.Collections.Generic;

namespace PortfolioManager.Models.ViewModels
{
    public class PortfolioPageViewModel
    {
        public Client Client { get; set; }
        public Summary Summary { get; set; }
        public List<ProfitLossReport> ProfitLosses { get; set; }
        public List<EquityTransaction> EquityTransactions { get; set; }
        public SortedDictionary<string, decimal> Last10DaysNetWorth { get; set; }
        public Dictionary<string, decimal> TickerContribution { get; set; }
    }
}