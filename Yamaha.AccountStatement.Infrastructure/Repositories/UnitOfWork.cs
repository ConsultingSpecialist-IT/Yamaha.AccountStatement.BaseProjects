using Yamaha.AccountStatement.Core.Models;
using Yamaha.AccountStatement.Infrastructure.Persistance;
using Yamaha.DataAccess.Db2.Persistance;
using Yamaha.DataAccess.Db2.Repositories;

namespace Yamaha.AccountStatement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DapperContext _dapperContext;
        public UnitOfWork(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
            AccountDealerOutput = new GenericRepository<AccountDealerOutput>(_dapperContext);
            ClientBalanceOutput = new GenericRepository<ClientBalanceOutput>(_dapperContext);
            ClientAccountStatementOutput = new GenericRepository<ClientAccountStatementOutput>(_dapperContext);
            PeriodsOutPut = new GenericRepository<PeriodsOutPut>(_dapperContext);
            ParametersOutPut = new GenericRepository<ParametersOutPut>(_dapperContext);
        }
        public GenericRepository<AccountDealerOutput> AccountDealerOutput { get; private set; }
        public GenericRepository<ClientBalanceOutput> ClientBalanceOutput { get; private set; }
        public GenericRepository<ClientAccountStatementOutput> ClientAccountStatementOutput { get; private set; }
        public GenericRepository<PeriodsOutPut> PeriodsOutPut { get; private set; }
        public GenericRepository<ParametersOutPut> ParametersOutPut { get; private set; }
    }
}
