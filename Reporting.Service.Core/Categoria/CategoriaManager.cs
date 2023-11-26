using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Categoria
{
    public class CategoriaManager : Catalog<Categoria, int, CategoriaCriteria>
    {
        public CategoriaManager()
            : base()
        {

        }

        protected override string FindPagedItemsProcedure => "prFindCategorias";

        protected override Categoria LoadItem(IDataReader dr)
        {
            Categoria nueva = new Categoria();

            nueva.Identifier = (int)dr["Id"];
            nueva.nameCate = (string)dr["Name"];

            return nueva;
        }

        protected override DbCommand PrepareAddStatement(Categoria item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCategoria");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int16, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Categoria item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(CategoriaCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            if (criteria.parentId.HasValue)
            {
                this.Database.AddInParameter(cmd, "@parentId", DbType.Int16, criteria.parentId);
            }
            return cmd;
        }
    }
}
