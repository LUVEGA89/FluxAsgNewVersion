using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Actividad
{
    public class Comentario : BusinessObject<int>
    {

        public DateTime RegistradorEl { get; set; }

        public string ReigstradoPor { get; set; }

        public string Nombre { get; set; }

        public bool Estatus { get; set; }

        public int NodoPadre { get; set; }


        private List<Respuesta> _ListaRepuestas = new List<Respuesta>();

        public List<Respuesta> ListaRepuestas
        {
            get
            {
                return this._ListaRepuestas;
            }
            set
            {
                this._ListaRepuestas = value ?? throw new InvalidOperationException("La colección de canales no puede ser un valor nulo.");
            }
        }

        public void AddItem(Respuesta item)
        {
            this.ListaRepuestas.Add(item);
        }      
    }
}
