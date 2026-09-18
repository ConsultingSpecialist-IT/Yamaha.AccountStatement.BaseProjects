using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamaha.AccountStatement.Core.Models
{
    public sealed record AccountDealersParameter(
    string? ClaveCliente,
    string? SufijoFacturacion,
    string? NombreCliente);
}
