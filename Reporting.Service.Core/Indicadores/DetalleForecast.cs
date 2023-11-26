using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Indicadores
{
    public class DetalleForecast
    {
        public int Sequence { get; set; }
        public int Folio { get; set; }
        public string Sku { get; set; }
        public int Tipo { get; set; }
        public string Mes { get; set; }
        public decimal SIAT { get; set; }
        public decimal SAP { get; set; }
        public decimal Total { get; set; }
        public decimal MediaReal { get; set; }
        public decimal MediaMinima { get; set; }
        public decimal RellenoMedia { get; set; }
        public decimal Pronostico { get; set; }
    }
}
