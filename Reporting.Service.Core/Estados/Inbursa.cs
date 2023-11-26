using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Estados
{
    public class Inbursa
    {
        public string Sequence { get; set; }
        public string Fecha { get; set; }
        public string Referencia { get; set; }
        public string ReferenciaExterna { get; set; }
        public string ReferenciaLeyenda { get; set; }
        public string ReferenciaNumerica { get; set; }
        public string Movimiento { get; set; }
        public string Cargo { get; set; }
        public string Abono { get; set; }
        public string Saldo { get; set; }
        public string Ordenante { get; set; }
        public string Sucursal { get; set; }
        public string FecDiaVenta { get; set; }
        public string TipoPago { get; set; }
        public string Comentario { get; set; }
        public string Estatus { get; set; }
    }
}