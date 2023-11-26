using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Anexo
{
    public class AnexoCriteria : Criteria
    {

        public int FolioSIE { get; set; }

        public int Modulo { get; set; }

        public int Sucursal { get; set; }

        public int FolioSAP { get; set; }

        public int Empresa { get; set; }
    }
}
