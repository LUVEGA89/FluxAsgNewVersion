using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Reportes.RotacionInventario
{
    public class RotacionInventario : BusinessObject<int>
    {
        public string Sku { get; set; }
        public string Descripcion { get; set; }
        public string Familia { get; set; }
        public string Estatus { get; set; }
        public int PromedioVtaMensualU { get; set; }
        public int Disponible { get; set; }
        public int PzaMesAnio { get; set; }
        public string PzaNombreMesAnio { get; set; }

        public int Valor_80_20_P { get; set; }

        public int Valor_80_20_U { get; set; }
    }
}
