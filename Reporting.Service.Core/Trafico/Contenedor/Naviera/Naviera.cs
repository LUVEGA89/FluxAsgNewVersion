using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Trafico.Contenedor.Naviera
{
    public class Naviera: BusinessObject<string>
    {
        public string nombreNaviera { get; set; }

        public string contacto { get; set; }

        public string email { get; set; }

    }
}
