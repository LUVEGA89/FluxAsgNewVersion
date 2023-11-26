using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Reportes.CostosKpis
{
    public class IndicadoresCostosKpis
    {
        public string Sku { get; set; }
        public int DocSap { get; set; }
        public int DocSapAnterior { get; set; }
        public string ProveedorActual { get; set; }
        public string ProveedorAnterior { get; set; }
        public decimal PrecioActual { get; set; }
        public decimal PrecioAnterior { get; set; }
        public decimal Diferencia { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Porcentaje { get; set; }
        public string Comentarios { get; set; }
        public string Fecha { get; set; }
    }
}
