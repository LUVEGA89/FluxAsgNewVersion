using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Pedidos.Retail
{
    public class PedidoItem : BusinessObject<int>
    {
        public int Producto { get; set; }

        public string SKU { get; set; }

        public decimal Cantidad { get; set; }

        public decimal Precio { get; set; }

        public decimal Importe { get; set; }

        public decimal Monto { get; set; }

        public ClienteKind TipoPedido { get; set; }

    }
}
