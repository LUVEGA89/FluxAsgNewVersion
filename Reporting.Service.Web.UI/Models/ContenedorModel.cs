using Reporting.Service.Core.Trafico.Contenedor;
using Reporting.Service.Core.Trafico.Contenedor.Naviera;
using Reporting.Service.Core.Trafico.Contenedor.StatusContenedor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WikiCore;

namespace Reporting.Service.Web.UI.Models
{
    public class ContenedorModel
    {
        //Para el contenedor
        public List<Naviera> navieras { get; set; }

        public List<StatusContenedor> status { get; set; }

        //Para los envíos de contenedores
        public List<Contenedor> contenedores { get; set; }
    }

    public class ContenedorModelCheckBox 
    {
        public List<ContenedorCheckBox> contenedores { get; set; }
    }
}