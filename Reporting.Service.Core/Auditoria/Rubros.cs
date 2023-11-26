
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Auditoria
{
    public class Rubros
    {
        public int Segmento { get; set; }
        public int Sequence { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Puntuacion { get; set; }
        public int Requerido { get; set; }
        public string Nota { get; set; }
    }
}
