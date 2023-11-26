using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.BardCode
{
    public class BardCode : BusinessObject<int>
    {
        public string ItemCode { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
        public string CodeBars { get; set; }
        public string U_CBInner { get; set; }
        public string U_CBMaster { get; set; }
        public string Base64 { get; set; }
        public string Base64Inner { get; set; }
        public string Base64Master { get; set; }

        private List<BardCodeItem> _ListaItems = new List<BardCodeItem>();
        public List<BardCodeItem> ListaItems
        {
            get
            {
                return this._ListaItems;
            }
            set
            {
                this._ListaItems = value ?? throw new InvalidOperationException("La colección de articulos no puede ser un valor nulo.");
            }
        }
        
        public void AddItemImagen(BardCodeItem item)
        {
            this.ListaItems.Add(item);
        }
    }
}
