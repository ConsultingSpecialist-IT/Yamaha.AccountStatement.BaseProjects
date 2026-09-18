using Yamaha.AccountStatement.Core.Request;
using Yamaha.AccountStatement.Core.Response;

namespace Yamaha.AccountStatement.Infrastructure.Contracts
{
    public interface IDealerStatementGenerationService
    {
        Task ProcessStatementsAsync(CancellationToken cancellationToken);
    }
}
