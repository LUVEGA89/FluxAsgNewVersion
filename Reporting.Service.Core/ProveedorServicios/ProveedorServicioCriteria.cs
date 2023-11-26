using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ProveedorServicios
{
    public class ProveedorServicioCriteria : Criteria
    {
        public ProveedorServicioCriteria()
        {
            this.Activo = 1;
        }
        public int Activo { get; set; }
    }
}
