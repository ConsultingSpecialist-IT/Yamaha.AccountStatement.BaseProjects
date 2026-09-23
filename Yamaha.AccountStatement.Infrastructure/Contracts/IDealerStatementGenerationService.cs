using Yamaha.AccountStatement.Core.Models;
using Yamaha.AccountStatement.Core.Request;
using Yamaha.AccountStatement.Core.Response;

namespace Yamaha.AccountStatement.Infrastructure.Contracts
{
    public interface IDealerStatementGenerationService
    {
        Task ProcessStatementsAsync(CancellationToken cancellationToken);
        Task<string> ProcessStatementAsync(string dealerKey, string billingSuffix, string statementPeriod, CancellationToken cancellationToken);
    }
}
