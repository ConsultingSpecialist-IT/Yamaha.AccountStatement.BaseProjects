using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Odbc;
namespace Yamaha.DataAccess.Db2.Persistance
{
    public class DapperContext
    {
        private readonly string? _connectionString;
        private readonly string? _databaseSchema;

        public DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("B2BConexionDB2");
            _databaseSchema = configuration.GetConnectionString("B2BConexionDB2");
        }

        public IDbConnection CreateConnection()
            => new OdbcConnection(_connectionString);

        public string? GetDatabaseSchema() => _databaseSchema;
    }
}
