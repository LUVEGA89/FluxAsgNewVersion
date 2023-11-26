using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor
{
    public class ContenedorCriteria: Criteria
    {
        public string nomCompleto { get; set; }

        public string nomParcial { get; set; }

        public int statusIni { get; set; }

        public int statusFin { get; set; }

        public int status { get; set; }

        public DateTime fecIni { get; set; }

        public DateTime fecFin { get; set; }

        public int seguimiento_id { get; set; }
    }
}
