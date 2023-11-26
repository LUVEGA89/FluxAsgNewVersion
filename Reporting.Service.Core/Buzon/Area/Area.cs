using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Buzon.Area
{
    public class Area : BusinessObject<int>
    {
        public string Nombre { get; set; }

        public bool Estatus { get; set; }

        public string Email { get; set; }

    }
}
