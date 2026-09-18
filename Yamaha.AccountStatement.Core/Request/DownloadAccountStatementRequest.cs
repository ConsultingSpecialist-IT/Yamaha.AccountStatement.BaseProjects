namespace Yamaha.AccountStatement.Core.Request
{
    public sealed record DownloadAccountStatementRequest(
        string DistributorKey,
        string BranchCode,
        string Month,
        int Year
    );
}
