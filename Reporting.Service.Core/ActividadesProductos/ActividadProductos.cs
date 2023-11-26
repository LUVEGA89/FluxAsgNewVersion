using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.ActividadesProductos
{
    public class ActividadProductos : BusinessObject<int>
    {
        public int Actividad { get; set; }
        public string Titulo { get; set; }
        public string Producto { get; set; }
        public string Comentario { get; set; }
        public DateTime? RegistradoEl { get; set; }
        public string RegistradoPor { get; set; }
        public int Estatus { get; set; }
        public int PermisosRol {get;set;}

        //rellena la lista de comentarios
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

        //rellena la lista de imagenes
        private List<ActividadProductosFotos> _ListaImagenes = new List<ActividadProductosFotos>();
        public List<ActividadProductosFotos> ListaImagenes
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
        public void AddItemImagen(ActividadProductosFotos item)
        {
            this.ListaImagenes.Add(item);
        }
        //rellena la lista de comparacion de precios
        private List<ActividadProductosComparacion> _ListaComparacion = new List<ActividadProductosComparacion>();
        public List<ActividadProductosComparacion> ListaComparacion
        {
            get
            {
                return this._ListaComparacion;
            }
            set
            {
                this._ListaComparacion = value ?? throw new InvalidOperationException("La colección de imagenes no puede ser un valor nulo.");
            }
        }
        public void AddItemComparacion(ActividadProductosComparacion item)
        {
            this.ListaComparacion.Add(item);
        }
    }
}
