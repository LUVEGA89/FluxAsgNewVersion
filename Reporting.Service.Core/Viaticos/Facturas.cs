using Reporting.Service.Core.Viaticos.Viaticos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Viaticos
{
    public class Facturas
    {
        public int Sequence { get; set; }
        public string XMLString { get; set; }
        public DateTime FechaTimbrado { get; set; }
        public string UUID { get; set; }
        public decimal Monto { get; set; }
        public string UsoCFDI { get; set; }
        public string FormaPago { get; set; }

        public FormaPagoSAT FormaPagoSAT { get; set; }

        public UsoCFDI UsoCFDISAT { get; set; }

        public bool FileExistPDF { get; set; }

        public ProductoServicioSAT ProductoServicioSAT { get; set; }

        public List<FacturaConcepto> Conceptos { get; set; }

        public int DetalleSolicitud { get; set; }
    }
}
