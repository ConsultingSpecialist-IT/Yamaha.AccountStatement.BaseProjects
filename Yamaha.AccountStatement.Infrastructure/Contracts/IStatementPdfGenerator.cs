using Yamaha.AccountStatement.Core.Models;

namespace Yamaha.AccountStatement.Infrastructure.Contracts
{
    public interface IStatementPdfGenerator
    {
        byte[] Generate(StatementPdfData pdfData);
    }
}
