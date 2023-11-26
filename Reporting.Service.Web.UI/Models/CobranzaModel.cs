using Reporting.Service.Core.CreditoCobranza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class CobranzaModel
    {
        public List<CondicionesPago> DetalleCondicionesPago { get; set; }
        public List<DetallePorCanal> Contado { get; set; }
        public List<DetallePorCanal> Credito { get; set; }
        public List<CuentasPorCobrar> PorCobrar { get; set; }
        public List<DetallePorCanal> Historial { get; set; }
        public List<DetallePorCanal> TopClientes { get; set; }
    }
}