namespace Yamaha.AccountStatement.Core.Models
{
    public sealed record DealerBalance(
    string DealerKey,
    string BillingSuffix,
    string DealerName,
    decimal CreditLimit,
    decimal PendingBilling,
    decimal Balance,
    decimal BalanceLetter,
    decimal AvailableBalance);

    public sealed record ClientBalanceOutput(
        string ClaveCliente,
        string SufijoFacturacion,
        string NombreCliente,
        decimal LimiteCredito,
        decimal PendienteFacturar,
        decimal SaldoBalance,
        decimal BalanceLetra,
        decimal SaldoDisponible);
}
