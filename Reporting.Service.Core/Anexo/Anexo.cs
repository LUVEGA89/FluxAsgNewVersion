using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reporting.Service.Core.Anexo.Sucursal;
using WikiCore;

namespace Reporting.Service.Core.Anexo
{
    public class Anexo : BusinessObject<int>
    {
        public bool Activo { get; set; }

        public DateTime RegistradoEl { get; set; }

        public Modulo.Modulo Modulo { get; set; }

        public Sucursal.Sucursal Sucursal { get; set; }

        public Empresa.Empresa Empresa { get; set; }

        public int FolioSIE { get; set; }

        public int FolioSAP { get; set; }

        public string ErrorCodeSAP { get; set; }
    }
}
