using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Estados
{
    public class CorteGlobal
    {
        public string Sucursal { get; set; }
        //public DateTime? Fecha { get; set; }
        public string Fecha { get; set; }
        public decimal Total { get; set; }
        public string CardCode { get; set; }
        public decimal Total2 { get; set; }
        public string Abono { get; set; }
        public string Comentario { get; set; }
        public string NombreBanco { get; set; }
        public string TipoPago { get; set; }

    }
}