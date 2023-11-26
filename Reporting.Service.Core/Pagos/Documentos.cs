using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Pagos
{
    public class Documentos
    {
        public int Folio { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Fecha { get; set; }
        public string Vencimiento { get; set; }
        public int Referencia { get; set; }
        public string Documento { get; set; }
        public decimal Total { get; set; }
        public decimal Pagado { get; set; }
        public decimal Saldo { get; set; }
    }
}
