using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.NotaCredito
{
    public class NotaCreditoItem : BusinessObject<int>
    {
        public string ItemCode { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public int Estatus { get; set; }

        public decimal Descuento { get; set; }

        public string CuentaContable { get; set; }

        public string AlmacenWhsCode { get; set; }
    }

    public class NotaCreditoItemAlmacen
    {
        public int Identifier { get; set; }

        public int FolioNotaCredito { get; set; }

        public string Articulo { get; set; }

        public int Cantidad { get; set; }

        public string Almacen { get; set; }

        public int Orden { get; set; }

        public int Estatus { get; set; }

        public DateTime RegistradoEl { get; set; }

    }      
}
