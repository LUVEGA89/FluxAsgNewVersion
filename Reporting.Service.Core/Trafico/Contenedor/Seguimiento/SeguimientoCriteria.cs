using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor.Seguimiento
{
    public class SeguimientoCriteria: Criteria
    {
        public DateTime fecIni { get; set; }
        public DateTime fecFin { get; set; }
    }
}
