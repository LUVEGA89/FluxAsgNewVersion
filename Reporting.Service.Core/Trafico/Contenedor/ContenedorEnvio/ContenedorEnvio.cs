using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Trafico.Contenedor.ContenedorEnvio
{
    public class ContenedorEnvio: BusinessObject<int>
    {
        public Contenedor Contenedor { get; set; }

        public Envio.Envio Envio { get; set; }

        public string usuario { get; set; }

        public int estado { get; set; }
    }
}
