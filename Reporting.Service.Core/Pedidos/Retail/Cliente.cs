using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Pedidos.Retail
{
    public class Cliente : BusinessObject<int>
    {
        public string Codigo { get; set; }

        public string Nombre { get; set; }

        public string ClienteName
        {
            get
            {
                return Codigo + " - " + Nombre;
            }
        }

        public ClienteKind TipoCliente { get; set; }

    }
}
