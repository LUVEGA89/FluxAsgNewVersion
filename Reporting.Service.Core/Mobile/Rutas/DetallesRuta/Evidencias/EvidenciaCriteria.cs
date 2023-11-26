using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Mobile.Rutas.DetallesRuta.Evidencias
{
    public class EvidenciaCriteria : Criteria
    {
        public int? Pedido { get; set; }
    }
}
