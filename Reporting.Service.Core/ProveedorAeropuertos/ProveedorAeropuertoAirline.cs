using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Resporting.Service.Core.ProveedorAeropuertos
{
    public class ProveedorAeropuertoAirline : BusinessObject<int>
    {
        public string Aerolinea { get; set; }
        public string NumerViajeroFrecuente { get; set; }
        public string EstatusAerolinea { get; set; }
        public int ContadorGeneral { get; set; }

    }
}
