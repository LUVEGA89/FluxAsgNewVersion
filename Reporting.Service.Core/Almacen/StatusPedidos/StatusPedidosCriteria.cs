using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Almacen.StatusPedidos
{
    public class StatusPedidosCriteria : Criteria 
    {
        public DateTime? Inicio { get; set; }
        public DateTime? Termino { get; set; }
    }
}
