using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Reportes.Mayoreo
{
    public class Mayoreo : BusinessObject<int>
    {
        public string Familia { get; set; }

        public string ItemCode { get; set; }

        public decimal Cantidad { get; set; }

        public decimal Total { get; set; }

        public decimal Precio { get; set; }

        public string Categoria { get; set; }

        public string CardCode { get; set; }

        public string Cliente { get; set; }


        public decimal Cantidad2018 { get; set; }

        public decimal Total2018 { get; set; }

        public decimal Precio2018 { get; set; }


        public decimal Cantidad2017 { get; set; }

        public decimal Total2017 { get; set; }

        public decimal Precio2017 { get; set; }


        public decimal VarianzaTotal { get; set; }

        public decimal VarianzaProcentaje { get; set; }

        public int Mes { get; set; }

        public string MesFormat { get; set; }

        public decimal SubTotal { get; set; }

        public decimal SubCantidad { get; set; }

        public int Tipo { get; set; }


        // auxiliar 
        public string CategoriaAux { get; set; }

        // auxiliar 1
        public string CategoriaAux1 { get; set; }

        public string Clasificado { get; set; }

        public string EstatusEjecutivo { get; set; }


        public Ejecutivo Ejecutivo { get; set; }


        public string StateS { get; set; }

    }
}
