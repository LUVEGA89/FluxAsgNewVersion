using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Auditoria
{
    public class SeguimientoAuditoria
    {
        public int Id_Tienda { get; set; }
        public string Tienda { get; set; }
        public int Sequence { get; set; }
        public string Nombre { get; set; }
        public decimal Cumplimiento { get; set; }
        public int Puntuacion { get; set; }
        public string RegistradoEl { get; set; }
        public int Aplicados { get; set; }
        public string Usuario { get; set; }
    }
}
