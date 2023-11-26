using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.ActividadesProductos
{
    public class Respuesta : BusinessObject<int>
    {
        public DateTime RegistradorEl { get; set; }
        public string ReigstradoPor { get; set; }
        public string Comentario { get; set; }
        public bool Estatus { get; set; }
        public int NodoPadre { get; set; }
        public int PermisosRol { get; set; }
    }
}
