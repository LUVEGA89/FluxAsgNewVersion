using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Indicadores
{
   public class ReduccionCostosDetalle
    {
        public string Sku { get; set; }
        public decimal PrecioActual { get; set; }
        public decimal UltimoPrecio { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
