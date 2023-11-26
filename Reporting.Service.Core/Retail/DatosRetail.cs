using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Retail
{
    public class DatosRetail
    {
        public decimal MontoFacturado { get; set; }

        public decimal NCAplicadasFacturasPeriodo { get; set; }

        public decimal NCAplicadasFacturasOtroPeriodo { get; set; }

        public decimal VentaTotal { get; set; }

        public decimal Utilidad { get; set; }

        public decimal VentaReal
        {
            get
            {
                return this.MontoFacturado - this.NCAplicadasFacturasPeriodo;
            }
        }

        private List<Detalles> _items = new List<Detalles>();

        public List<Detalles> Items
        {
            get
            {
                return this._items;
            }
            set
            {
                this._items = value ?? throw new InvalidOperationException("La colección de items no puede ser un valor nulo."); ;
            }
        }

        public void AddItem(Detalles item)
        {
            this._items.Add(item);
        }
    }
}
