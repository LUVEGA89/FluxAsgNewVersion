using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Precio
{
    public class TipoPrecioManager : Catalog<TipoPrecio, int, TipoPrecioCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindTipoPrecio";

        protected override TipoPrecio LoadItem(IDataReader dr)
        {
            TipoPrecio nueva = new TipoPrecio();

            nueva.Identifier = (int)dr["IDTipoPrecioArt"];
            nueva.Descripcion = (string)dr["Descripcion"];

            return nueva;
        }

        protected override DbCommand PrepareAddStatement(TipoPrecio item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTipoPrecio");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);
            return cmd;
        }

        

        protected override DbCommand PrepareUpdateStatement(TipoPrecio item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prUpdateTipoPrecio");
            this.Database.AddInParameter(command, "@Familia", DbType.String, item.SequenceFamilia);
            this.Database.AddInParameter(command, "@TipoPrecioArt", DbType.String, item.TipArt);
            return command;
          

        }
        public  Boolean UpdateOfertaSku(string Sku)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prUpdateTipoPrecioOfertaSku");
            this.Database.AddInParameter(command, "@sku", DbType.String, Sku);
            IDataReader dr = this.Database.ExecuteReader(command);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;

        }

        protected override DbCommand PrepareFindPagedItemsStatement(TipoPrecioCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            
            this.Database.AddInParameter(cmd, "@Estatus", DbType.Int32, criteria.Estatus);
            
            return cmd;
        }




    }
}
