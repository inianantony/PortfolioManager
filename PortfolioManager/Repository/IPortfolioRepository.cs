using System.Collections.Generic;
using PortfolioManager.Models;

namespace PortfolioManager.Repository
{
    public interface IPortfolioRepository
    {
        List<Client> GetAllClients();
        Client GetClientById(int clientId);
        EquityTransactions GetEquityTransactions(int clientId);
    }
}