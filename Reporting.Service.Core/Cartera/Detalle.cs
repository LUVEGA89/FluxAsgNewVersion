using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Cartera
{
    public class Detalle
    {
        public string Canal { get; set; }
        public string Factura { get; set; }
        public string Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Agente { get; set; }
        public decimal Total { get; set; }
        public decimal Saldo { get; set; }
        public string Estatus { get; set; }
        public int DiasVencimiento { get; set; }

    }
}
