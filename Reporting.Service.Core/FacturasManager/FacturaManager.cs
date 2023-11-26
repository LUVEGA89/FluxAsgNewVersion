using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.FacturasManager
{
    public class FacturaManager
    {
        public int DocKey { get; set; }
        public int DocNum { get; set; }
        public DateTime DocFecha { get; set; }
        public string CodeCliente { get; set; }
        public string CodeQR { get; set; }
        public string NameCliente { get; set; }
        public string U_UUID { get; set; }
        public DateTime FechaTimbrado { get; set; }
    }
}
