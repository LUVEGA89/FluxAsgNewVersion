using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Garantias
{
    public class GarantiaManager : Catalog<Garantia, int, GarantiaCriteria>
    {
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override Garantia LoadItem(IDataReader dr)
        {
            Garantia item = new Garantia();

            item.Sku = dr["SKU"] == DBNull.Value ? "" : (string)dr["SKU"];
            item.Descripcion = dr["Descripción"] == DBNull.Value ? "" : (string)dr["Descripción"];
            item.Marca = dr["Marca"] == DBNull.Value ? "": (string)dr["Marca"];
            item.Familia = (string)dr["Familia"];
            item.Categoria = (string)dr["Categoria"];
            item.SubCategoria = (string)dr["SubCategoria"];
            item.ordenCompra = (string)dr["OrdendeCompra"].ToString();
            item.CostoMx = Convert.ToDecimal(dr["CostoMx"]);
            item.Cantidad = Convert.ToDecimal(dr["cantidad"]);
            item.Fecha = Convert.ToDateTime(dr["Fecha"]);
            return item;
        }

        protected override DbCommand PrepareAddStatement(Garantia item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(Garantia item)
        {
            throw new NotImplementedException();
        }

        public List<Garantia> GetReporte()
        {
            List<Garantia> list = new List<Garantia>();
            DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetDetalleProducto_llegadas_reporte_garantias");

            IDataReader dr = this.Database.ExecuteReader(command);

            while (dr.Read())
            {
                list.Add(this.LoadItem(dr));
            }

            return list;
        }

       
    }
}
