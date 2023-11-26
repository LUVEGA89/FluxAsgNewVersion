using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.CreditoCobranza
{
   public class DetallePorCanal
    {
        public string Identificador { get; set; }
        public decimal Total { get; set; }
        public decimal Participacion { get; set; }
        public decimal Financiamiento { get; set; }
        public int Documentos { get; set; }
        public int Tipo { get; set; }
    }
}
