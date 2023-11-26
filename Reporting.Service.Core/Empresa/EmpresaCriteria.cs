using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Empresa
{
    public class EmpresaCriteria : Criteria
    {
        public EmpresaCriteria()
        {
            this.Activo = 1;
        }
        public int Activo { get; set; }
    }
}
