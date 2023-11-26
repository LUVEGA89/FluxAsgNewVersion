using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.CompletarFacturacionRecibida
{
    public class CompletarFacturacionRecibidaCriteria : Criteria
    {
        public DateTime? Del { get; set; }
        public DateTime? Al { get; set; }
        public string RfcReceptor { get; set; }
    }
}
