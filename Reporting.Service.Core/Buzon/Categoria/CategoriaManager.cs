using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Buzon.Categoria
{
    public class CategoriaManager : Catalog<Categoria, int, CategoriaCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindBuzonCategoria";

        protected override Categoria LoadItem(IDataReader dr)
        {
            Categoria item = new Categoria();

            item.Identifier = (int)dr["Identificador"];
            item.Nombre = (string)dr["Nombre"];
            item.Estatus = (bool)dr["Estatus"];

            return item;    
        }

        protected override DbCommand PrepareAddStatement(Categoria item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(CategoriaCriteria Criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(Criteria);
            return cmd;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetBuzonCategoria");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);            
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Categoria item)
        {
            throw new NotImplementedException();
        }
    }
}
