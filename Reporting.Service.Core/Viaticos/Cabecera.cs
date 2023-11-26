using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Viaticos
{
    public class Cabecera
    {
        public int Folio { get; set; }
        public int Item { get; set; }
        public decimal Presupuesto { get; set; }
        public decimal MontoActual { get; set; }
        public decimal Disponible { get; set; }
    }
}
