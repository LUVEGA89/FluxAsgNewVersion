using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Auditoria
{
    public class DetalleAuditoria
    {
        public int Sequence { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Segmento { get; set; }
        public string Rubro { get; set; }
        public string DescripcionRubro { get; set; }
        public int Puntuacion { get; set; }
        public int PuntosAplicados { get; set; }
        public string Aplica { get; set; }
        public string Observaciones { get; set; }
        public DateTime RegistradoEl { get; set; }
        public DateTime FinalizadoEl { get; set; }
        public decimal TotalPuntuacion { get; set; }
        public string Evidencia { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Exactitud { get; set; }
        public IList<ImagenDetalle> Imagenes { get; set; }
    }
}
