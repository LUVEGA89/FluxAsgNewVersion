using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Viaticos.Viaticos
{
    public class ProductoServicioSAT : BusinessObject<int>
    {
        public string Codigo { get; set; }

        public string Descripcion { get; set; }

        public string VigenciaDel { get; set; }

        public bool Activo { get; set; }

        public bool IsOcupado { get; set; }
    }
}
