using WikiCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Reporting.Service.Core.Store
{
    public class StoreCatalog : Catalog<Store, int, StoreCriteria>
    {
        private StoreDatosSapCatalog catalog = new StoreDatosSapCatalog();

        public StoreCatalog()
            :base("SIATADMINConnection")
        {
        }

        public StoreCatalog(string database)
            : base(database)
        { }

        protected override string FindPagedItemsProcedure
        {
            get{ return "prFindStores"; }
        }

        protected override Store LoadItem(IDataReader dr)
        {
            Store _store = new Store();

            _store.Identifier = (int)dr["Sequence"];
            _store.Nombre = (string)dr["Nombre"];
            _store.Tipo = (int)dr["Tipo"];
            _store.CambiosPendientes = (bool)dr["CambiosPendientes"];
            _store.Region = dr["Region"] == DBNull.Value ? 0 : (int)dr["Region"];
            _store.NombreTiendaSAP = (string)dr["NombreTiendaSAP"];
            _store.CodigoSAP = dr["CodigoSAP"] == DBNull.Value ? "" : (string)dr["CodigoSAP"];
            _store.Origen = (string)dr["Origen"];
            _store.CodigoSucursalSAP = (int)dr["CodigoSucursalSAP"];

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    _store.AddDatosSap(this.catalog.Find((int)dr["Sequence"]));
                }
            }

            return _store;
        }

        protected override DbCommand PrepareAddStatement(Store item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetStore");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(StoreCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            if(!string.IsNullOrEmpty(criteria.Codigo))
                this.Database.AddInParameter(cmd, "@Codigo", DbType.String, criteria.Codigo);

            if(criteria.Tipo.HasValue)
                this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, criteria.Tipo);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Store item)
        {
            throw new NotImplementedException();
        }
    }
}
