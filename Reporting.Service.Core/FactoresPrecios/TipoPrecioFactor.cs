using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.FactoresPrecios
{
    public class TipoPrecioFactor:BusinessObject<int>
    {
        public TipoPrecioArticulo TipoPrecioArticulo { get; set; }

        public TipoPrecioCanal TipoPrecioCanal { get; set; }

        public decimal Factor { get; set; }
    }
}
