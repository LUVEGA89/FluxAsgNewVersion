using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Pedidos.TiendaWeb
{
    public class RegistroPedido
    {
        public string SKU { get; set; }

        public int Cantidad { get; set; }

        public decimal precioTotal { get; set; }

        public int auth { get; set; }

    }
}
