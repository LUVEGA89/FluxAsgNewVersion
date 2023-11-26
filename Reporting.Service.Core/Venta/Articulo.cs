using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Venta
{
    public class Articulo
    {
        public string Canal { get; set; }

        public string SKU { get; set; }

        public int anio { get; set; }

        public List<int> MesPiezas { get; set; }

        public List<decimal> MesDinero { get; set; }

        public List<decimal> MesMargen { get; set; }

        public List<string> columnas { get; set; }

        public int PiezasTotal { get; set; }

        public decimal DineroTotal { get; set; }

    }
}
