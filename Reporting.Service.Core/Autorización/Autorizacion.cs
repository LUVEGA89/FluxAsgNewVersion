using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Autorización
{
    public class Autorizacion : BusinessObject<int>
    {
        public string Cliente { get; set; }

        public string tienda { get; set; }

        public decimal Importe { get; set; }

        public string fecha { get; set; }

        public string referencia { get; set; }

        public string comentarios { get; set; }

        public int status { get; set; }

        public int PedidosTiendasWEB { get; set; }
    }
}
