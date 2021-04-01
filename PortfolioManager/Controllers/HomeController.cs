using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using PortfolioManager.Cache;
using PortfolioManager.Models;
using PortfolioManager.Models.ViewModels;
using PortfolioManager.Repository;
using Serilog;

namespace PortfolioManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public HomeController(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }
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
                    var quantity = kvp.Value.Sum(a => a.NettQuantity);
                    if (quantity <= 0) continue;
                    var l = new ProfitLossReportViewModel
                    {
                        Ticker = kvp.Key,
                        Quantity = quantity,
                        Cost = kvp.Value.Sum(a => a.Cost),
                        AsOfDate = latestDate.ToString("dd/MM/yyyy"),
                        Price = first.Close,
                        MarketValue = first.Close * quantity,
                        PrevPrice = previous.Close,
                        DailyPandL = (first.Close - previous.Close) * quantity,
                        InceptionPandL = first.Close * quantity -
                                         kvp.Value.Sum(a => a.Cost)
                    };
                    ll.Add(l);
                }

                return ll;
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "HomeController");
            }

            return ll;
        }

        private Dictionary<string, decimal> NewMethod1(List<EquityTransaction> equityTransactions)
        {
            try
            {
                var ll = new List<ProfitLossReportViewModel>();
                int count = 0;
                while (count < 10)
                {
                    var groupBy = equityTransactions.GroupBy(a => a.Ticker);
                    var transactionGroups = groupBy.ToDictionary(a => a.Key, b => b.ToList());
                    foreach (var kvp in transactionGroups)
                    {
                        var priceList = EquitiesPriceCache.EquityPrices[kvp.Key];
                        var dates = priceList.TimeSeriesDaily.Keys.OrderByDescending(a => a.Date).ToList();
                        var latestDate = dates.Skip(count).Take(1).First();
                        var first = priceList.TimeSeriesDaily[latestDate];
                        var quantity = kvp.Value.Sum(a => a.NettQuantity);

                        if (quantity <= 0) continue;
                        var l = new ProfitLossReportViewModel
                        {
                            AsOfDate = latestDate.ToString("dd/MM/yyyy"),
                            MarketValue = first.Close * quantity,
                        };
                        ll.Add(l);
                    }
                    count++;
                }


                return ll.GroupBy(a => a.AsOfDate).ToDictionary(a =>
                        a.Key,
                    b => b.Sum(c => c.MarketValue));
            }
            catch (HttpRequestException e)
            {
            }

            return null;
        }

        public ActionResult Portfolio(int id)
        {
            var client = _portfolioRepository.GetClientById(id);
            var transactions = _portfolioRepository.GetEquityTransactions(id);
            var plReport = NewMethod(transactions);
            var last10Days = new SortedDictionary<string, decimal>(NewMethod1(transactions));
            var summary = new Summary
            {
                NetWorth = plReport.Sum(a => a.MarketValue),
                Assets = plReport.Sum(a => a.MarketValue),
                Liability = 0
            };
            var tickerContribution =
                plReport.GroupBy(a => a.Ticker).ToDictionary(a => a.Key, b => b.Sum(c => c.MarketValue));

            var viewModel = new PortfolioPageViewModel
            {
                Client = client,
                Summary = summary,
                ProfitLosses = plReport,
                EquityTransactions = transactions.OrderByDescending(a => a.TradeDate).ToList(),
                Last10DaysNetWorth = last10Days,
                TickerContribution = tickerContribution
            };
            return View(viewModel);
        }
    }
}
