using System.Collections.Generic;
using System.Linq;
using PortfolioManager.Models.ViewModels;
using PortfolioManager.Repository;

namespace PortfolioManager.Services
{
    public class ClientDashboardService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ProfitLossReportBuilder _profitLossReportBuilder;
        private readonly NetWorthReportBuilder _netWorthReportBuilder;

        public ClientDashboardService(IPortfolioRepository portfolioRepository, ProfitLossReportBuilder profitLossReportBuilder, NetWorthReportBuilder netWorthReportBuilder)
        {
            _portfolioRepository = portfolioRepository;
            _profitLossReportBuilder = profitLossReportBuilder;
            _netWorthReportBuilder = netWorthReportBuilder;
        }

        public PortfolioPageViewModel GetPortfolioPageViewModel(int clientId)
        {
            var equityTransactions = _portfolioRepository.GetEquityTransactions(clientId);

            var client = _portfolioRepository.GetClientById(clientId);
            
            var transactionGroups = equityTransactions.GetTransactionByTicker();

            var plReport = _profitLossReportBuilder.GetProfitLossReport(transactionGroups);

            var last10DaysNetWorth = _netWorthReportBuilder.GetNetWorthHistory(transactionGroups);
            
            var summary = GetSummary(plReport);

            var tickerContribution = GetTickerContribution(plReport);

            var orderedTransaction = equityTransactions.GetOrderedTransaction();
            
            var viewModel = new PortfolioPageViewModel
            {
                Client = client,
                Summary = summary,
                ProfitLosses = plReport,
                EquityTransactions = orderedTransaction,
                Last10DaysNetWorth = last10DaysNetWorth,
                TickerContribution = tickerContribution
            };
            return viewModel;
        }

        private static Dictionary<string, decimal> GetTickerContribution(List<ProfitLossReport> plReport)
        {
            var tickerContribution =
                plReport.GroupBy(a => a.Ticker).ToDictionary(a => a.Key, b => b.Sum(c => c.MarketValue));
            return tickerContribution;
        }

        private static Summary GetSummary(List<ProfitLossReport> plReport)
        {
            var assetNetWorth = plReport.Sum(a => a.MarketValue);
            var summary = new Summary
            {
                NetWorth = assetNetWorth,
                Assets = assetNetWorth,
                Liability = 0
            };
            return summary;
        }

        public List<ProfitLossReport> GetProfitLossReport(int clientId)
        {
            var equityTransactions = _portfolioRepository.GetEquityTransactions(clientId);

            var client = _portfolioRepository.GetClientById(clientId);

            var transactionGroups = equityTransactions.GetTransactionByTicker();

            var plReport = _profitLossReportBuilder.GetProfitLossReport(transactionGroups);

            return plReport;

        }
    }
}