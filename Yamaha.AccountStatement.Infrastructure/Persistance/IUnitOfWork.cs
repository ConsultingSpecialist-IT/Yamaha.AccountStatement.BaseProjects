using Yamaha.AccountStatement.Core.Models;
using Yamaha.DataAccess.Db2.Repositories;

namespace Yamaha.AccountStatement.Infrastructure.Persistance
{
    public interface IUnitOfWork
    {
        GenericRepository<AccountDealerOutput> AccountDealerOutput { get; }
        GenericRepository<ClientBalanceOutput> ClientBalanceOutput { get; }
        GenericRepository<ClientAccountStatementOutput> ClientAccountStatementOutput { get; }
        GenericRepository<PeriodsOutPut> PeriodsOutPut { get; }
        GenericRepository<ParametersOutPut> ParametersOutPut { get; }
    }
}
