using WikiCore;

namespace Reporting.Service.Core.Mobile.Pedidos.Items
{
    public class PedidoItems : BusinessObject<int>
    {
        public string ItemCode { get; set; }
        public int Quantity { get; set; }
    }
}