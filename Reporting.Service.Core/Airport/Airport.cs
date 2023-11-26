using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Resporting.Service.Core.Airport
{
    public class Airport : BusinessObject<int>
    {
        public Airport() { }
        public string itemName { get; set; }
        public string id { get; set; }
    }
}
