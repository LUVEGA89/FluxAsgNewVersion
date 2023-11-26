using WikiCore.Data;

namespace Reporting.Service.Core.Proveedores
{
    public class ProveedorCriteria : Criteria
    {
        public StatusPedido Status { get; set; }
        public string Empresa { get; set; }
        public int Tipo { get; set; }
        public EstatusPagos EstatusPagos { get; set; }
    }
}