namespace PortfolioManager.Models.ViewModels
{
    public class ProfitLossReportViewModel
    {
        public string Ticker { get; set; }
        public string AsOfDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal PrevPrice { get; set; }
        public decimal Cost { get; set; }
        public decimal MarketValue { get; set; }
        public decimal DailyPandL { get; set; }
        public decimal InceptionPandL { get; set; }

    }
}