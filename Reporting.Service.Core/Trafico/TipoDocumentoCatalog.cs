using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico
{
    public class TipoDocumentoCatalog : Catalog<TipoDocumento, int, TipoDocumentoCriteria>
    {
        public TipoDocumentoCatalog()
        {
        }

        public TipoDocumentoCatalog(string database)
            : base(database)
        {
        }

        protected override string FindPagedItemsProcedure
        {
            get { return "prFindTiposDocumento"; }
        }

        protected override TipoDocumento LoadItem(IDataReader dr)
        {
            TipoDocumento TD = new TipoDocumento();
            //return new TipoDocumento()
            //{
            TD.Identifier = (int)dr["Sequence"];
            TD.Nombre = (string)dr["Nombre"];
            TD.Descripcion = (string)dr["Descripcion"];
            TD.Extencion = (string)dr["Extencion"];
            TD.Estatus = (TipoDocumentKind)(int)dr["Estatus"];
            TD.Obligatorio = (bool)dr["Obligatorio"];
            TD.Requerido = (string)dr["Requerido"];
            //};

            return TD;
        }

        protected override DbCommand PrepareAddStatement(TipoDocumento item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTipoDocumento");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int16, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(TipoDocumento item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(TipoDocumentoCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            this.Database.AddInParameter(cmd, "@Estatus", DbType.Int32, criteria.Estatus);

            return cmd;
        }
    }
}
