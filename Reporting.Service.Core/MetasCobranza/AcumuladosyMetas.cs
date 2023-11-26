using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.MetasCobranza
{
    public class AcumuladosyMetas
    {
        public decimal Acumulado { get; set; }
        public decimal Meta { get; set; }
        // 1/10/2018
        public decimal AcumuladoMayoreo { get; set; }
        public decimal AcumuladoRetail { get; set; }
        public decimal AcumuladoInternet { get; set; }

        private List<Canal> canales = new List<Canal>();

        public List<Canal> Canales
        {
            get
            {
                return this.canales;
            }
            set
            {
                this.canales = value ?? throw new InvalidOperationException("La colección de canales no puede ser un valor nulo.");
            }
        }

        public void AddCanal(Canal canal)
        {
            this.canales.Add(canal);
        }
        // metas canales
        private List<Canal> canalesMetas = new List<Canal>();

        public List<Canal> MetasExistentes
        {
            get
            {
                return this.canalesMetas;
            }
            set
            {
                this.canalesMetas = value ?? throw new InvalidOperationException("La colección de canales no puede ser un valor nulo.");
            }
        }

        public void AddMetaExistente(Canal canal)
        {
            this.canalesMetas.Add(canal);
        }


    }
}
