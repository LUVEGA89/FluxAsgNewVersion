using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Pedidos.Retail
{
    public class PedidoSIVE : BusinessObject<int>
    {


        public Cliente Cliente { get; set; }


        public int PedidosSIVE { get; set; }

        public DateTime RegistradoEl { get; set; }

        public QuoteStatus EstadoSIVE { get; set; }


        public PedidoSIVEKind EstadoSIE { get; set; }

        public decimal SubTotal { get; set; }

        public decimal Impuestos { get; set; }

        public decimal Total { get; set; }

        public bool Estatus { get; set; }


        public Core.Evidencia.Evidencia Evidencia { get; set; }


        /// <summary>
        /// Lista de Articulos del pedido
        /// </summary>
        private List<PedidoItem> _ItemsPedido = new List<PedidoItem>();

        public List<PedidoItem> ItemsPedido
        {
            get
            {
                return this._ItemsPedido;
            }
            set
            {
                this._ItemsPedido = value ?? throw new InvalidOperationException("La colección de productos no puede ser un valor nulo.");
            }
        }

        public void AddItemPedido(PedidoItem item)
        {
            this._ItemsPedido.Add(item);
        }


        /// <summary>
        /// Lista comentarios del pedido
        /// </summary>
        private List<PedidoSIVEComentario> _Items = new List<PedidoSIVEComentario>();

        public List<PedidoSIVEComentario> Items
        {
            get
            {
                return this._Items;
            }
            set
            {
                this._Items = value ?? throw new InvalidOperationException("La colección de comentarios no puede ser un valor nulo.");
            }
        }


        public void AddItemComentario(PedidoSIVEComentario item)
        {
            this.Items.Add(item);
        }

        public PedidoSIVEComentario PedidoComentario { get; set; }
        
    }
}
