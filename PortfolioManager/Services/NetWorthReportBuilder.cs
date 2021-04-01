using System;
using System.Collections.Generic;
using System.Linq;
using PortfolioManager.Cache;
using PortfolioManager.Models;
using Serilog;

namespace PortfolioManager.Services
{
    public class NetWorthReportBuilder
    {
        private readonly IEquitiesPriceCache _cache;
        private readonly int _netWorthRange = 10;

        public NetWorthReportBuilder(IEquitiesPriceCache cache)
        {
            _cache = cache;
        }

        public SortedDictionary<string, decimal> GetNetWorthHistory(Dictionary<string, List<EquityTransaction>> transactionGroups)
        {
            try
            {
                var reportList = new List<ProfitLossReport>();
                var count = 0;
                while (count < _netWorthRange)
                {
                    foreach (var eachTickerTransaction in transactionGroups)
                    {
                        var tickerId = eachTickerTransaction.Key;
                        var equityTransactions = eachTickerTransaction.Value;

                        var tickerPrices = _cache.GetEquityPrice(tickerId);
                        var latest2Prices = tickerPrices.GetTopTickerPricesByRange(count, count + 1);

                        var transactions = equityTransactions.Where(a => a.TradeDate <= latest2Prices.LatestDate).ToList();

                        var quantity = equityTransactions.Sum(a => a.NettQuantity);
                        if (quantity <= 0) continue;

                        reportList.Add(new ProfitLossReport(tickerId, transactions, latest2Prices));
                    }

                    count++;
                }

                var groupNetWorthByDate = reportList.GroupBy(a => a.AsOfDate).ToDictionary(a =>
                        a.Key,
                    b => b.Sum(c => c.MarketValue));
                var tenDaysNetWorth = new SortedDictionary<string, decimal>(groupNetWorthByDate);
                return tenDaysNetWorth;
            }
            catch (Exception e)
            {
                Log.Error(e, "NetWorthReportBuilder");
            }

            return new SortedDictionary<string, decimal>();
        }
    }
}