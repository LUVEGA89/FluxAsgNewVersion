using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Disclaimers
{
    public class Disclaimer : BusinessObject<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
