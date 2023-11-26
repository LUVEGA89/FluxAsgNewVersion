using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Bitacora
{
    public class BitacoraRequerimiento
    {
        public int Sequence { get; set; }
        public string Requerimiento { get; set; }
        public string Descripcion { get; set; }
        public string Ejemplo { get; set; }
        public int Orden { get; set; }
        public string Nombre { get; set; }
        public int IdDepartamento { get; set; }
        public string Icono { get; set; }
        public string Color { get; set; }
    }
}
