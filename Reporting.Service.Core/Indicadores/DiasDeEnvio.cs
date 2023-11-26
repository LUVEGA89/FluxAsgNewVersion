using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Indicadores
{
    public class DiasDeEnvio
    {
        public int Folio { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Fecha2 { get; set; }
        public int Dias { get; set; }
        public string Producto { get; set; }
        public string TamañoContenedor { get; set; }
        public EstadoEnvio Estado { get; set; }
        public int NumeroCompras { get; set; }
        public int Aplica { get; set; }
        public int Factura { get; set; }
    }
}
