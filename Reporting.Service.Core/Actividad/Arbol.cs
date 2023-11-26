using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Actividad
{
    public class Arbol
    {
        public string text { get; set; }

        //  public Arbol Nodes { get; set; }



        private List<Arbol> _Nodes = new List<Arbol>();

        public List<Arbol> nodes
        {
            get
            {
                return this._Nodes;
            }
            set
            {
                this._Nodes = value;// ?? throw new InvalidOperationException("La colección de nodos no puede ser un valor nulo.");
            }
        }
        public int id { get; set; }
        public string icon { get; set; }

        public string selectedIcon { get; set; }

        public string color { get; set; }

        public string backColor { get; set; }

        public string href { get; set; }

        public string collapseIcon { get; set;}
        public string expandIcon { get; set; }

        public string iconColor { get; set; }

        public bool expanded { get; set; }

        public string state { get; set; }



        //color: "#000000",
        //backColor: "#FFFFFF",

        public void AddNode(Arbol item)
        {
            this.nodes.Add(item);
        }
    }
}
