using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Reportes.Mayoreo
{
    public class MayoreoCriteria : Criteria
    {
        public DateTime Del { get; set; }
        public DateTime Al { get; set; }
        public string Familia { get; set; }
        public int Mes { get; set; }

        public string Categoria { get; set; }
        public string Cliente { get; set; }
        public ReporteKind Tipo { get; set; }
        public string Categoria1 { get; set; }
        public string Clasificado { get; set; }

        public string TipoEjecutivo { get; set; }
        public int IdEjecutivo { get; set; }

        public string StateS { get; set; }

    }

}
