using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Requerimiento.Solicitud
{
    public class Solicitud : BusinessObject<int>
    {
        public string Comentarios { get; set; }

        public Buzon.Area.Area Area { get; set; }

        public Requerimiento.TipoReq.TipoRequerimiento Requerimiento { get; set; }

        public Requerimiento.Concepto.Concepto Concepto { get; set; }

        public DateTime RegistradoEl { get; set; }

        public DateTime FechaRequerida { get; set; }

        public DateTime? FechaCompromiso { get; set; }

        public string RegistradorPor { get; set; }

        public EstatusKind Estatus { get; set; }

        public string EstatusDescripcion { get; set; }

        public string ModificadoPor { get; set; }

        public DateTime? ModifcadolEl { get; set; }


    }
}
