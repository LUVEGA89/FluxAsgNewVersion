using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Bitacora
{
    public class BitacoraDetalle
    {
        public int Sequence { get; set; }
        public string Nombre { get; set; }
        public int FolioSolucion { get; set; }
        public string Solucion { get; set; }
        public int Requerimiento { get; set; }
        public int Tipo { get; set; }
        public string Departamento { get; set; }
        public string Email { get; set; }
        public string RegistradoEl { get; set; }
    }
}
