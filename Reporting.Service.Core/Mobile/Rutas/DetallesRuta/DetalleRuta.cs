using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Mobile.Rutas.DetallesRuta
{
    public class DetalleRuta : BusinessObject<int>
    {
        public DetalleRuta()
        {
            this.Evidencias = new List<Evidencias.Evidencia>();
        }

        public int Pedido { get; set; }
        public List<Evidencias.Evidencia> Evidencias { get; set; }
        public StatusDetalleRuta Status { get; set; }
        public string Ubicacion { get; set; }
    }
}
