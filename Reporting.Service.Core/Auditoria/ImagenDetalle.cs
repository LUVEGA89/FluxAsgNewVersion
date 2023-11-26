using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Auditoria
{
    public class ImagenDetalle
    {
        public int Sequence { get; set; }
        public int Rubro { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get;set; }
        public string Imagen64 { get; set; }
    }
}
