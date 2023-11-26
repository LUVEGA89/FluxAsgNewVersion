using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Reporting.Service.Core.Productos;

namespace Reporting.Service.Web.UI.Models
{
    public class Producto
    {
        public string ItemCode { get; set; }
        public string Familia { get; set; }
        public string MayoreoDesde { get; set; }
        public string MayoreoDistribuidorDesde { get; set; }
        public string Lista10 { get; set; }
        public string Lista9 { get; set; }
        public string Lista15 { get; set; }
        public string FRN { get; set; }
        public string Lista33 { get; set; }
        public string Lista33iva { get; set; }
        public string mbvsstgp33 { get; set; }
        public string Lista25 { get; set; }
        public string Lista25iva { get; set; }
        public string mbvsstgp25 { get; set; }
        public string Lista14 { get; set; }
        public string Lista14iva { get; set; }
        public string mbvsstgp14 { get; set; }
        public string Lista29 { get; set; }
        public string Lista29iva { get; set; }
        public string mbvsstgp29 { get; set; }
        public string Lista28 { get; set; }
        public string Lista28iva { get; set; }
        public string mbvsstgp28 { get; set; }

        public List<TipoPrecioArt> ListPrecioArts { get;  set; }
    }
}

