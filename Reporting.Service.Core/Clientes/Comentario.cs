using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Clientes
{
    public class Comentario
    {
        public int Sequence { get; set; }
        public string Cliente { get; set; }
        public string Detalle { get; set; }
        public string RegistradoPor { get; set; }
        public DateTime RegistradoEl { get; set; }
        public string Codigo { get; set; }
        public int SequencePadre { get; set; }
    }
}
