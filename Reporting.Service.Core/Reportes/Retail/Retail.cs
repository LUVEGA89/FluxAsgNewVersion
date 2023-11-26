using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Reportes.Retail
{
    public class Retail : BusinessObject<int>
    {

        public Reportes.Mayoreo.Cliente Cliente { get; set; }

        public Reportes.Mayoreo.Ejecutivo Vendedor { get; set; }

        public int YEAR { get; set; }

        public string Familia { get; set; }

        public decimal Cantidad { get; set; }

        public decimal Total { get; set; }

        public int Mes { get; set; }

        public string ItemCode { get; set; }

        public decimal Cantidad2017 { get; set; }

        public decimal Total2017 { get; set; }

        public decimal Total2018 { get; set; }

        public decimal Cantidad2018 { get; set; }

        public decimal Precio2017 { get; set; }

        public decimal Precio2018 { get; set; }

        public decimal Precio { get; set; }

        public decimal VarianzaTotal { get; set; }

        public decimal VarianzaProcentaje { get; set; }

        public string Categoria { get; set; }

        public string Categoria1 { get; set; }

        public string Clasificado { get; set; }

        public decimal Promedio2017 { get; set; }

        public decimal Promedio2018 { get; set; }

        public decimal Promedio { get; set; }
    }
}
