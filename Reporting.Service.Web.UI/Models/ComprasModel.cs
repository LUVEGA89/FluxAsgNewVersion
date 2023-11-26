using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class ComprasModel
    {
        public string CodigoA { get; set; }
        public int TotalA { get; set; }
        public int ConStockA { get; set; }
        public decimal PorcentajeA { get; set; }
        public string CodigoB { get; set; }
        public int TotalB { get; set; }
        public int ConStockB { get; set; }
        public decimal PorcentajeB { get; set; }
        public string CodigoC { get; set; }
        public int TotalC { get; set; }
        public int ConStockC { get; set; }
        public decimal PorcentajeC { get; set; }
    }
}