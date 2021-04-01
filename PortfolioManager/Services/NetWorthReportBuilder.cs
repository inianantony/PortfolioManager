using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using PortfolioManager.Cache;
using PortfolioManager.Models;
using PortfolioManager.Models.ViewModels;
using Serilog;

namespace PortfolioManager.Services
{
    public class NetWorthReportBuilder
    {
        private readonly IEquitiesPriceCache _cache;
        private int _NetWorthRange = 10;

        public NetWorthReportBuilder(IEquitiesPriceCache cache)
        {
            _cache = cache;
        }

        public SortedDictionary<string, decimal> GetNetWorthHistory(Dictionary<string, List<EquityTransaction>> transactionGroups)
        {
            try
            {
                var ll = new List<ProfitLossReport>();
                int count = 0;
                while (count < _NetWorthRange)
                {
                    foreach (var eachTickerTransaction in transactionGroups)
                    {
                        var tickerId = eachTickerTransaction.Key;


                        var tickerPrices = _cache.GetEquityPrice(tickerId);
                        var latest2Prices = tickerPrices.GetTwoTickerPricesByRange(count, count + 1);

                        var transactions = eachTickerTransaction.Value.Where(a => a.TradeDate <= latest2Prices.LatestDate).ToList();

                        var quantity = eachTickerTransaction.Value.Sum(a => a.NettQuantity);
                        if (quantity <= 0) continue;

                        ll.Add(new ProfitLossReport(tickerId, transactions, latest2Prices));
                    }

                    count++;
                }

                var groupNetWorthByDate = ll.GroupBy(a => a.AsOfDate).ToDictionary(a =>
                        a.Key,
                    b => b.Sum(c => c.MarketValue));
                var tenDaysNetWorth = new SortedDictionary<string, decimal>(groupNetWorthByDate);
                return tenDaysNetWorth;
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "NetWorthReportBuilder");
            }


            return new SortedDictionary<string, decimal>();
        }
    }
}