using Reporting.Service.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Trafico
{
    public class TipoDocumento : BusinessObject<int>
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Extencion { get; set; }
        public TipoDocumentKind Estatus { get; set; }
        public bool Obligatorio { get; set; }
        public string Requerido { get; set; }
    }
}
