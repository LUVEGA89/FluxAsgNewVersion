using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Viaticos.Viaticos
{
    public class Sucursal : BusinessObject<int>
    {
        public string Nombre { get; set; }
    }
}
