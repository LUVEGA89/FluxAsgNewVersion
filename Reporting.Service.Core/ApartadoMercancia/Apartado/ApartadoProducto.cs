using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.ApartadoMercancia.Apartado 
{
    public class ApartadoProducto: BusinessObject<int>
    {
        public string SKU { get; set; }
        public int apartado_id { get; set; }
        public int piezas { get; set; }
        public int piezasLiberadas { get; set; }
        public int piezasDisponibles { get; set; }

        //Para venta detalle en el rotación artículos

        public decimal precio { get; set; }
        public decimal precioTotal { get; set; }
        public string cliente { get; set; }

    }
}
