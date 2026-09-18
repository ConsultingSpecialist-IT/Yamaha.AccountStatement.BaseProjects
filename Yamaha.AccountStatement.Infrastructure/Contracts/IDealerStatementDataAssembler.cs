using Yamaha.AccountStatement.Core.Models;

namespace Yamaha.AccountStatement.Infrastructure.Contracts
{
    public interface IDealerStatementDataAssembler
    {
            Task<StatementPdfData> BuildAsync(
                AccountDealer dealer,
                string statementPeriod,
                CancellationToken cancellationToken);
    }
}
