using System.Net;

namespace Yamaha.AccountStatement.Core.Response
{
    public sealed record BaseDataResponse<T>(
        bool IsSuccess,
        string Message,
        T? Data
        ) where T : class;
}
