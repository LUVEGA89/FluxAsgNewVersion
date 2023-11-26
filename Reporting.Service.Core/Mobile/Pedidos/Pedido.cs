using Reporting.Service.Core.Mobile.Pedidos.Items;
using System.Collections.Generic;
using WikiCore;

namespace Reporting.Service.Core.Mobile.Pedidos
{
    public class Pedido : BusinessObject<int>
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Address2 { get; set; }
        public int Cajas { get; set; }
        public string Fletera { get; set; }
        public List<PedidoItems> Items { get; set; }
    }
}