using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ActividadesProductos
{
    public class NuevoSKUSugeridoCriteria:Criteria
    {
        public int IdSKU { get; set; }
        public string SKUCode { get; set; }
        public DateTime? Del { get; set; }
        public DateTime? Al { get; set; }
    }
}
