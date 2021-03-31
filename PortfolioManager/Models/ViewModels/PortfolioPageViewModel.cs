using System.Collections.Generic;

namespace PortfolioManager.Models.ViewModels
{
    public class PortfolioPageViewModel
    {
        public Client Client { get; set; }
        public Summary Summary { get; set; }
        public List<ProfitLossReportViewModel> ProfitLosses { get; set; }
        public List<EquityTransaction> EquityTransactions { get; set; }
    }
}