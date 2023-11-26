using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Retail
{
    public class Detalles
    {
        public string Cliente { get; set; }
        public string Nombre { get; set; }
        public decimal TotalFacturaAA { get; set; }
        public decimal TotalFactura { get; set; }
        public decimal TotalNC { get; set; }
        public decimal VentaPeriodo { get; set; }
        public decimal Utilidad { get; set; }
    }
}
