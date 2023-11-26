using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.FactoresPrecios
{
    public class TipoPrecioFactorCriteria : Criteria
    {
        public TipoPrecioFactorCriteria()
        {
            this.IdTipoPrecioArt = -1;
        }

        public int IdTipoPrecioArt { get; set; }

        public string IdTipoPrecioCanal { get; set; }

    }
}
