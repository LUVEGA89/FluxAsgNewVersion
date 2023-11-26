using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Garantias
{
    public class Garantia : BusinessObject<int>
    {
        public string ordenCompra;

        public string Sku { get; set; }

        public string Descripcion { get; set; }

        public string Marca { get; set; }

        public string Familia { get; set; }

        public string Categoria { get; set; }

        public string SubCategoria { get; set; }        

        public decimal CostoMx { get; set; }

        public decimal Cantidad { get; set; }

        public DateTime Fecha { get; set; }

    }
}
