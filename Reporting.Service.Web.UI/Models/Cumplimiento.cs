using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reporting.Service.Web.UI.Models
{
    public class Cumplimiento
    {
        public string Del { get; set; }
        public string Al { get; set; }
        public string Anio { get; set; }
        public string Mes { get; set; }
        public string Tienda { get; set; }
        public string Meta { get; set; }
        public string Venta { get; set; }
        public string VentaAnioPasado { get; set; }
        public string MetaDistribuidor { get; set; }
        public string VentaDistribuidor { get; set; }
        public string VentaAnioPasadoDistribuidor { get; set; }
        public string Porcentaje { get; set; }
        public string PorcentajeDistribuidor { get; set; }
        public string CumplimientoMeta { get; set; }
        public string CumplimientoMetaDistribuidor { get; set; }

    }
}

