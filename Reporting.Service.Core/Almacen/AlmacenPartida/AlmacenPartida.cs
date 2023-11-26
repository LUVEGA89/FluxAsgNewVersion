using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Almacen.AlmacenPartida
{
    public class AlmacenPartida : BusinessObject<string>
    {
        public string Nombre { get; set; }
    }
}
