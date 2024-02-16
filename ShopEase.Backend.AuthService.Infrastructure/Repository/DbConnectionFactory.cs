using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ShopEase.Backend.AuthService.Infrastructure
{
    public class DbConnectionFactory
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration) 
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("AuthUserDB") ?? string.Empty;
        }

        public NpgsqlConnection GetConnection() => new NpgsqlConnection(_connectionString);
    }
}
