using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;
namespace Reporting.Service.Core.ActividadesProductos
{
    public class NuevoSKUSugerido:BusinessObject<int>
    {
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string RegistradoPor { get; set; }
        public string Marca { get; set; }
        public string Categoria { get; set; }
        public string Subcategoria { get; set; }
        public string Coincidencia { get; set; }
        public bool Estatus { get; set; }
        public string Auxiliar { get; set; }
        public string Empaque { get; set; }

        private List<PreciosCompetencia> _ListaPreciosCompetencias = new List<PreciosCompetencia>();
        public List<PreciosCompetencia> ListaPreciosCompetencias
        {
            get
            {
                return this._ListaPreciosCompetencias;
            }
            set
            {
                this._ListaPreciosCompetencias = value ?? throw new InvalidOperationException("La colección de precios de competencia no puede ser un valor nulo.");
            }
        }


        private List<ImagenesNuevoSKU> _ListaImagenes = new List<ImagenesNuevoSKU>();
        public List<ImagenesNuevoSKU> ListaImagenes
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
        public void AddItemImagen(ImagenesNuevoSKU item)
        {
            this.ListaImagenes.Add(item);
        }
    }
}
