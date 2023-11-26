using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Productos
{
    public class PreciosSap
    {
        public int Sequence { get; set; }
        public string ItemCode { get; set; }
        public Int16 PriceList { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public string Ovrwritten { get; set; }
       
    }
}
