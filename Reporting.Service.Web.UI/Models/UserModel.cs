using Reporting.Service.Core.Common;
using Reporting.Service.Core.MenuByUser;
using Reporting.Service.Core.MenuByUser.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class UserModel
    {
        public bool IsAdmin { get; set; }
        public string Nombre { get; set; }
        public string CodigoEmpleado { get; set; }
        public string Area { get; set; }
        public IList<Rol> Roles { get; set; }
        public Menu Menu { get; set; }
    }
}