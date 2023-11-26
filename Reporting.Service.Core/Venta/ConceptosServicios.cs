using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Venta
{
    public class ConceptosServicios
    {
        public int Sequence { get; set; }
        public string Nombre { get; set; }
        public decimal Descuento { get; set; }
        public string Cuenta { get; set; }
        public string Cliente { get; set; }
    }
}
