using Reporting.Service.Core.Mobile.Pedidos.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Mobile.PedidosChofer
{
    public class PedidoChofer : BusinessObject<int>
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Address2 { get; set; }
        public int Cajas { get; set; }
        public string Fletera { get; set; }
        public List<PedidoItems> Items { get; set; }
        public string Ubicacion { get; set; }
    }
}
