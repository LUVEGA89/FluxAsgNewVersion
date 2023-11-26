using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Cliente
{
    public class ClienteFilter: Criteria
    {
        public int Activo { get; set; }
    }
}
