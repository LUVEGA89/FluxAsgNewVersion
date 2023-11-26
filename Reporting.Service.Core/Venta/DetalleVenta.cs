using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Venta
{
    public class DetalleVenta
    {
        public decimal CF { get; set; }
        public decimal Factura { get; set; }
        public decimal Total { get; set; }
        public decimal Promedio { get; set; }
        public decimal MontoFactura { get; set; }
        // monto de la factura origen
        public decimal MontoFacturaOrigen { get; set; }

        public string Canal { get; set; }
    }
}
