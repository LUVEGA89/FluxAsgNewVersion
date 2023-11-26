using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Viaticos
{
    public class Solicitudes
    {
        public int Sequence { get; set; }
        public int FolioSAP { get; set; }
        public int Cheque { get; set; }
        public DateTime RegistradoEl { get; set; }
        public DateTime FechaRequisicion { get; set; }
        public decimal Total { get; set; }
        public string Sucursal { get; set; }
        public string Estado { get; set; }


        public Usuario Usuario { get; set; }

    }
}
