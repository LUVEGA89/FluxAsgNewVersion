using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.ActividadesProductos
{
    public class PriceProducto
    {
        public decimal PrecioLocal { get; set; }
        public decimal PrecioMayoreoTiendas { get; set; }
        public decimal PrecioMenudeoTiendas { get; set; }
        public decimal PrecioDistribuidorLocal { get; set; }
        public decimal PrecioDistribuidorForanea { get; set; }
        public int StockCedis { get; set; }
        public string TipoPrecio { get; set; }
    }
}
