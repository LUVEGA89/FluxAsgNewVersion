using System;
using WikiCore.Data;

namespace Reporting.Service.Core.Mobile.Pedidos
{
    public class PedidoCriteria : Criteria
    {
        public DateTime Del { get; set; }
        public DateTime Al { get; set; }
    }
}