using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamaha.AccountStatement.Core.Models
{
    public sealed record DealerAccountStatement(
        DateTime? DocumentDate,
        DateTime? PaymentDate,
        DateTime? ExpirationDate,
        int DocumentId,
        decimal TransactionAmount,
        decimal PaymentAmount,
        decimal DocumentNumber,
        string DocumentType,
        decimal Balance);

    public sealed record ClientAccountStatementOutput(
        int IdDocumento,
        string TipoDocumento,
        decimal NumeroDocumento,
        string FechaDocumento,
        string FechaVencimiento,
        decimal ImporteDocumento,
        decimal ImportePago,
        string FechaPago,
        string SumaSaldoVencido);

    public sealed class DealerAccountStatementResult
    {
        public decimal BalanceDue { get; init; }
        public List<DealerAccountStatement> Statements { get; init; } = [];
    }

}
