using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Autorización
{
    public class AutorizacionCriteria : Criteria
    {
        public bool? status { get; set; }
    }
}
