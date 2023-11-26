using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Reportes.Mayoreo
{
    public class Meses: BusinessObject<int>
    {
        public string MesName { get; set; }
    }
}
