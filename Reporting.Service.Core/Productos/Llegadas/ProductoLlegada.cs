using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Productos.Llegadas
{
    public class ProductoLlegada
    {
        public int Sequence { get; set; }

        public string Sku { get; set; }

        public string Nombre { get; set; }

        public string Contenedor { get; set; }

        public int Envio { get; set; }

        public string FechaSalida { get; set; }

        public string FechaPuerto { get; set; }

        public string FechaLlegada { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public string Nom { get; set; }
        public DateTime? VencimientoNom { get; set; }
        public string FraccionArancelaria { get; set; }
        public string Certificado { get; set; }
        public DateTime? EmisionCertificado { get; set; }

        public string Proveedor { get; set; }
        public decimal PrecioFlete { get; set; }
    }
}
