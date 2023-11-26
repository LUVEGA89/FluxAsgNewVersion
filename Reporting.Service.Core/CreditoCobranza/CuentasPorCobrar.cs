using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.CreditoCobranza
{
    public class CuentasPorCobrar
    {
        public string Canal { get; set; }
        public decimal Monto { get; set; }
        public decimal DiasCartera { get; set; }
    }
}
