using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Reportes.Mayoreo
{
    public class Cliente : BusinessObject<string>
    {

        public string Nombre { get; set; }

        public string Tipo { get; set; }

    }
}
