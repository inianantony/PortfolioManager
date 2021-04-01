using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using PortfolioManager.Models;

namespace PortfolioManager.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly string _connectionString;

        public PortfolioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Client> GetAllClients()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                return conn.Query<Client>("Select ClientId,ClientName,Salutation,Currency From Clients").ToList();
            }
        }

        public Client GetClientById(int clientId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                return conn.Query<Client>("Select ClientId,ClientName,Salutation,Currency From Clients WHERE ClientId = @ClientId", new { clientId }).First();
            }
        }

        public EquityTransactions GetEquityTransactions(int clientId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                return new EquityTransactions
                {
                    Transactions = conn
                        .Query<EquityTransaction>(
                            "Select ClientId,Ticker,TradeDate,Action,Quantity,Price From EquityTransactions WHERE ClientId = @ClientId",
                            new { clientId }).ToList()
                };

            }
        }
    }
}