using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.CompletarFacturacionRecibida.Factory
{
    public interface ICreadorDB
    {
        string GetCodigoImpuesto(string CodigoSAT, decimal TasaoCuota, decimal LimiteInferior = 0.0m, decimal LimiteSuperior = 0.0m);
        SAPbobsCOM.Company CreateOcompany();
        FactoryBaseKind AsignarTipo();
        FacturaCompra ValidaConceptos(FacturaCompra factura);
        Documents FillPurchaseInvoice(Documents oPurchase, FacturaCompra factura, ICreadorDB DbActual, string Referencia);
    }
}
