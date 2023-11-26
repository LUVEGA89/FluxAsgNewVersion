using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ActividadesProductos
{
    public class ComentarioCriteria:Criteria
    {
        public int Actividad { get; set; }
        public int Comentario { get; set; }
    }
}
