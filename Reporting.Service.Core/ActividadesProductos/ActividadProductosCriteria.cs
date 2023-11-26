using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ActividadesProductos
{
   public class ActividadProductoCriteria:Criteria
    {
        public string Producto { get; set; }
        public DateTime Del { get; set; }
        public DateTime Al { get; set; }
        public int PermisosRol { get; set; } 
        public int CodeSKU { get; set; }
    }
}
