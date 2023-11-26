using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Requerimiento.TipoReq
{
    public class TipoRequerimiento: BusinessObject<int>
    {
        public string Descripcion { get; set; }  
        
        public Buzon.Area.Area Area { get; set; }
    }
}
