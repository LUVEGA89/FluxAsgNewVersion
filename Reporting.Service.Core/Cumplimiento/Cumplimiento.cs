using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Cumplimiento
{
    public class Cumplimiento
    {
        public DateTime Del { get; set; }
        public DateTime Al { get; set; }
        public string Anio { get; set; }
        public string Mes { get; set; }
        public string Tienda { get; set; }
        public decimal Meta { get; set; }
        public decimal Venta { get; set; }
        public decimal VentaAnioPasado { get; set; }
        public decimal MetaDistribuidor { get; set; }
        public decimal VentaDistribuidor { get; set; }
        public decimal VentaAnioPasadoDistribuidor { get; set; }
        public decimal Porcentaje { get; set; }
        public decimal PorcentajeDistribuidor { get; set; }
        public decimal CumplimientoMeta { get; set; }
        public decimal CumplimientoMetaDistribuidor { get; set; }

        public decimal PorcentajeMeta { get; set; }
    }
}

