using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.PagosRecibidos
{
    public class DetallePago
    {
        public int Pago { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaFactura { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal MontoPago { get; set; }
        public int Factura { get; set; }
        public decimal MontoAplicado { get; set; }
        public int DiasVencidos { get; set; }
        public string Canal { get; set; } 

    }
}
