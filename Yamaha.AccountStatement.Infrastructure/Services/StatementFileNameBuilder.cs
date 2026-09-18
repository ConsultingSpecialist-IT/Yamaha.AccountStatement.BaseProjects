using Yamaha.AccountStatement.Infrastructure.Contracts;

namespace Yamaha.AccountStatement.Infrastructure.Services
{
    public class StatementFileNameBuilder: IStatementFileNameBuilder
    {
        public string GetStatementPeriod()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)
                .AddMonths(-1)
                .ToString("yyyy-MM");
        }

        public string BuildFileName(string dealerKey, string dealerLocationId, string statementPeriod)
        {
            return $"{dealerKey}-{dealerLocationId}-{statementPeriod}.pdf";
        }
    }
}
