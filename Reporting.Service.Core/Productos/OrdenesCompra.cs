using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Productos
{
    public class OrdenesCompra
    {
        public int Folio { get; set; }
        public DateTime Fecha { get; set; }
        public string Proveedor { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
