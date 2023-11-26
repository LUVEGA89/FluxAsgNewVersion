using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Tarimas
{
    public class Tarima : BusinessObject<int>
    {
        public int DocNum { get; set; }
        public string CardCodeCliente { get; set; }
        public string CardNameCliente { get; set; }
        public DateTime DocDate { get; set; }
        public decimal DocTotal { get; set; }
        public int NoCajas { get; set; }
        public decimal NoPiezas { get; set; }

        public int EstatusSincronizacion { get; set; }
        public string FolioSAP { get; set; }
        

    }
}
