using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Viaticos
{
    public class DetalleSolicitud
    {
        public int Sequence { get; set; }

        public int SequenceItem { get; set; }

        public int Id_Departamento { get; set; }

        public string Departamento { get; set; }

        public string Sku_Producto { get; set; }

        public string CentroCosto { get; set; }

        public string Producto { get; set; }

        public int Id_Sucursal { get; set; }

        public string Sucursal { get; set; }        

        public DateTime FechaSolicitud { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public decimal Monto { get; set; }

        public decimal Presupuesto { get; set; }

        public decimal MontoActual { get; set; }

        public decimal Disponible { get; set; }

        public int Estado { get; set; }

        public string Comentarios { get; set; }

        public string Nombre { get; set; }

        public string Email { get; set; }

        public decimal MontoSubido { get; set; }

        public int FolioSAP { get; set; }

        public int Cheque { get; set; }

        public IList<Facturas> Facturas { get; set; }

        public Usuario Usuario { get; set; }   
        


    }
}
