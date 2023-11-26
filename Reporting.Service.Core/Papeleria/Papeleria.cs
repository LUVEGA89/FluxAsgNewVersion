using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Papeleria
{
    public class Papeleria
    {
        public int Sequence { get; set; }
        public int Pedido { get; set; }
        public string FolioSap { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string SubArea { get; set; }
        public string Marca { get; set; }
        public decimal Stock { get; set; }
        public int Cantidad { get; set; }
        public int CantidadAprobada { get; set; }
        public decimal CantidadReporte { get; set; }
        public string UsuarioPedido { get; set; }
        public string UsuarioFolio { get; set; }
        public int EstatusPedido { get; set; }
        public string FechaPedido { get; set; }
        public string Comentario { get; set; }
        public string CentroCosto { get; set; }
        public string Area { get; set; }
        public Int32 Foliox { get; set; }
        public string ComentarioFolio { get; set; }
        public string ComentarioSistema { get; set; }
        public int EstatusFolio { get; set; }
        public decimal Total { get; set; }
    }
}
