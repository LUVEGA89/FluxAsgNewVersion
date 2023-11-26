using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Trafico.Contenedor
{
    public class Contenedor: BusinessObject<int>
    {
        public string nomContenedor { get; set; }

        public DateTime fechaCrea { get; set; }

        public Naviera.Naviera naviera { get; set; }

        public int statusContenedor_id { get; set; }

        public int seguimiento_id { get; set; }

        public decimal flete { get; set; }
        public decimal tipoCambio { get; set; }

        public string usuario { get; set; }
        public string LlegadaPuerto { get; set; }
        public string LibresAlmacenaje { get; set; }
        public string LibresDemoras { get; set; }
        public string FleteMarino { get; set; }
        
    }

    public class ContenedorCheckBox : BusinessObject<int>
    {
        public string nomContenedor { get; set; }
    }
}
