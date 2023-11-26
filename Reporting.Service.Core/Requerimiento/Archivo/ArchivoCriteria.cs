using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Requerimiento.Archivo
{
    public class ArchivoCriteria: Criteria
    {
        public int FolioGeneral { get; set; }

        public EstatusKind Estatus { get; set; }
    }
}
