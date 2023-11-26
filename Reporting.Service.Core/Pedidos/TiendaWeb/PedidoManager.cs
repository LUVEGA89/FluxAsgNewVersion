using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Pedidos.TiendaWeb
{
    public class PedidoManager : Catalog<Pedido, int, PedidoCriteria>
    {
        public PedidoManager()
        : base()
        {

        }

        public PedidoManager(string database)
        :base(database)
        {

        }

        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override Pedido LoadItem(IDataReader dr)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(Pedido item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddPedido");
            this.Database.AddInParameter(cmd, "@tienda", DbType.String, item.tienda);
            this.Database.AddInParameter(cmd, "@DocDate", DbType.Date, item.fecha);
            this.Database.AddInParameter(cmd, "@totalPedido", DbType.Decimal, item.totalPedido);
            this.Database.AddInParameter(cmd, "@folioSAP", DbType.String, item.folioSAP);
            this.Database.AddInParameter(cmd, "@usuario", DbType.String, item.usuario);
            return cmd;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(Pedido item)
        {
            throw new NotImplementedException();
        }
    }
}
