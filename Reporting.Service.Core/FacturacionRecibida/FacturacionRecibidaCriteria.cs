using System;
using WikiCore.Data;

namespace Reporting.Service.Core.FacturacionRecibida
{
    public class FacturacionRecibidaCriteria : Criteria
    {
        public DateTime? Del { get; set; }
        public DateTime? Al { get; set; }
        public string RfcReceptor { get; set; }
        public FacturacionKind Tipo { get; set; }
        public TipoComprobanteKind TipoComprobante { get; set; }
    }
}