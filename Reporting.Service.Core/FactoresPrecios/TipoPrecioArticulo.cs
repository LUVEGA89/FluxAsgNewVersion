using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.FactoresPrecios
{
    public class TipoPrecioArticulo : BusinessObject<int>
    {

        public string Descripcion { get; set; } 
        public int Estatus { get; set; }

    }
}
