using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Buzon.Categoria
{
    public class Categoria : BusinessObject<int>
    {
        public string Nombre { get; set; }

        public bool Estatus { get; set; }

        
    }
}
