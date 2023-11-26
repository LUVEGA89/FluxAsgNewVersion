using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.EvaluacionVendedor
{
    public class Cliente
    {
        public string Nombre { get; set;}
        public decimal VentaAñoAnterior { get; set; }
        public decimal VentaAñoActual { get; set; }
        public string MesMayorVenta { get; set; }
        public string CodigoDelCliente { get; set; }
        public string TipoDelCliente { get; set; }
        public string VentaCF { get; set; }
        public string VentaCFA { get; set; }
        public string VentaF { get; set; }
        public string VentaFA { get; set; }
        public string TotalClienteConVenta { get; set; }
    }
}

