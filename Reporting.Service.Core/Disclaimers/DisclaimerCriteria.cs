using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Disclaimers
{
    public class DisclaimerCriteria : Criteria
    {
        public int? Servicio { get; set; }
    }
}
