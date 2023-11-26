using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Anexo.Sucursal
{
    public class Sucursal : BusinessObject<int>
    {
        public string Nombre { get; set; }

        public bool Activo { get; set; }

    }
}
