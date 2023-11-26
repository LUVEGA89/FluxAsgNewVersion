using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.ActividadesProductos
{
    public class ActividadProductosFotos : BusinessObject<int>
    {
        public string Foto { get; set; }
        public bool Estatus { get; set; }
        public int Actividad { get; set; }
    }
}
