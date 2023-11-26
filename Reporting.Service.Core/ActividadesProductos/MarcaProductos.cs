using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.ActividadesProductos
{
    public class MarcaProductos: BusinessObject<int>
    {
        public string Marca { get; set; }
        public bool Estatus { get; set; }
    }
}
