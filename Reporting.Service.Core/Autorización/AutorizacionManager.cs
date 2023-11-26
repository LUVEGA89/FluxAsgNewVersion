using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Autorización
{
    public class AutorizacionManager : Catalog<Autorizacion, int, AutorizacionCriteria>
    {
        public AutorizacionManager()
        :base()
        {

        }
        protected override string FindPagedItemsProcedure => "prFindAutorizaciones";

        protected override Autorizacion LoadItem(IDataReader dr)
        {
            Autorizacion nueva = new Autorizacion();

            nueva.Identifier = (int)dr["idAuth"];
            nueva.Cliente = (string)dr["cliente"];
            nueva.Importe = (decimal)dr["importe"];
            nueva.tienda = (string)dr["tienda"];
            nueva.fecha = (string)((DateTime)dr["fecha"]).ToShortDateString();
            nueva.referencia = (string)dr["referencia"];
            nueva.comentarios = (string)dr["comentarios"];
            nueva.status = (int)dr["status"];
            nueva.PedidosTiendasWEB = (int)dr["PedidosTiendasWEB"];

            return nueva;
        }

        protected override DbCommand PrepareAddStatement(Autorizacion item)
        {
            DateTime ahora = DateTime.Now;
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddAutorizacion");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, item.Cliente);
            this.Database.AddInParameter(cmd, "@Importe", DbType.Decimal, item.Importe);
            this.Database.AddInParameter(cmd, "@fecha", DbType.Date, ahora);
            this.Database.AddInParameter(cmd, "@referencia", DbType.String, item.referencia);
            this.Database.AddInParameter(cmd, "@comentarios", DbType.String, item.comentarios);
            this.Database.AddInParameter(cmd, "@status", DbType.Int16, item.status);
            this.Database.AddInParameter(cmd, "@registros_id", DbType.Int16, item.PedidosTiendasWEB);
            return cmd;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAutorizacion");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int16, id);
            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(AutorizacionCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Autorizacion item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpAutorizacion");
            this.Database.AddInParameter(cmd, "@id", DbType.Int16, item.Identifier);
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, item.Cliente);
            this.Database.AddInParameter(cmd, "@Importe", DbType.Decimal, item.Importe);
            this.Database.AddInParameter(cmd, "@fecha", DbType.Date, item.fecha);
            this.Database.AddInParameter(cmd, "@referencia", DbType.String, item.referencia);
            this.Database.AddInParameter(cmd, "@comentarios", DbType.String, item.comentarios);
            this.Database.AddInParameter(cmd, "@status", DbType.Int16, item.status);
            this.Database.AddInParameter(cmd, "@registros_id", DbType.Int16, item.PedidosTiendasWEB);
            return cmd;
        }
    }
}
