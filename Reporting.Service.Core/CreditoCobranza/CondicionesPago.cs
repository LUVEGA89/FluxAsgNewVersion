using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.CreditoCobranza
{
    public class CondicionesPago
    {
        public string Identificador { get; set; }
        public int Documentos { get; set; }
        public decimal SaldoTotal { get; set; }
        public decimal Participacion { get; set; }


    }
}
