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
    public class ComentarioCatalog : Catalog<Comentario, int, ComentarioCriteria>
    {
        RespuestaCatalog respuestaCatalog = new RespuestaCatalog();

        

        public int Actividad { get; set; }

        public ComentarioCatalog()
            : base()
        {

        }        

        public ComentarioCatalog(string database)
            : base(database)
        {

        }


        protected override string FindPagedItemsProcedure
        {
            get { return "prFindActividadComentarios"; }
        }
        
        protected override Comentario LoadItem(IDataReader dr)
        {
            Comentario comentario = new Comentario();

            comentario.Identifier = (int)dr["Identificador"];
            comentario.Nombre = (string)dr["Comentario"];
            comentario.RegistradorEl = (DateTime)dr["RegistradoEl"];
            comentario.ReigstradoPor = (string)dr["RegistradoPor"];
            comentario.Estatus = (bool)dr["Estatus"];
            //comentario.NodoPadre = dr["NodoPadre"] == DBNull.Value ? (int)dr["NodoPadre"] : 
            comentario.ListaRepuestas = respuestaCatalog.FindPagedItems(new RespuestaCriteria() { Comentario = (int)dr["Identificador"] }).ToList();
            return comentario;
        }
        // para agregar a actividad catalogo 
        protected override DbCommand PrepareAddStatement(Comentario item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddActividadComentario");
            this.Database.AddInParameter(command, "@Actividad", DbType.Int32, this.Actividad);
            this.Database.AddInParameter(command, "@RegistradoPor", DbType.String, item.ReigstradoPor);
            this.Database.AddInParameter(command, "@Comentario", DbType.String, item.Nombre);
            this.Database.AddOutParameter(command, "@IdComentario", DbType.Int32, 4);
            return command;
        }
        // metodo para agregar eliminar
        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }
        // buscar por cada item (Sequence)
        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prGetActividadComentario");
            this.Database.AddInParameter(command, "@Id", DbType.Int16, id);
            return command;
        }
        // para actualizar 
        protected override DbCommand PrepareUpdateStatement(Comentario item)
        {
            throw new NotImplementedException();
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
