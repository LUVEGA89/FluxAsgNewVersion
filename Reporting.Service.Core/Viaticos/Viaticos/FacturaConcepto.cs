using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Viaticos.Viaticos
{
    public class FacturaConcepto: BusinessObject<int>
    {
        public decimal Importe { get; set; }

        public decimal ValorUnitario { get; set; }

        public string Descripcion { get; set; }

        public int ClaveProdServ { get; set; }

        public string Cantidad { get; set; }

        public ConceptoTraslado ConceptoTraslado { get; set; }

        public bool IsOcupado { get; set; }

        public bool IsActivoSAT { get; set; }



    }
}
