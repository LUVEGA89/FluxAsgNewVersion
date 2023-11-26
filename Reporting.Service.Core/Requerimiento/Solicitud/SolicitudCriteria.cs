using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Requerimiento.Solicitud
{
    public class SolicitudCriteria : WikiCore.Data.Criteria
    {
        public EstatusKind Estatus { get; set; }

        public string RegistradoPor { get; set; }

        public string ResponsableArea { get; set; }

        public int Area { get; set; }

        public DateTime Del { get; set; }

        public DateTime Al { get; set; }

    }
}
