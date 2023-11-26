using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Reporting.Service.Core.Productos
{
    public class Familia
    {
        public int Sku { get; set; }
        public int Sequence { get; set; }
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int MayoreoDesde { get; set; }
        public int MayoreoDistribuidorDesde { get; set; }
    }
}



