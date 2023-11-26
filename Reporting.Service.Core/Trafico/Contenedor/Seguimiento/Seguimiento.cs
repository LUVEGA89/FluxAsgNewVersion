using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Trafico.Contenedor.Seguimiento
{
    public class Seguimiento: BusinessObject<int>
    {
        public DateTime embarcada { get; set; }
        public DateTime llegadaPuerto { get; set; }
        public DateTime salidaPuerto { get; set; }
        public DateTime llegadaPantaco { get; set; }
        public DateTime salidaPantaco { get; set; }
        public DateTime libTransito { get; set; }
        public DateTime libDespacho { get; set; }
        public string usuario { get; set; }

    }
}
