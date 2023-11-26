using WikiCore.Data;

namespace Reporting.Service.Core.Requerimiento.Concepto
{
    public class ConceptoCritetria : Criteria
    {
        public EstatusKind? Estatus { get; set; }
        public int TipoRequerimiento { get; set; }
    }
}