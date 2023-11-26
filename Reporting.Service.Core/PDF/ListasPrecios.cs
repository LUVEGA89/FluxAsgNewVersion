using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.PDF
{
    public class ListasPrecios
    {
        public int Sequence { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioActualizado { get; set; }
        public decimal PrecioSinIVA { get; set; }
    }
}
