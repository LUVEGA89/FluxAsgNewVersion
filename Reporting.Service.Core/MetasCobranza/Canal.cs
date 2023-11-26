using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.MetasCobranza
{
    public class Canal
    {
        public int Sequence { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Estatus { get; set; }
        public decimal Total { get;  set; }
        public decimal MontoMeta { get; set; }
        public decimal MontoVencido { get; set; }
        public decimal MontoCobrado { get; set; }
    }
}
