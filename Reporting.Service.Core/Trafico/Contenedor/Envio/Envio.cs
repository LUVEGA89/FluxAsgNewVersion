
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Trafico.Contenedor.Envio
{
    public class Envio : BusinessObject<int>
    {
        public string Proveedor { get; set; }

        public decimal Importe { get; set; }
    }
}
