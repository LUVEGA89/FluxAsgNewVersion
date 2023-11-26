using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Mobile.Rutas.DetallesRuta.Evidencias
{
    public class Evidencia : BusinessObject<int>
    {
        public string Imagen { get; set; }
        public EvidenciaKind Tipo { get; set; }
        public int Pedido { get; set; }
    }
}
