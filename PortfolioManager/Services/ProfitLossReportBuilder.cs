using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using PortfolioManager.Cache;
using PortfolioManager.Models;
using PortfolioManager.Models.ViewModels;
using Serilog;

namespace PortfolioManager.Services
{
    public class ProfitLossReportBuilder
    {
        private readonly IEquitiesPriceCache _cache;

        public ProfitLossReportBuilder(IEquitiesPriceCache cache)
        {
            _cache = cache;
        }

        public List<ProfitLossReport> GetProfitLossReport(Dictionary<string, List<EquityTransaction>> transactionGroups)
        {
            var plReport = new List<ProfitLossReport>();

            try
            {
                foreach (var eachTickerTransaction in transactionGroups)
                {
                    var tickerId = eachTickerTransaction.Key;
                    var transactions = eachTickerTransaction.Value;

                    var tickerPrices = _cache.GetEquityPrice(tickerId);
                    var latest2Prices = tickerPrices.GetLatestTwoTickerPrices();

                    var quantity = eachTickerTransaction.Value.Sum(a => a.NettQuantity);
                    if (quantity <= 0) continue;

                    plReport.Add(new ProfitLossReport(tickerId, transactions, latest2Prices));
                }
            }
            catch (HttpRequestException e)
            {
                Log.Error(e, "ProfitLossReportBuilder");
            }

            return plReport;
        }
    }
}