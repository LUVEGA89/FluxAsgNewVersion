using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Seguimiento
{
    public class Comentario
    {
        public int Folio { get; set; }
        public int Sequence { get; set; }
        public string Detalle { get; set; }
        public DateTime RegistradoEl { get; set; }
    }
}
