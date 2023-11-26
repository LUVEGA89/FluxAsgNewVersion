using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Empresa
{
    public class Empresa : BusinessObject<int>
    {
        public string Nombre { get; set; }

        public string RazonSocial { get; set; }
        
        public string Entrega { get; set; }
        public int Estatus { get; set; }
    }
}
