using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.EvaluacionVendedor
{
    public class Vendedor
    {
        public int Sequence { get; set; }
        public string Nombre { get; set; }
        public int Codigo2 { get; set; }
        public decimal MetaDelMes { get; set; }
        public decimal MontoDelMes { get; set; }
        public decimal MontoDelAño { get; set; }
        public decimal MontoDelAñoPasado { get; set; }

    }
}
