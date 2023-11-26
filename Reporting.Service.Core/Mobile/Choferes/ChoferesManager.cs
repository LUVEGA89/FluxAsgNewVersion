using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Mobile.Choferes
{
    public class ChoferesManager : Catalog<Chofer, string, ChoferesCriteria>
    {
        protected override string FindPagedItemsProcedure => "mobile.prFindChoferes";

        protected override Chofer LoadItem(IDataReader dr)
        {
            return new Chofer
            {
                Identifier = (string)dr["Sequence"],
                Nombre = (string)dr["Nombre"]
            };
        }

        protected override void BeforeAddExecuted(DataContext<Chofer, string, ChoferesCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }

        protected override DbCommand PrepareAddStatement(Chofer item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prAddChofer");

            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, item.Nombre);

            return cmd;
        }

        protected override void CommandAddException(DataContext<Chofer, string, ChoferesCriteria> context, Exception ex)
        {
            if (context.Transaction != null)
            {
                DbConnection connection = context.Transaction.Connection;
                context.Transaction.Rollback();

                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            base.CommandAddException(context, ex);
        }

        protected override void CommandAddComplete(DataContext<Chofer, string, ChoferesCriteria> context)
        {
            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }

        protected override DbCommand PrepareDeleteStatement(string id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(string id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prGetChofer");

            this.Database.AddInParameter(cmd, "@Id", DbType.String, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Chofer item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(ChoferesCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
        
            return cmd;
        }
    }
}
