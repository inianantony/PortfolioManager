using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace PortfolioManager.Models
{
    public class EquityPrices
    {

        [JsonProperty("Time Series (Daily)")]
        public Dictionary<DateTime, Quote> TimeSeriesDaily { get; set; }

        public LatestTwoTickerPrices GetLatestTwoTickerPrices()
        {
            return GetTwoTickerPricesByRange(0, 1);
        }

        public LatestTwoTickerPrices GetTwoTickerPricesByRange(int from, int to)
        {
            var priceList = this;
            var dates = priceList.TimeSeriesDaily.Keys.OrderByDescending(a => a.Date).ToList();
            var latestDate = dates.Skip(from).Take(to).First();
            var previousClosingDate = dates.Skip(from + 1).Take(to + 1).First();

            var first = priceList.TimeSeriesDaily[latestDate];
            var previous = priceList.TimeSeriesDaily[previousClosingDate];

            var latest2Prices = new LatestTwoTickerPrices
            {
                LatestDate = latestDate,
                PreviousDate = previousClosingDate,
                LatestQuote = first,
                PreviousQuote = previous
            };
            return latest2Prices;
        }
    }
}