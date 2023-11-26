using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Servicio
{
    public class Servicio : BusinessObject<int>
    {
        public string Nombre { get; set; }

        public DateTime FechaCreacion { get; set; }

        public int Estatus { get; set; }
    }
}
