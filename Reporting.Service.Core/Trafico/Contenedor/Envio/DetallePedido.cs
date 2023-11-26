using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Trafico.Contenedor.Envio
{
    public class DetallePedido: BusinessObject<string>
    {
        public string familia { get; set; }
        public string subfamilias { get; set; }
        public string empaque { get; set; }
        public int master { get; set; }
        public int inner { get; set; }
        public string accesorios { get; set; }
    }
}
