using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.PDF
{
    public class Productos
    {
        public int Sequence { get; set; }
        public string Nombre { get; set; }
        public string Familia { get; set; }
        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public string Clasificacion { get; set; }
        public string Sku { get; set; }
        public int Stock { get; set; }
        public IList<ListasPrecios> ListaPrecio { get; set; }
    }
}
