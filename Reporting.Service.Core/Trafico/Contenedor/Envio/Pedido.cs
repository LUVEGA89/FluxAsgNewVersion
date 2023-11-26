using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Trafico.Contenedor.Envio
{
    public class Pedido: BusinessObject<string>
    {
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal TotalImporte { get; set; }
        public string tipoCambio { get; set; }
        public string nom { get; set; }
        public DateTime fechaVencimiento { get; set; }
        public int arancel { get; set; }

        //Para Costo Venta
        public string agente { get; set; }

        //Para Ventas Retail
        public int cantidadAA { get; set; }
        public decimal TotalImporteAA { get; set; }

        //Para reporte SKUS
        public string status { get; set; }
        public string contenedor { get; set; }
        public string certificado { get; set; }
        public DateTime fechaCertificado { get; set; }
        public string ordCompra { get; set; }
        public string envios { get; set; }
        public int cantEnvio { get; set; }
        public int stock { get; set; }
        public int U_Maximo { get; set; }
        public int U_Minimo { get; set; }


        public string familia { get; set; }
    }
}
