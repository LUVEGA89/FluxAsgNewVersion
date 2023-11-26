using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Precio
{
    public class TipoPrecioCriteria : Criteria
    {
        public TipoPrecioCriteria()
        {
            this.Estatus = 1;
        }

        public int Estatus { get; set; }
    }
}
