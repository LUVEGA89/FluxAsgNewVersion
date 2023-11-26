using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Viaticos.Viaticos
{
    public class ConceptoTraslado : BusinessObject<int>
    {

        public decimal Importe { get; set; }

        public decimal TasaOCuota { get; set; }

        public string TipoFactor { get; set; }

        public decimal Base { get; set; }        

    }
}
