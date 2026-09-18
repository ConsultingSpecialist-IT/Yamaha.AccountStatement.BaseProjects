using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamaha.DataAccess.Db2.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(string query);
        Task<T> GetAsync(string query, object? parameters = null);
        Task<IReadOnlyList<T>> QueryStoredProcedureAsync(string query);
        Task<IReadOnlyList<T>> QueryStoredProcedureAsync(string query, object? parameters = null);
        Task<T> ExecuteSelectAsync(string query);
        Task<T?> ExecuteSelectAsync(string query, object? parameters = null);
        Task<int> ExecuteAsync(string query, object? parameters = null);
        Task<int> DeleteAsync(int id);
    }
}
