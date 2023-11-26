using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Consejo
{
    public class CanalDetalle
    {
        public string Cliente { get; set; }
        public string Codigo { get; set; }
        public decimal CantidadAñoAct { get; set; }
        public  decimal MontoAñoAct { get; set; }
        public decimal UtilidadAñoAct { get; set; }
        public decimal CantidadAñoAnt { get; set; }
        public decimal MontoAñoAnt { get; set; }
        public decimal UtilidadAñoAnt { get; set; }
        public decimal ActualPorcentPart { get; set; }
        public decimal AnteriorPorcentPart { get; set; }
        public decimal ActualPorcentUtilidad { get; set; }
        public decimal AnteriorPorcentUtilidad { get; set; }
        public decimal PorcentajeCrecimiento { get; set; }
    }
}
