using System;

namespace PortfolioManager.Models
{
    public class EquityTransaction
    {
        public string Ticker { get; set; }
        public DateTime TradeDate { get; set; }
        public string Action { get; set; }
        public int Quantity { get; set; }

        public int NettQuantity => Quantity * (Action == "Buy" ? 1 : -1);

        public decimal Price { get; set; }
        public decimal Cost => Price * NettQuantity;
    }
}