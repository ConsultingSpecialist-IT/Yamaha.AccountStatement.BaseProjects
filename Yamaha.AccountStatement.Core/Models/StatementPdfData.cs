namespace Yamaha.AccountStatement.Core.Models
{
    public sealed record StatementPdfData
    {
        public required DealerBalance Header { get; init; }
        public required List<DealerAccountStatement> Details { get; init; }
        public required decimal BalanceDue { get; init; }
        public required List<BankAccounts> BankAccount { get; init; }
    }
}
