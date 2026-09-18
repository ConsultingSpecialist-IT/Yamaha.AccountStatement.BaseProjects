using Dapper;
using System.Data;
using Yamaha.DataAccess.Db2.Contracts;
using Yamaha.DataAccess.Db2.Persistance;

namespace Yamaha.DataAccess.Db2.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DapperContext _dapperContext;
        public GenericRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }
        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> ExecuteAsync(string query, object? parameters = null)
        {
            using var connection = _dapperContext.CreateConnection();
            var result = await connection.ExecuteAsync(query, parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public Task<T> ExecuteSelectAsync(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<T?> ExecuteSelectAsync(string query, object? parameters = null)
        {
            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<T>(query, parameters, commandType: CommandType.StoredProcedure);
            return result;
        }

        public Task<T> GetAsync(string query)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(string query, object? parameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<T>> QueryStoredProcedureAsync(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<T>> QueryStoredProcedureAsync(string query, object? parameters = null)
        {
            using var connection = _dapperContext.CreateConnection();
            var result = await connection.QueryAsync<T>(query, parameters, commandType: CommandType.StoredProcedure);
            return [.. result];
        }
    }
}
