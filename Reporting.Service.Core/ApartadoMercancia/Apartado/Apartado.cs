using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.ApartadoMercancia.Apartado
{
    public class Apartado : BusinessObject<int>
    {
        public string cliente { get; set; }
        public string agente { get; set; }
        public string agente_email { get; set; }
        public int folioSAP { get; set; }
        public DateTime fechaApartado { get; set; }
        public DateTime fechaLiberacion { get; set; }
        public string motivo { get; set; }
        public int status { get; set; }
        public string canal { get; set; }
        public List<Evidencia> archivos { get; set; }
        public List<ApartadoProducto> productos { get; set; }
    }
}
