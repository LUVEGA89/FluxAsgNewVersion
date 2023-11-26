using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.ApartadoMercancia.Factura
{
    public class Factura: BusinessObject<int>
    {
        public DateTime fecha { get; set; }
        public decimal importe { get; set; }
    }
}
