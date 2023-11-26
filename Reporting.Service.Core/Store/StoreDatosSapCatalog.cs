using System;
using System.Data;
using System.Data.Common;
using WikiCore.Data;

namespace Reporting.Service.Core.Store
{
    public class StoreDatosSapCatalog : Catalog<StoreDatosSap, int, StoreDatosSapCriteria>
    {
        public StoreDatosSapCatalog()
            :base("SIATADMINConnection")
        {
        }

        protected override string FindPagedItemsProcedure
        {
            get
            {
                return "prFindStoreDatosSap";
            }
        }

        protected override StoreDatosSap LoadItem(IDataReader dr)
        {
            StoreDatosSap x = new StoreDatosSap();

            x.Identifier = (int)dr["Sequence"];
            x.Nombre = (string)dr["Nombre"];
            x.Codigo = (string)dr["Codigo"];
            x.CodigoSucursal = (string)dr["CodigoSucursal"];
            x.Almacen = (string)dr["Almacen"];
            x.Tipo = (StoreDatosSapKind)(int)dr["Tipo"];

            return x;
        }

        protected override DbCommand PrepareAddStatement(StoreDatosSap item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetStoreDatosSap");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(StoreDatosSap item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(StoreDatosSapCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            if (!string.IsNullOrEmpty(criteria.CodigoTienda))
                this.Database.AddInParameter(cmd, "@Tienda", DbType.String, criteria.CodigoTienda);

            if (criteria.Tipo.HasValue)
                this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, criteria.Tipo);

            return cmd;
        }
    }
}