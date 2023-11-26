using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Pedidos
{
    public class Pedido : BusinessObject<int>
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string tienda { get; set; }

        public DateTime fecha { get; set; }

        public decimal totalPedido { get; set; }

        public string folioSAP { get; set; }

        public string usuario { get; set; }

        //Seguimiento Pedidos
        public int folioEntrega { get; set; }
        public DateTime fechaSurtido { get; set; }
        public int folioFactura { get; set; }
        public DateTime fechaFactura { get; set; }
        public string numGuia { get; set; }
        public string ruta { get; set; }
        public int idruta { get; set; }
        public DateTime fechaSalida { get; set; }
        public string Status { get; set; }
        public int Minutos { get; set; }
    }
}
