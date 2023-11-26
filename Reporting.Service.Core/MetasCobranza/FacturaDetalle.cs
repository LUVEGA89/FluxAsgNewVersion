using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.MetasCobranza
{
    public class FacturaDetalle
    {
        public int Sequence { get; set; }
        public int IdFactura { get; set; }
        public DateTime FechaFactura { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string CarCode { get; set; }
        public string CardName { get; set; }
        public string Canal { get; set; }
        public decimal Saldo { get; set; }
    }
}
