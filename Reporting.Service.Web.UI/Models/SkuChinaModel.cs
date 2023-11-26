using Reporting.Service.Core.Categoria;
using Reporting.Service.Core.Clasificaciones;
using Reporting.Service.Core.Productos;
using Reporting.Service.Core.Tipos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class SkuChinaModel
    {

        public List<Familia> Familias { get; set; }
        public List<Categoria> Categorias { get; set; }
        public List<Clasificacion> Clasificaciones { get; set; }
        public List<Tipo> Tipos { get; set; }
        
    }
}