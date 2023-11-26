using Reporting.Service.Core.Viaticos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class ViaticosModel
    {
        public IList<DetalleSolicitud> DetalleSolicitud { get; set; }
        public IList<DetalleActividad> DetalleActividad { get; set; }
    }
}