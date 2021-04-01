using PortfolioManager.Models;

namespace PortfolioManager.Cache
{
    public interface IEquitiesPriceCache
    {
        EquityPrices GetEquityPrice(string ticker);
    }
}