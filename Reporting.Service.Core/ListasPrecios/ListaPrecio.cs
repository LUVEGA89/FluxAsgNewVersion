using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.ListasPrecios
{
    public class ListaPrecio: BusinessObject<int>
    {
        public int Nombre { get; set; }
        public decimal Precio { get; set; }
        public int ListaModificada { get; set; }
    }
}
