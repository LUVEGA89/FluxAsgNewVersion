using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.NotaCredito.ConceptoDescuento
{
    public class ConceptoDescuento : BusinessObject<int>
    {
        public string Nombre { get; set; }
        public decimal Descuento { get; set; }
        public string Cuenta { get; set; }
        public string Cliente { get; set; }
    }
}
