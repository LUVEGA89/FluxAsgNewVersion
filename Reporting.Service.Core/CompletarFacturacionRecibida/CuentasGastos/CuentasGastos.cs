using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.CompletarFacturacionRecibida.CuentasGastos
{
    public class CuentasGastos : BusinessObject<string>
    {
        public string ActId { get; set; }
        public string AcctName { get; set; }
    }
}
