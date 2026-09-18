namespace Yamaha.AccountStatement.Infrastructure.Contracts
{
    public interface IStatementFileNameBuilder
    {
        string GetStatementPeriod();
        string BuildFileName(string dealerKey, string dealerLocationId, string statementPeriod);
    }
}
