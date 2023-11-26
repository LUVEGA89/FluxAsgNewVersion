using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ApartadoMercancia.Cliente
{
    public class ClienteCriteria: Criteria
    {
        public string nombre { get; set; }
        public string emailAgente { get; set; }
    }
}
