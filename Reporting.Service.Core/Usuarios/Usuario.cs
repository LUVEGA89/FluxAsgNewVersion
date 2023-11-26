using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Usuarios
{
    public class Usuario : BusinessObject<string>
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Password { get; set; }
        public string IdEmpresa { get; set; }
        public string Empresa { get; set; }
        public string IdRol { get; set; }
        public string Rol { get; set; }
        public string FechaCreacion { get; set; }
        public string Estado { get; set; }
    }
}
