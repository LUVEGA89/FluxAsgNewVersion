using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Evidencia
{
    public class Evidencia : BusinessObject<int>
    {

        public int FolioSIE { get; set; }

        public Modulo.Modulo Modulo { get; set; }

        public DateTime RegistradoEl { get; set; }

        public string RegistradoPor { get; set; }

        public string FileByte { get; set; }

        public string FileName { get; set; }

        public string Extension { get; set; }

        public bool Activo { get; set; }

        public EvidenciaKind TipoEvidencia { get; set; }

    }
}
