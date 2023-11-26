using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Ecommerce
{
    public class VentasEcommerce
    {
        public decimal Subtotal { get; set; }
        public decimal Descuentos { get; set; }
        public decimal Total { get; set; }
        public IList<DetalleInternet> Detalle { get; set; }
    }
}
