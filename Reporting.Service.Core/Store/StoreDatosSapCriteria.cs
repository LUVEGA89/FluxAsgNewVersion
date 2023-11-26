using WikiCore.Data;


namespace Reporting.Service.Core.Store
{
    public class StoreDatosSapCriteria : Criteria
    {
        public string CodigoTienda { get; set; }

        public int? Tipo { get; set; }

    }
}