using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;
namespace Reporting.Service.Core.ActividadesProductos
{
    public class PreciosCompetencia: BusinessObject<int>
    {
        public int IdSKU { get; set; }
        public string TipoPrecio { get; set; }
        public decimal Precio { get; set; }
        public int NumPiezas { get; set; }
    }
}
