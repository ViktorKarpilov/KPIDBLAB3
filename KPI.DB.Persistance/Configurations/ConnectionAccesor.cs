using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using System.Data.SqlClient;

namespace KPI.DB.Persistance.Configurations
{
    public class ConnectionAccesor : IConnectionAccessor 
    {
        private string ConnectionString { get; }
        private IDbConnection InternalConnection { get; set; }
        public IDbConnection Connection
        {
            get
            {
                if (InternalConnection is not null)
                    return InternalConnection;
                InternalConnection = new NpgsqlConnection(ConnectionString);
                return InternalConnection;
            }
        }

        public ConnectionAccesor(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("ApplicationDbContext");
        }
    }
}
