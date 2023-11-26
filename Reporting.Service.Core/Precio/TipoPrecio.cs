using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Precio
{
    public class TipoPrecio: BusinessObject<int>
    {
        public string Descripcion { get; set; }
        public int TipArt { get; set; }
        public string Art { get; set; }
        public int SequenceFamilia { get; set; }

    }
}
