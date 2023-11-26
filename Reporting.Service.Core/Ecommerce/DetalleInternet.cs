using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Ecommerce
{
    public class DetalleInternet
    {
        public string Tipo { get; set; }
        public int Folio { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
    }
}
