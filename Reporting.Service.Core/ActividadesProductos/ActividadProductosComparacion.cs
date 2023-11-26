using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.ActividadesProductos
{
    public class ActividadProductosComparacion: BusinessObject<int>
    {
        public int Actividad { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public decimal PrecioLocal { get; set; }
        public int MinPiezas { get; set; }
        public string TipoPrecio { get; set; }
        public decimal PrecioCompetencia { get; set; }
        public bool Estatus { get; set; }
    }
}
