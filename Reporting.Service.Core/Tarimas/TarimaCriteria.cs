using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Tarimas
{
    public class TarimaCriteria : Criteria
    {
        public TarimaCriteria()
        {
            this.EstatusSincronizacion = 1; //Agregadas de SAP a SIE
            //Se onitiran todos los estatus restantes
        }
        public DateTime? Inicio { get; set; }
        public DateTime? Termino { get; set; }
        public int? EstatusSincronizacion {get; set;}
        public decimal? Tarimas { get; set; }
        public string Completados { get; set; }
        public string Cancelados { get; set; }
    }
}
