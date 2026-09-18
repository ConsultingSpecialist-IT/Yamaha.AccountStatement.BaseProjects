using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamaha.AccountStatement.Core.Response
{
    public sealed record BaseResponse(
        bool IsSuccess,
        string Message    
    );
}
