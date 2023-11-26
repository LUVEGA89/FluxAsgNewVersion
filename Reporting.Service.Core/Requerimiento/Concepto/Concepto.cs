using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Requerimiento.Concepto
{
    public class Concepto : BusinessObject<int>
    {
        public string Descripcion { get; set; }

        public Requerimiento.TipoReq.TipoRequerimiento TipoRequerimiento { get; set; }
    }
}
