using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Viaticos
{
    public class ViaticosCriteria : Criteria
    {

        public string UUID { get; set; }

        public int ClaveProServ { get; set; }

        public decimal Total { get; set; }

        public string Descripcion { get; set; }

        public decimal Importe { get; set; }

        public string Cantidad { get; set; }

        public int Viatico { get;  set; }

        public int ViaticoDetalle { get; set; }
        
        public string Sku { get; set; }
        
    }
}
