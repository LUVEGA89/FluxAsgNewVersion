using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Seguimiento
{
    public class Llegadas
    {
        public int Sequence { get; set; }
        public int Identifier { get; set; }
        public string Contenedor { get; set; }
        public string Proveedor { get; set; }
        public string Forwarder { get; set; }
        public DateTime Embarcada { get; set; }
        public DateTime LlegadaPuerto { get; set; }
        public decimal CostoFlete { get; set; }
        //public List<Comentario> Comentarios { get; set; }
        public DateTime SalidaPuerto { get; set; }
        public DateTime LlegadaPatco { get; set; }
        public DateTime SalidaPatco { get; set;}
        public EstadoCompra Estado { get; set; }

        public DateTime CuentaDeGastosEly { get; set; }
        public DateTime CuentaDeGastosFinanzas { get; set; }
        public DateTime CuentaDeGastosTrafico { get; set; }
        public int IsOk { get; set; }
        public string Comentarios { get; set; }
        public DateTime LibDespacho { get; set; }
        public DateTime LibTransito { get; set; }

    }
}
