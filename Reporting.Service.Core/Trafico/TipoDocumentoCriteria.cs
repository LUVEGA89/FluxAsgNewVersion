using WikiCore.Data;

namespace Reporting.Service.Core.Trafico
{
    public class TipoDocumentoCriteria : Criteria
    {
        public TipoDocumentoCriteria()
        {
            this.Estatus = 1;
        }

        public int Estatus { get; set; }
        public string Nombre { get; set; }
    }
}