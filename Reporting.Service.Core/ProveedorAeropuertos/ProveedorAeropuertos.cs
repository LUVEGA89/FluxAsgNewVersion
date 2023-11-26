using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Resporting.Service.Core.ProveedorAeropuertos
{
    public class ProveedorAeropuertos : BusinessObject<int>
    {

        public string Clabe { get; set; }
        public int ContadorGeneral { get; set; }

    }
}