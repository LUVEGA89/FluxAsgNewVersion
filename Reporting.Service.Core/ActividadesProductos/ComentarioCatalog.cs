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
    public class ComentarioCatalog : Catalog<Comentario, int, ComentarioCriteria>
    {
        RespuestaCatalog respuestaCatalog = new RespuestaCatalog();

        public int Actividad { get; set; }

        public ComentarioCatalog()
        {
        }
        public ComentarioCatalog(string database)
            : base(database)
        {
        }

        protected override string FindPagedItemsProcedure
        {
            get { return "prFindActividadProductosComentarios"; }
        }
        protected override Comentario LoadItem(IDataReader dr)
        {
            Comentario comentario = new Comentario();

            comentario.Identifier = (int)dr["Identificador"];
            comentario.Comentarios = (string)dr["Comentario"];
            comentario.RegistradorEl = (DateTime)dr["RegistradoEl"];
            comentario.ReigstradoPor = (string)dr["RegistradoPor"];
            comentario.Estatus = (bool)dr["Estatus"];
            comentario.PermisoRol = (int)dr["PermisosRol"];
            //comentario.NodoPadre = dr["NodoPadre"] == DBNull.Value ? (int)dr["NodoPadre"] : 
            comentario.ListaRepuestas = respuestaCatalog.FindPagedItems(
                new RespuestaCriteria()
                {
                    Comentario = (int)dr["Identificador"]
                }).ToList();

            return comentario;
            //throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(Comentario item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddActividadProductosComentario");
            this.Database.AddInParameter(command, "@Actividad", DbType.Int32, this.Actividad);
            this.Database.AddInParameter(command, "@RegistradoPor", DbType.String, item.ReigstradoPor);
            this.Database.AddInParameter(command, "@Comentario", DbType.String, item.Comentarios);
            this.Database.AddInParameter(command, "@PermisosRol", DbType.Int32, item.PermisoRol);
            this.Database.AddOutParameter(command, "@IdComentario", DbType.Int32, 4);
            return command;
            // throw new NotImplementedException();
        }
        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prGetActividadProductosComentario");
            this.Database.AddInParameter(command, "@Id", DbType.Int16, id);
            return command;
        }
        protected override DbCommand PrepareUpdateStatement(Comentario item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prUpdateActividadComentario");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(command, "@PermisosRol", DbType.Int32, item.PermisoRol);
            return command;
        }
        // para buscar un comentarios de acuerdo a una actividad
        protected override DbCommand PrepareFindPagedItemsStatement(ComentarioCriteria Criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(Criteria);
            this.Database.AddInParameter(cmd, "@Actividad", DbType.Int16, Criteria.Actividad);
            return cmd;
        }
    }
}
