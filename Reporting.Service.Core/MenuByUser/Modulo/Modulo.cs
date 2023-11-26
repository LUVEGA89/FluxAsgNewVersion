using Reporting.Service.Core.MenuByUser.Vistas;
using System.Collections.Generic;
using WikiCore;

namespace Reporting.Service.Core.MenuByUser.Modulo
{
    public class Modulo : BusinessObject<int>
    {
        public List<Modulo> submodulos = new List<Modulo>();
        public string Nombre { get; set; }
        public string Icon { get; set; }
        public bool Activo { get; set; }
        public List<Vista> vistas { get; set; }
        public int? Padre { get; set; }
        public Modulo[] modulos
        {
            get
            {
                return this.submodulos.ToArray();
            }
            set
            {
                this.submodulos.Clear();
                this.submodulos.AddRange(value);
            }
        }
        public void AddSubmodulos(params Modulo[] modulos)
        {
            this.submodulos.AddRange(modulos);
        }
    }
}