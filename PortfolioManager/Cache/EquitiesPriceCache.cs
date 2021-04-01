using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using AlphaVantage.Net.Common;
using AlphaVantage.Net.Core.Client;
using Newtonsoft.Json;
using PortfolioManager.Models;
using Serilog;

namespace PortfolioManager.Cache
{
    public class EquitiesPriceCache : IEquitiesPriceCache
    {
        private static readonly List<string> SupportedEquities = new List<string> { "GOOG", "MSFT" };
        private static readonly string ApiKey = ConfigurationManager.AppSettings["APIKey"];
        public static Dictionary<string, EquityPrices> EquityPrices { get; private set; }

        public EquitiesPriceCache()
        {
            EquityPrices = new Dictionary<string, EquityPrices>();
            using (var client = new AlphaVantageClient(ApiKey))
            {
                foreach (var supportedEquity in SupportedEquities)
                {
                    try
                    {
                        var setting = new Dictionary<string, string> { { "symbol", supportedEquity } };
                        var priceFetchTask  =  client.RequestPureJsonAsync(ApiFunction.TIME_SERIES_DAILY, setting);
                        Task.WaitAll(priceFetchTask);
                        var pricesResult = priceFetchTask.Result;
                        var prices = JsonConvert.DeserializeObject<EquityPrices>(pricesResult);
                        EquityPrices.Add(supportedEquity, prices);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "EquitiesPriceCache");
                    }
                }
            }
        }

        public EquityPrices GetEquityPrice(string ticker)
        {
            return EquityPrices.ContainsKey(ticker) ? EquityPrices[ticker] : new EquityPrices();
        }

    }
}