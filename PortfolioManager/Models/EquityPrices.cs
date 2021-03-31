using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PortfolioManager.Models
{
    public class EquityPrices
    {

        [JsonProperty("Time Series (Daily)")]
        public Dictionary<DateTime, Quote> TimeSeriesDaily { get; set; }
    }
}