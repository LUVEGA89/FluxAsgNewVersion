using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Requerimiento.TipoReq
{
    public class TipoRequerimientoCriteria : Criteria
    {
        public int Area { get; set; }

        public EstatusKind? Estatus { get; set; }
    }
}
