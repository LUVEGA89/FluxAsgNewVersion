using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.MetasCobranza
{
    public class Meta
    {
        public int Identifier { get; set;}
        public DateTime FechaMeta { get; set; }

        public decimal Vencido { get; set; }
        public decimal Cobrado { get; set; }
        public decimal MetaMonto { get; set; }        


        //public decimal VencidoAnioPasado { get; set; }
        //public decimal VencidoAnioActual { get; set; }
        //public decimal MetaAnioPasado { get; set; }
        //public decimal MetaAnioActual { get; set; }
        //public decimal CobradoAnioPasado { get; set; }
        //public decimal CobradoAnioActual { get; set; }

        public DateTime RegistradoEl { get; set; }
        public string RegistradoPor { get; set; }
        public DateTime? ActualizadoEl { get; set; }
        public string ActualizadoPor { get; set; }

        public Canal Canal { get; set; }

        public List<Canal> Detalles { get; set; }
    }
}
