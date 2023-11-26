using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Common
{
    public class User
    {
        public string Sequence { get; set; }
        public string Email { get; set; }
        public string Usuario { get; set; }
        public string Rol { get; set; }
        public int SequenceId { get; set; }
        public int EnProceso { get; set; }
        public int Finalizados { get; set; }
        public string Area { get; set; }
        public string CodigoEmpleado { get; set; }

    }
}
