using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.EmpresasSap
{
    public class EmpresasSapCriteria : Criteria
    {
        public EmpresasSapCriteria()
        {
            this.Activo = 1;
        }
        public int Activo { get; set; }
    }
}
