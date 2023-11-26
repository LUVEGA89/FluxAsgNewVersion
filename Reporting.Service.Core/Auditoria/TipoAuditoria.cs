using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Auditoria
{
    public class TipoAuditoria
    {
        public int Sequence { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime RegistradoEl { get; set; }
        public IList<Segmento> Segmentos { get; set; }

    }
}
