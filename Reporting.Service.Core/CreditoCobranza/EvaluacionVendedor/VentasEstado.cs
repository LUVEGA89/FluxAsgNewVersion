using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.EvaluacionVendedor
{
    public class VentasEstado
    {
        public string Estado { get; set; }
        public decimal Monto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
