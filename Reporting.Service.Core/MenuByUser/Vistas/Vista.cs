using WikiCore;

namespace Reporting.Service.Core.MenuByUser.Vistas
{
    public class Vista : BusinessObject<int>
    {
        public string Nombre { get; set; }
        public int Modulo { get; set; }
        public string Controller { get; set; }
        public string View { get; set; }
        public string Icon { get; set; }
        public bool Activo { get; set; }
        public string IdUsuario { get; set; }
        public string Email { get; set; }
        public bool VistaUserActivo { get; set; }
        public string ModuloName { get; set; }
    }
}