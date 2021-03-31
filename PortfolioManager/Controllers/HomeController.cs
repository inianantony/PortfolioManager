using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using PortfolioManager.Cache;
using PortfolioManager.Models;
using PortfolioManager.Models.ViewModels;
using PortfolioManager.Repository;

namespace PortfolioManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPortfolioRepository _portfolioRepository = new PortfolioRepository(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public ActionResult Index()
        {
            var clients = _portfolioRepository.GetAllClients();
            return View(clients);
        }

        private List<ProfitLossReportViewModel> NewMethod(List<EquityTransaction> equityTransactions)
        {
            var ll = new List<ProfitLossReportViewModel>();

            try
            {
                var groupBy = equityTransactions.GroupBy(a => a.Ticker);
                var transactionGroups = groupBy.ToDictionary(a => a.Key, b => b.ToList());
                foreach (var kvp in transactionGroups)
                {
                    var priceList = EquitiesPriceCache.EquityPrices[kvp.Key];
                    var dates = priceList.TimeSeriesDaily.Keys.OrderByDescending(a => a.Date).ToList();
                    var latestDate = dates.First();
                    var previousClosingDate = dates.Skip(1).Take(1).First();
                    var first = priceList.TimeSeriesDaily[latestDate];
                    var previous = priceList.TimeSeriesDaily[previousClosingDate];
                    var l = new ProfitLossReportViewModel
                    {
                        Ticker = kvp.Key,
                        Quantity = kvp.Value.Sum(a => a.NettQuantity),
                        Cost = kvp.Value.Sum(a => a.Cost),
                        AsOfDate = latestDate.ToString("dd/MM/yyyy"),
                        Price = first.Close,
                        MarketValue = first.Close * kvp.Value.Sum(a => a.NettQuantity),
                        PrevPrice = previous.Close,
                        DailyPandL = (first.Close - previous.Close) * kvp.Value.Sum(a => a.NettQuantity),
                        InceptionPandL = first.Close * kvp.Value.Sum(a => a.NettQuantity) -
                                         kvp.Value.Sum(a => a.Cost)
                    };
                    ll.Add(l);
                }

                return ll;
            }
            catch (HttpRequestException e)
            {
            }

            return ll;
        }

        public ActionResult Portfolio(int id)
        {
            var client = _portfolioRepository.GetClientById(id);
            var transactions = _portfolioRepository.GetEquityTransactions(id);
            var plReport = NewMethod(transactions);
            var summary = new Summary
            {
                NetWorth = plReport.Sum(a => a.MarketValue),
                Assets = plReport.Sum(a => a.MarketValue),
                Liability = 0
            };

            var viewModel = new PortfolioPageViewModel
            {
                Client = client,
                Summary = summary,
                ProfitLosses = plReport,
                EquityTransactions = transactions.OrderByDescending(a=>a.TradeDate).ToList()
            };
            return View(viewModel);
        }
    }
}
