using Reporting.Service.Core.Indicadores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class EnvioModel
    {
        public decimal NuevosPorcentaje { get; set; }
        public List<DiasDeEnvio> DetalleNuevo { get; set; }
        public decimal CompletosPorcentaje { get; set; }
        public List<DiasDeEnvio> DetalleCompleto { get; set; }
        public decimal ConsolidadoPorcentaje { get; set; }
        public List<DiasDeEnvio> DetalleConsolidado { get; set; }
    }
}