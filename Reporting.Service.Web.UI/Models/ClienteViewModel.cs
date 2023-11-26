using Reporting.Service.Core.Cliente;
using Reporting.Service.Core.Disclaimers;
using Reporting.Service.Core.ProveedorServicios;
using Reporting.Service.Core.Servicio;
using Reporting.Service.Core.TipoServicio;
using Reporting.Service.Core.Usuarios;
using System.Collections.Generic;

namespace Reporting.Service.Web.UI.Models
{
    public class ClienteViewModel
    {
        public List<Cliente> clientes { get; set; }
        public List<Servicio> servicios { get; set; }
        public List<TipoServicio> tipoServicios { get; set; }
        public List<Disclaimer> Disclaimers { get; set; }
        public List<ProveedorServicio> proveedorServicios { get; set; }
    }
}