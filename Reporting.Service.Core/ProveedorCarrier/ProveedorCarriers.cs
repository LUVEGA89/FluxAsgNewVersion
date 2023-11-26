using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Resporting.Service.Core.ProveedorCarrier
{
    public class ProveedorCarriers : BusinessObject<int>
    {
        public string Nombre { get; set; }
        public string NombreLargo { get; set; }
        public int TipoAuto { get; set; }
        public string Placas { get; set; }
        public string DescripcionVehiculo { get; set; }
        public string Capacidad { get; set; }

    }
}