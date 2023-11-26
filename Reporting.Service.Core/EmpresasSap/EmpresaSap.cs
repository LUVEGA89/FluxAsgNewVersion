using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.EmpresasSap
{
    public class EmpresaSap : BusinessObject<int>
    {
        public string Nombre { get; set; }
        public string Rfc { get; set; }
        public string NombreBaseDatos { get; set; }
    }
}
