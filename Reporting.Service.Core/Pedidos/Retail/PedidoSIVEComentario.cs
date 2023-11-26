using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Pedidos.Retail
{
    public class PedidoSIVEComentario : BusinessObject<int>
    {
        public string RegistradoPor { get; set; }

        public int PedidoSIVE { get; set; }        

        public DateTime RegistradoEl { get; set; }

        public string Accion { get; set; }

        public string Comentario { get; set; }

        public bool Estatus { get; set; }

    }
}
