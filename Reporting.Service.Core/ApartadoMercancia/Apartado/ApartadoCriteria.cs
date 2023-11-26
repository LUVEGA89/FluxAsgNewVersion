using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ApartadoMercancia.Apartado
{
    public class ApartadoCriteria: Criteria
    {
        public string coincidenciaCanal { get; set; }

        public int estado { get; set; }

        public DateTime Inicio { get; set; }

        public DateTime Fin { get; set; }
    }
}
