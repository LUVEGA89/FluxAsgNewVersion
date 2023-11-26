using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Evidencia
{
    public class EvidenciaCriteria : Criteria
    {
        public int FolioSIE { get; set; }

        public int Modulo { get; set; }

    }
}
