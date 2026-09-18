namespace Yamaha.AccountStatement.Core.Models
{
    public sealed record AccountDealer(
        int DealerId,
        string DealerKey,
        string BillingSuffix,
        string DealerName,
        bool Status);

    public sealed record AccountDealerOutput(
        int IdCliente,
        string ClaveCliente,
        string SufijoFacturacion,
        string NombreCliente,
        short Estatus);
}
