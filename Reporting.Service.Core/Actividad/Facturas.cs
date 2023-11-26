using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Actividad
{
    public class Facturas
    {
        public int DocKey { get; set; }
        public int DocNum { get; set; }
        public DateTime DocFecha { get; set; }
        public string CodeCliente { get; set; }
        public string CodeQR { get; set; }
    }
}
