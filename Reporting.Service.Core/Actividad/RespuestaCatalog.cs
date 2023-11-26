using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Actividad
{
    public class RespuestaCatalog : Catalog<Respuesta, int, RespuestaCriteria>
    {
        public int Actividad { get; set; }

        

        public RespuestaCatalog()
            : base()
        {

        }

        public RespuestaCatalog(string database)
            : base(database)
        {

        }


        protected override string FindPagedItemsProcedure
        {
            get { return "prFindActividadComentarioRepuestas"; }
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
            return respuesta;
        }

        //agregar respuesta
        protected override DbCommand PrepareAddStatement(Respuesta item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddActividadComentarioRespuesta");
            this.Database.AddInParameter(command, "@Actividad", DbType.Int32, this.Actividad);
            this.Database.AddInParameter(command, "@ComentarioNodoPadre", DbType.Int32, item.NodoPadre);
            this.Database.AddInParameter(command, "@Respuesta", DbType.String, item.Comentario);
            this.Database.AddInParameter(command,"@RegistradoPor", DbType.String, item.ReigstradoPor);
            return command;

            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prGetActividadComentarioRespuestas");
            this.Database.AddInParameter(command, "@Id", DbType.Int16, id);
            return command;
        }

        protected override DbCommand PrepareUpdateStatement(Respuesta item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(RespuestaCriteria Criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(Criteria);
            this.Database.AddInParameter(cmd, "@Comentario", DbType.Int16, Criteria.Comentario);            
            return cmd;
        }
    }

}
