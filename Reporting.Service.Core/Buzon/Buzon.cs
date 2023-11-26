using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Buzon
{
    public class Buzon: BusinessObject<int>
    {
        
        public string Nombre { get; set; }

        public string Sugerencia { get; set; }

        public BuzonKind Tipo { get; set; }

        public DateTime RegistradoEl { get; set; }

        public Core.Buzon.Categoria.Categoria Categoria { get; set; }

        public Core.Buzon.Area.Area Area { get; set; }

        public string Sucursal { get; set; }

        public bool Estatus { get; set; }

    }
}
