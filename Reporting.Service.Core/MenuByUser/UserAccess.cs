using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.MenuByUser
{
    public class UserAccess
    {
        public string IdUser { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public int View { get; set; }

        public bool Activo { get; set; }
    }
}
