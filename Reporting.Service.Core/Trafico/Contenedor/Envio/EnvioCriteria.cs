using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor.Envio
{
    public class EnvioCriteria: Criteria
    {
        public DateTime Inicio { get; set; }

        public DateTime Fin { get; set; }
    }
}
