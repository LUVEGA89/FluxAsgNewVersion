using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Reportes.Retail
{
    public class RetailCriteria : Criteria
    {

        public DateTime Del { get; set; }

        public DateTime Al { get; set; }

        public RetailKind Tipo { get; set; }

        public string Cliente { get; set; }

        public string Vendedor { get; set; }

        public string Categoria { get; set; }

        public string Categoria1 { get; set; }

        public string Clasificado { get; set; }

        public string Familia { get; set; }

        public int Mes { get; set; }

        public string ClienteName { get; set; }
    }


    public enum RetailKind
    {
        Vacio = 0,// lista de familias del primer nivel
        Mes =1,
        SKU = 2,
        Categoria = 3,
        Categoria1 = 4,
        Clasificacion = 5,
        Cliente = 6,

        ClientePzs = 100,
        ClientePzsSKU= 200
    }
}
