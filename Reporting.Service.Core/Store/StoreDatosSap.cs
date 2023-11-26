using WikiCore;

namespace Reporting.Service.Core.Store
{
    public class StoreDatosSap : BusinessObject<int>
    {
        public string Nombre { get; set; }
        public StoreDatosSapKind Tipo { get; set; }
        public string Codigo { get; set; }
        public string CodigoSucursal { get; set; }
        public string Almacen { get; set; }
    }
}