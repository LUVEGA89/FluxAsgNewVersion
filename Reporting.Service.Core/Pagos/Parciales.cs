using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Pagos
{
    public class Parciales
    {
        public int Sequence { get; set; }
        public int Documento { get; set; }
        public string TipoPago { get; set; }
        public string Banco { get; set; }
        public string FechaDeposito { get; set; }
        public decimal Monto { get; set; }
        public string Referencia { get; set; }
        public string NoCuenta { get; set; }
        public string Beneficiario { get; set; }
        public string Imagen { get; set; }
        public string Origen { get;  set; }
        public int Aplicado { get;  set; }
        public int Eliminado { get;  set; }
    }
}
