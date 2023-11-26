using Reporting.Service.Core.MenuByUser.Modulo;
using Reporting.Service.Core.MenuByUser;
using Reporting.Service.Core.MenuByUser.Vistas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reporting.Service.Web.UI.Models
{
    public class MenuByUser
    {
        public List<Modulo> modulos { get; set; }
        public List<UserAccess> usuarios { get; set; }
        public List<Vista> vistas { get; set; }

        public List<Modulo> modulosUser { get; set; }
    }
}