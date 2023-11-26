using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core
{
    public class Folio
    {
        public int Sequence { get; set; }
        public string FechaPedido { get; set; }
        public string Departamento { get; set; }
        public string UsuarioFolio { get; set; }
        public int EstatusFolio { get; set; }
        public string Comentario { get; set; }
    }
}
