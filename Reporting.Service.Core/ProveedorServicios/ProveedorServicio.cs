using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.ProveedorServicios
{
    public class ProveedorServicio : BusinessObject<int>
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
