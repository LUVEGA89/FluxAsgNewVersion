using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.FactoresPrecios
{
    public class TipoPrecioCanal: BusinessObject<string>
    {
        
        public string Descripcion { get; set; }
    }
}
