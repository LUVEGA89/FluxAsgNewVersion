using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Viaticos
{
    public class DetalleItemSolicitud
    {
        public int Sequence { get; set; }
        public string Producto { get; set; }
        public decimal Monto { get; set; }
        public string CodigoSat { get; set; }
    }
}
