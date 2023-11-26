using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Productos
{
    public class ProductoChina
    {
        public string SKU { get; set; }
        public string Marca { get; set; }
        public string Articulo { get; set; }
        public string Empaque { get; set; }
        public string Familia { get; set; }
        public string SubFamilias { get; set; }
        public string Inner { get; set; }
        public string Master { get; set; }
        public string Proveedor { get; set; }
        public string Codigo { get; set; }
        public decimal Costo_USD { get; set; }
        public decimal Costo_MXN { get; set; }
        public int Stock_CEDIS { get; set; }
        public string Accesorios { get; set; }
        public string Subcategoria { get; set; }
        public string Subcategoria2 { get; set; }
        public string Clasificacion { get; set; }
        public string Categoria { get; set; }
        public string Tipo { get; set; }
        public int Activo { get; set; }
        public string Descripcion { get; set; }
        public string TipoPrecio { get; set; }
        public decimal PrecioUltra { get; set; }
        public int Estatus { get; set; }
        public string Canal { get; set; }
        public string StatusC { get; set; }
        public string Cliente { get; set; }
        public string Igi { get; set; }
        public string Nom { get; set; }
        public DateTime? NomVigencia { get; set; }
        public DateTime? UltimaFechaLlegada { get; set; }
        public int CantidadRecibida { get; set; }
    }
}
