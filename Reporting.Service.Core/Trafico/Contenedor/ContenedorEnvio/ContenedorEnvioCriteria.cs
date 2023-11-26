using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor.ContenedorEnvio
{
    public class ContenedorEnvioCriteria: Criteria
    {
        public int idContenedor { get; set; }
        public int estadoCriteria { get; set; }
        public int DocNum { get; set; }
    }
}
