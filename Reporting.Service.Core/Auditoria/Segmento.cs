using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Auditoria
{
    public class Segmento
    {
        public int Sequence { get; set; }
        public int Tipo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public int Activo { get; set; }
        public IList<Rubros> Rubros { get; set; }
    }
}
