namespace Yamaha.AccountStatement.Core.Response
{
    public sealed record BasePagedDataResponse<T>(
        int Page,
        int PageSize,
        int TotalCount,
        IEnumerable<T> Data
        ) where T : class;
}
