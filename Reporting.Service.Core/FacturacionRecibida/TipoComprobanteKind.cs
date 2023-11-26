using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.FacturacionRecibida
{
    public enum TipoComprobanteKind
    {
        ///
        /// El Comprobante es de tipo Factura I
        ///
        Invoice = 1,
        ///
        /// El Comprobante es de tipo Nota de Crédito E
        ///
        CreditNote = 2,
        ///
        /// El Comprobante es de tipo Complemento de Pago P
        ///
        PaymentSupplement = 5
    }
}
