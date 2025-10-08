using Microsoft.Extensions.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RestaurantManagementUI.Data
{
    public class DBConnection
    {
        private readonly IConfiguration _config;
        private readonly string connectionString;

        public DBConnection(IConfiguration config)
        {
            _config = config;
            connectionString = _config.GetConnectionString("dbcs")
                               ?? throw new InvalidOperationException("Connection string 'dbcs' is not configured.");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
