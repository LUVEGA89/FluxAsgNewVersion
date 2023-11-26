using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Reporting.Service.Core.Indicadores;
namespace Reporting.Service.Web.UI.Models
{
    public class Info8020TiendasSAPModel
    {
        public decimal VPPorcentaje { get; set; } 
        public List<_8020TiendasSAP> VPDetalle { get; set; }
        public decimal VPorcentaje { get; set; }
        public List<_8020TiendasSAP> VDetalle { get; set; }
        public decimal PPorcentaje { get; set; }
        public List<_8020TiendasSAP> PDetalle { get; set; }
        public decimal VPPorcentajeCostos { get; set; }
        public List<ReduccionCostosDetalle> VPCostosDetalle { get; set; }
        public decimal VPorcentajeCostos { get; set; } 
        public List<ReduccionCostosDetalle> VCostosDetalle { get; set; }
        public decimal PPorcentajeCostos { get; set; }
        public List<ReduccionCostosDetalle> PCostosDetalle { get; set; }
        public ExcelDocumento Documento { get; set; }
    }
}