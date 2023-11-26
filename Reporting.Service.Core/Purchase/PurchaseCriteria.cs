using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;
using WikiCore.Data;

namespace Reporting.Service.Core.Purchase
{
    public class PurchaseCriteria: Criteria
    {

        public DateTime Del { get; set; }

        public DateTime Al { get; set; }

        public int Descontinuados { get; set; }
        public string Familia { get; set; }
        public int PorFamilia { get; set; }
    }
}
