using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.ApartadoMercancia.Cliente
{
    public class Cliente: BusinessObject<string>
    {
        public string nombre { get; set; }
        public string listaPrecios { get; set; }
        public string canal { get; set; }
        //public int factura { get; set; }
        //public DateTime fechaFactura { get; set; }
        //public decimal importe { get; set; }
    }
}
