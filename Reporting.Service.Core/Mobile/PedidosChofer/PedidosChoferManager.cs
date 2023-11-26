using Reporting.Service.Core.Mobile.Pedidos.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Mobile.PedidosChofer
{
    public class PedidosChoferManager : Catalog<PedidoChofer, int, PedidoChoferCriteria>
    {
        protected override string FindPagedItemsProcedure => "mobile.prFindPedidosChofer";

        protected override PedidoChofer LoadItem(IDataReader dr)
        {
            PedidoItemsManager Items = new PedidoItemsManager((int)dr["DocEntry"]);

            return new PedidoChofer
            {
                Identifier = (int)dr["DocNum"],
                CardCode = (string)dr["CardCode"],
                CardName = (string)dr["CardName"],
                Address2 = (string)dr["Address2"],
                Cajas = (int)dr["cajas"],
                Fletera = (string)dr["Fletera"],
                Ubicacion = (string)dr["Ubicacion"],
                Items = Items.FindPagedItems(new PedidoItemsCriteria { Pedido = (int)dr["DocEntry"] }).ToList()
            };
        }

        protected override DbCommand PrepareAddStatement(PedidoChofer item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prGetPedidoChofer");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(PedidoChofer item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(PedidoChoferCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            this.Database.AddInParameter(cmd, "@Chofer", DbType.String, criteria.Chofer);
            if(criteria.Ruta != 0)
                this.Database.AddInParameter(cmd, "@Ruta", DbType.Int32, criteria.Ruta);

            return cmd;
        }
    }
}
