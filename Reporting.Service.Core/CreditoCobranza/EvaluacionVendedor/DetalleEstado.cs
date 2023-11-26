using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.CreditoCobranza.EvaluacionVendedor
{
    public class DetalleEstado: BusinessObject<string>
    {
        public string tienda { get; set; }
        public string Agente { get; set; }
        public decimal monto { get; set; }
        public decimal montoAnt { get; set; }
    }
}
