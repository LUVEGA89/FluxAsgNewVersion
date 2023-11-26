using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Mobile.Pedidos.Items
{
    public class PedidoItemsManager : Catalog<PedidoItems, int, PedidoItemsCriteria>
    {
        private int _docEntry;
        public PedidoItemsManager(int DocEntry)
        {
            this._docEntry = DocEntry;
        }

        protected override string FindPagedItemsProcedure => "mobile.prFindPedidoItems";

        protected override PedidoItems LoadItem(IDataReader dr)
        {
            return new PedidoItems
            {
                Identifier = (int)dr["LineNum"],
                ItemCode = (string)dr["ItemCode"],
                Quantity = (int)(decimal)dr["Quantity"]
            };
        }

        protected override DbCommand PrepareAddStatement(PedidoItems item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prGetPedidoItems");

            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, _docEntry);
            this.Database.AddInParameter(cmd, "@LineNum", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(PedidoItems item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(PedidoItemsCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            this.Database.AddInParameter(cmd, "@Pedido", DbType.Int32, criteria.Pedido);

            return cmd;
        }
    }
}
