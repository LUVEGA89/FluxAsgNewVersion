using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ActividadesProductos
{
    public class MarcaProductosCatalog : Catalog<MarcaProductos, int, MarcaProductosCriteria>
    {
        public MarcaProductosCatalog()
        {
        }
        public MarcaProductosCatalog(string database)
            : base(database)
        {
        }

        protected override string FindPagedItemsProcedure
        {
            get { return "prFindOpcionMarcaProducto"; }
        }

        protected override MarcaProductos LoadItem(IDataReader dr)
        {
            MarcaProductos marcaProducto = new MarcaProductos();
            marcaProducto.Identifier = (int)dr["Identificador"];
            marcaProducto.Marca = (string)dr["Marca"];
            marcaProducto.Estatus = (bool)dr["Estatus"];
            return marcaProducto;
        }

        protected override DbCommand PrepareAddStatement(MarcaProductos item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetOpcionMarcaProducto");
            this.Database.AddInParameter(cmd,"@Id",DbType.Int32,id);
            return cmd;
        }
        protected override DbCommand PrepareFindPagedItemsStatement(MarcaProductosCriteria Criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(Criteria);
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, Criteria.Id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(MarcaProductos item)
        {
            throw new NotImplementedException();
        }
    }
}
