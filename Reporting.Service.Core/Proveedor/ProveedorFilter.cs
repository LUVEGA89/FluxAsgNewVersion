using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Resporting.Service.Core.Proveedor
{
    public class ProveedorFilter: Criteria
    {
        public int Activo { get; set; }
        public int? Mision { get; set; }
        public int? Cotizacion { get; set; }
    }
}
