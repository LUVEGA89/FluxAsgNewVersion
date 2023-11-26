using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Bitacora
{
    public class Bitacora
    {
        public int Sequence { get; set; }
        public int IdSucursal { get; set; }
        public string Sucursal { get; set; }
        public int Estado { get; set; }
        public string Observaciones { get; set; }
        public DateTime RegistradoEl { get; set; }
        public string RegistradoPor { get; set; }
    }
}
