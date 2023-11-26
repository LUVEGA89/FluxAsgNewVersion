using Reporting.Service.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Web.UI.Models
{
    public class ProductosModel
    {
        public IList<Core.ActividadesProductos.MarcaProductos> marcaProductos {get;set;}
        public IList<Core.ActividadesProductos.TitulosActividad> titulosActividads { get; set; }
        public IList<Core.ActividadesProductos.CategoriasSKUProductos> CategoriasSKU { get; set; }
        public IList<Rol> Roles { get; set; }
    }
}
