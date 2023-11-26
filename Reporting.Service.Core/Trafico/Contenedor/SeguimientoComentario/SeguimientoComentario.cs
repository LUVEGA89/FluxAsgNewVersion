using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Trafico.Contenedor.SeguimientoComentario
{
    public class SeguimientoComentario: BusinessObject<int>
    {
        public int seguimiento_id { get; set; }
        public string coment { get; set; }
        public DateTime fecRegistro { get; set; }
        public string usuario { get; set; }
        public int parentID { get; set; }
    }
}
