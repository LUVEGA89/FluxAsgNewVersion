using Reporting.Service.Core.Auditoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class CapturaModel
    {
        public int Tipo { get; set; }
        public int Tienda { get; set; }
        public IList<SeguimientoAuditoria> Auditorias { get; set; }
        public IList<string> Fechas { get; set; }
    }
}