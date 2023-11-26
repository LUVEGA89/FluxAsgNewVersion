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
    public class RespuestaCatalog : Catalog<Respuesta, int, RespuestaCriteria>
    {
        public int Actividad { get; set; }

        public RespuestaCatalog()
        {
        }
        public RespuestaCatalog(string database)
            : base(database)
        {

        }
        protected override string FindPagedItemsProcedure
        {
            get { return "prFindActividadProductosComentarioRepuestas"; }
        }
        protected override Respuesta LoadItem(IDataReader dr)
        {
            Respuesta respuesta = new Respuesta();
            respuesta.Identifier = (int)dr["Identificador"];
            respuesta.Comentario = (string)dr["Comentario"];
            respuesta.RegistradorEl = (DateTime)dr["RegistradoEl"];
            respuesta.ReigstradoPor = (string)dr["RegistradoPor"];
            respuesta.Estatus = (bool)dr["Estatus"];
            respuesta.NodoPadre = (int)dr["NodoPadre"];
            respuesta.PermisosRol = (int)dr["PermisosRol"];
            return respuesta;
            //throw new NotImplementedException();
        }
        protected override DbCommand PrepareAddStatement(Respuesta item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddActividadProductosComentarioRespuesta");
            this.Database.AddInParameter(command, "@Actividad", DbType.Int32, this.Actividad);
            this.Database.AddInParameter(command, "@ComentarioNodoPadre", DbType.Int32, item.NodoPadre);
            this.Database.AddInParameter(command, "@Respuesta", DbType.String, item.Comentario);
            this.Database.AddInParameter(command, "@RegistradoPor", DbType.String, item.ReigstradoPor);
            this.Database.AddInParameter(command, "@PermisosRol", DbType.Int32, item.PermisosRol);
            return command;
            //throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prGetActividadProductosComentarioRespuestas");
            this.Database.AddInParameter(command, "@Id", DbType.Int16, id);
            return command;
            //throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(Respuesta item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prUpdateActividadComentario");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(command, "@PermisosRol", DbType.Int32, item.PermisosRol);
            return command;
        }
        protected override DbCommand PrepareFindPagedItemsStatement(RespuestaCriteria Criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(Criteria);
            this.Database.AddInParameter(cmd, "@Comentario", DbType.Int16, Criteria.Comentario);
            return cmd;
        }
    }
}
