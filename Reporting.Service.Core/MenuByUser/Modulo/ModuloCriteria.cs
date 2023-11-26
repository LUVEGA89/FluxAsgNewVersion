using WikiCore.Data;

namespace Reporting.Service.Core.MenuByUser.Modulo
{
    public class ModuloCriteria: Criteria
    {
        public ModuloCriteria()
        {
            //this.IsAdmin = 0;//Para acceso de administrador
        }
        //public int IsAdmin { get; set; }

        public string UuidUser { get; set; }
        public int Padre { get; set; }


        public ModuloKind Tipo { get; set; }
        
    }
}