using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Auditoria
{
    public class Captura
    {
        public int Id_TipoAuditoria { get; set; }
        public string TipoAuditoria { get; set; }
        public string DescripcionAuditoria { get; set; }
        public int Id_Segmento { get; set; }
        public string Segmento { get; set; }
        public string SegmentoDescripcion { get; set; }
        public int Id_Rubro { get; set; }
        public string Rubro { get; set; }
        public string RubroDescripcion { get; set; }
        public int Puntuacion { get; set; }
        public int Requerido { get; set; }
        public string Nota { get; set; }
    }
}
