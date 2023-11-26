using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Productos
{
    public class Productos
    {
        public decimal Cantidad { get; set; }
        public decimal Monto { get; set; }
        public decimal MontoAnterior { get; set; }
        public string Sku { get; set;}
        public decimal PeriodoActual { get; set; }
        public decimal PeriodoAnterior { get; set; }
        public decimal Crecimiento { get; set; }
        public string Cliente { get; set; }
        public string Estado { get; set; }
        public string Tipo { get; set; }
        public decimal Participacion { get; set; }
        public decimal Stock { get; set; }
        public string Familia { get; set; }
        public string Agente { get; set; }

    }
}
