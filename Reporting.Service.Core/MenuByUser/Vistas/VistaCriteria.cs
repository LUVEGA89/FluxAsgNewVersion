using WikiCore.Data;

namespace Reporting.Service.Core.MenuByUser.Vistas
{
    public class VistaCriteria : Criteria
    {
        public VistaCriteria()
        {
            Tipo = Modulo.ModuloKind.User;
        }
        //public int IsAdmin { get; set; }
        public string UuidUser { get; set; }
        public int IdModulo { get; set; }

        public Modulo.ModuloKind Tipo { get; set; }
    }
}