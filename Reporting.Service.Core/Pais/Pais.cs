using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Pais
{
    public class Pais : BusinessObject<int>
    {
        public int Codigo { get; set; }

        public string Nombre { get; set; }

    }
}
