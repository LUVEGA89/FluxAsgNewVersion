using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.CreditoCobranza
{
    public class DetalleCobranza
    {
        public int folio { get; set; }
        public string Identificador { get; set; }
        public decimal Saldo { get; set; }
        public DateTime Fecha { get; set; }
    }
}
