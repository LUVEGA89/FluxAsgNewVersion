using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Inventarios
{
    public class Facturas
    {
        /*Campos del SP*/
        public int DocNum { get; set; }
        public int Pedido { get; set; }
        public string DocDate { get; set; }
        public string CardName { get; set; }
        public string Vendedor { get; set; }
        public string TipoPedido { get; set; }
        public string Credito { get; set; }
        public string TrackNo { get; set; }
        public string Status { get; set; }
        public string NoFolio { get; set; }
        public string TipoDoc { get; set; }
        public string TipoCliente { get; set; }
        public int NoCajas { get; set; }
        
        /*Parametros del SP*/
        public string  pFecIni { get; set; }
        public string  pFecFin { get; set; }
    }
}
