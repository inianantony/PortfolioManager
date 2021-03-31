using System.Collections.Generic;
using PortfolioManager.Controllers;
using PortfolioManager.Models;

namespace PortfolioManager.Repository
{
    public interface IPortfolioRepository
    {
        List<Client> GetAllClients();
        Client GetClientById(int clientId);
        List<EquityTransaction> GetEquityTransactions(int clientId);
    }
}