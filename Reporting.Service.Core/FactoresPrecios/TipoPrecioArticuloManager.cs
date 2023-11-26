using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.FactoresPrecios
{
    public class TipoPrecioArticuloManager : Catalog<TipoPrecioArticulo, int, TipoPrecioCanalCriteria>
    {
        public TipoPrecioArticuloManager()
           : base()
        {

        }
        
        protected override string FindPagedItemsProcedure => "prFindTipoPrecioArticulo";

        protected override TipoPrecioArticulo LoadItem(IDataReader dr)
        {
            TipoPrecioArticulo item = new TipoPrecioArticulo();
            item.Identifier = (int)dr["Identificador"];
            item.Descripcion = (string)dr["Descripcion"];
            
            return item;
        }

        protected override DbCommand PrepareAddStatement(TipoPrecioArticulo item)
        {
            DbCommand comand = this.Database.GetStoredProcCommand("prAddTipoPrecioArticulo");
            this.Database.AddInParameter(comand, "@Descripcion", DbType.String, item.Descripcion);
            return comand;
        }

       
        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand comand = this.Database.GetStoredProcCommand("prGetTipoPrecioArticulo");
            this.Database.AddInParameter(comand, "@Id", DbType.Int16, id);
            return comand;
        }

        protected override DbCommand PrepareUpdateStatement(TipoPrecioArticulo item)
        {
            throw new NotImplementedException();
        }

       
    }
}
