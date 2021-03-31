using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using PortfolioManager.Controllers;
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
                return conn.Query<Client>("Select * From Clients").ToList();
            }
        }

        public Client GetClientById(int clientId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                return conn.Query<Client>("Select * From Clients WHERE ClientId = @ClientId", new { clientId }).First();
            }
        }

        public List<EquityTransaction> GetEquityTransactions(int clientId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                return conn.Query<EquityTransaction>("Select * From EquityTransactions WHERE ClientId = @ClientId", new { clientId }).ToList();
            }
        }
    }
}