using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.CompletarFacturacionRecibida.CuentasGastos
{
    public class CuentasGastosCriteria : Criteria
    {
        public string Empresa { get; set; }
    }
}
