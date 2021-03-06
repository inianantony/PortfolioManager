using System;

namespace PortfolioManager.Models
{
    public class LatestTwoTickerPrices
    {
        public DateTime LatestDate { get; set; }
        public DateTime PreviousDate { get; set; }
        public Quote LatestQuote { get; set; }
        public Quote PreviousQuote { get; set; }

        public decimal PreviousPrice => PreviousQuote.Close;

        public decimal LatestPrice => LatestQuote.Close;
    }
}