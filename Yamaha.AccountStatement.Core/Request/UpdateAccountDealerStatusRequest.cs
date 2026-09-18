using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamaha.AccountStatement.Core.Request
{
    public sealed record UpdateAccountDealerStatusRequest(
    string DealerKey,
    string BillingSuffix,
    bool Status);

    public sealed record UpdateAccountDealerOutput(
        string ClaveCliente,
        string SufijoFacturacion,
        short Estatus);
}
