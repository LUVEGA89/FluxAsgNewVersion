using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Anexo.Empresa
{
    public class Empresa : BusinessObject<int>
    {
        public bool Activo { get; set; }
        public string Nombre { get; set; }
        public string Origen { get; set; }
    }
}
