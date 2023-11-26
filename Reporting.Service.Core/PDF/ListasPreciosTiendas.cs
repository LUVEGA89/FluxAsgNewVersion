using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.PDF
{
    public class ListasPreciosTiendas
    {
        public int Sequence { get; set; }
        public string NombreTienda { get; set; }
        public string Usuario { get; set; }
        public int IdListaPreciosSAP { get; set; }
        public string NombreListaPreciosSAP { get; set; }
    }
}
