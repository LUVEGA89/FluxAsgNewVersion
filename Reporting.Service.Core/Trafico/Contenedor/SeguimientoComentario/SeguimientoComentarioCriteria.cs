using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor.SeguimientoComentario
{
    public class SeguimientoComentarioCriteria: Criteria
    {
        public int seguimientoId { get; set; }
        public int parentID { get; set; }
    }
}
