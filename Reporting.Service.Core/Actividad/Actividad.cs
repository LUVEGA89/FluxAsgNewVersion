using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Actividad
{
    public class Actividad : BusinessObject<int>
    {
        
        public string Nombre { get; set; }
        public int Estatus { get; set; }
        public DateTime RegistradoEl { get; set; }
        public string RegistradoPor { get; set; }
        public DateTime ActualizadoEl { get; set; }
        public string ActualizadoPor { get; set; }
        //agregado Jimeru
        public string Comentario { get; set; }

        private List<Comentario> _ListaComentarios = new List<Comentario>();

        public List<Comentario> ListaComentarios
        {
            get
            {
                return this._ListaComentarios;
            }
            set
            {
                this._ListaComentarios = value ?? throw new InvalidOperationException("La colección de comentarios no puede ser un valor nulo.");
            }
        }

        public void AddItem(Comentario item)
        {
            this.ListaComentarios.Add(item);
        }

        private List<ActividadFoto> _ListaImagenes = new List<ActividadFoto>();
        public List<ActividadFoto> ListaImagenes
        {
            get
            {
                return this._ListaImagenes;
            }
            set
            {
                this._ListaImagenes = value ?? throw new InvalidOperationException("La colección de imagenes no puede ser un valor nulo.");
            }
        }

        public DateTime? ProximaActividad { get;  set; }
        public string Cliente { get;  set; }
        public DateTime? Fecha { get; set; }

        public void AddItemImagen(ActividadFoto item)
        {
            this.ListaImagenes.Add(item);
        }

    }
}
