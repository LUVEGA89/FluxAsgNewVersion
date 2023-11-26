using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;
namespace Reporting.Service.Core.ActividadesProductos
{
    public class ImagenesNuevoSKU:BusinessObject<int>
    {
        public int IdSKU { get; set; }
        public string Imagen { get; set; }
        public bool Estatus { get; set; }
    }
}
