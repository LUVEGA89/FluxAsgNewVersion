using Reporting.Service.Core.Mobile.Pedidos.Items;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Mobile.Pedidos
{
    public class PedidosManager : Catalog<Pedido, int, PedidoCriteria>
    {
        protected override string FindPagedItemsProcedure => "mobile.prFindPedidos";

        protected override Pedido LoadItem(IDataReader dr)
        { 
            PedidoItemsManager Items = new PedidoItemsManager((int)dr["DocEntry"]);

            return new Pedido
            {
                Identifier = (int)dr["DocNum"],
                CardCode = (string)dr["CardCode"],
                CardName = (string)dr["CardName"],
                Address2 = (string)dr["Address2"],
                Cajas = (int)dr["cajas"],
                Fletera = (string)dr["Fletera"],
                Items = Items.FindPagedItems(new PedidoItemsCriteria { Pedido = (int)dr["DocEntry"] }).ToList()
            };
        }

        protected override DbCommand PrepareAddStatement(Pedido item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prGetPedido");

            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Pedido item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(PedidoCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, criteria.Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, criteria.Al);

            return cmd;
        }
    }
}
