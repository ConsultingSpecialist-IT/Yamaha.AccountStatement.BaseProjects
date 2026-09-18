using Yamaha.AccountStatement.Core.Models;
using Yamaha.AccountStatement.Infrastructure.Contracts;

namespace Yamaha.AccountStatement.Infrastructure.Services
{
    public class StatementPdfGenerator: IStatementPdfGenerator
    {
        public byte[] Generate(StatementPdfData pdfData)
        {
            var pdf = new PdfClientStatement(
                pdfData.Header,
                pdfData.Details,
                pdfData.BalanceDue,
                pdfData.BankAccount);

            return pdf.Process();
        }
    }
}
