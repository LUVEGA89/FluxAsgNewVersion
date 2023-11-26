using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Trafico.Contenedor.SeguimientoComentario
{
    public class SeguimientoComentarioManager : Catalog<SeguimientoComentario, int, SeguimientoComentarioCriteria>
    {
        public SeguimientoComentarioManager()
            :base()
        {

        }

        protected override string FindPagedItemsProcedure => "prFindSeguimientoComentarios";

        protected override SeguimientoComentario LoadItem(IDataReader dr)
        {
            SeguimientoComentario nuevo = new SeguimientoComentario();
            nuevo.Identifier = int.Parse(dr["idComentario"].ToString());
            nuevo.seguimiento_id = int.Parse(dr["seguimiento_id"].ToString());
            nuevo.coment = (string)dr["comentario"];
            nuevo.fecRegistro = (DateTime)dr["fechaRegistro"];
            nuevo.usuario = (string)dr["Usuario"];
            nuevo.parentID = int.Parse(dr["parentID"].ToString());
            return nuevo;

        }

        protected override DbCommand PrepareAddStatement(SeguimientoComentario item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddSeguimientoComentario");
            this.Database.AddInParameter(cmd, "@idSeg", DbType.Int16, item.seguimiento_id);
            this.Database.AddInParameter(cmd, "@comentario", DbType.String, item.coment);
            this.Database.AddInParameter(cmd, "@fechaReg", DbType.Date, DateTime.Now);
            this.Database.AddInParameter(cmd, "@usuario", DbType.String, item.usuario);
            this.Database.AddInParameter(cmd, "@parentID", DbType.Int16, item.parentID);
            return cmd;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSeguimientoComentario");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int16, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(SeguimientoComentario item)
        {
            throw new NotImplementedException();
        }
        protected override DbCommand PrepareFindPagedItemsStatement(SeguimientoComentarioCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);

            this.Database.AddInParameter(cmd, "@idSeguimiento", DbType.Int16, criteria.seguimientoId);
            this.Database.AddInParameter(cmd, "@parentID", DbType.Int16, criteria.parentID);

            return cmd;
        }
    }
}
