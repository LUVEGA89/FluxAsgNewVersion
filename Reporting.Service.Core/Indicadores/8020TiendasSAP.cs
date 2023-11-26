using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Indicadores
{
    public class _8020TiendasSAP
    {
        public string Sku { get; set; }
        public decimal Stock { get; set; }
        public decimal TotalPiezasSAP { get; set; }
        public decimal TotalPiezasTienda { get; set; }
        public decimal SumaPiezasTiendaSap { get; set; }
        public decimal PorcentajeCantidad { get; set; }
        public decimal TotalVentaSAP { get; set; }
        public decimal TotalVentaTienda { get; set; }
        public decimal SumaVentaTiendaSap { get; set; }
        public decimal PorcentajeVenta { get; set; }
        public bool StockValidoNMeses { get; set; }
        public bool En8020Piezas { get; set; }
        public bool En8020Venta { get; set; }
        public bool EnPiezasVenta8020 { get; set; }
        public int NMeses { get; set; }
        public decimal PrecioActual { get; set; }
        public decimal UltimoPrecio { get; set; }
        public decimal PiezasEnPedidos { get; set; }
        public DateTime FechaEstimadaPedidos { get; set; }
        public decimal PiezasEnTransito { get; set; }
        public DateTime FechaEstimadaTransito { get; set; }

    }
}
