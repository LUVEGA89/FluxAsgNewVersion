using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ApartadoMercancia.Factura
{
    public class FacturaManager : Catalog<Factura, int, FacturaCriteria>
    {
        protected override string FindPagedItemsProcedure => "prFindFacturasByCliente";

        protected override Factura LoadItem(IDataReader dr)
        {
            Factura nueva = new Factura();
            nueva.Identifier = int.Parse(dr["DocNum"].ToString());
            nueva.fecha = (DateTime)dr["DocDate"];
            nueva.importe = (decimal)dr["DocTotal"];

            return nueva;
        }

        protected override DbCommand PrepareAddStatement(Factura item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetUltimaFactura");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);
            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(FacturaCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            this.Database.AddInParameter(cmd, "@cliente", DbType.String, criteria.CardCode);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Factura item)
        {
            throw new NotImplementedException();
        }
    }
}
