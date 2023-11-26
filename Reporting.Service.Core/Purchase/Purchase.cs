using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Purchase
{
    public class Purchase : BusinessObject<int>
    {
        public string ItemCode { get; set; }

        public decimal Precio { get; set; }

        public int DocSap { get; set; }

        public DateTime FechCreacion { get; set; }

        public DateTime? FechPagoAnticipo { get; set; }

        public string Contenedor { get; set; }

        public string EstatusBarco { get; set; }

        public DateTime? FechSalida { get; set; }

        public DateTime? FechLlegadaPuerto { get; set; }

        public DateTime? FechLlegadaPantaco { get; set; }

        public decimal Cantidad { get; set; }

        public DateTime FechPrometida { get; set; }

        public string Envio { get; set; }

        public decimal CantidadOC { get; set; }


        public string CBM { get; set; }

        public string Puerto { get; set; }

        public string Anticipo { get; set; }

        public string Produccion { get; set; }


        public string BuqueViaje { get; set; }
        public string BillOfLanding { get; set; }
        public string BlMaster { get; set; }
        public string Origen { get; set; }
        public DateTime? FechaLlegadaAPuerto { get; set; }
        public DateTime? FechaPrometida { get; set; }
        public string Proveedor { get; set; }
        public string AgenteAduanal { get; set; }

    }
}
