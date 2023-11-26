using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Trafico.Contenedor.Envio
{
    public class Anexo: BusinessObject<int>
    {
        public string path { get; set; }
        public string archivo { get; set; }
        public string ext { get; set; }
        public string subPath { get; set; }
    }
}
