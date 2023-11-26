using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Trafico.Contenedor.ContenedorAnexo
{
    public class ContenedorAnexo: BusinessObject<int>
    {
        public int contenedor_id { get; set; }
        public string path { get; set; }
        public string archivo { get; set; }
        public string ext { get; set; }
        public DateTime dateInsert { get; set; }
        public string subPath { get; set; }
        public string usuario { get; set; }
    }
}
