using Reporting.Service.Core.Common;
using Reporting.Service.Core.CompletarFacturacionRecibida.SucursalesSAP;
using Reporting.Service.Core.Trafico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class EmisoresModel
    {
        public List<string> Emisores { get; set; }
        public SucursalesSAP[] SucursalesSAP { get; set; }
    }
}