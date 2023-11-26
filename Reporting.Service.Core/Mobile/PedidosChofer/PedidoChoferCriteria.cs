using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Mobile.PedidosChofer
{
    public class PedidoChoferCriteria : Criteria
    {
        public string Chofer { get; set; }
        public int Ruta { get; set; }
    }
}
