using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Anexo.Modulo
{
    public class Modulo : BusinessObject<int>
    {

        public string Nombre { get; set; }

        public string Sistema { get; set; }

        public bool Activo { get; set; }

    }
}
