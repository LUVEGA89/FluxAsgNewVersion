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
    public class ActividadManager : Catalog<Actividad, int, ActividadCriteria>
    {
        ComentarioCatalog comentarioCatalog = new ComentarioCatalog();

        public ActividadManager()
            : base()
        {

        }

        public ActividadManager(string Database)
            : base(Database)
        {

        }
        protected override string FindPagedItemsProcedure
        {
            get { return "prFindActividad"; }
        }

        protected override Actividad LoadItem(IDataReader dr)
        {
            Actividad actividad = new Actividad();
            actividad.Identifier = (int)dr["Identificador"];
            actividad.Nombre = (string)dr["Titulo"];
            actividad.Estatus = (int)dr["Estatus"];
            actividad.Cliente = (string)dr["Cliente"];
            actividad.RegistradoEl = (DateTime)dr["RegistradoEl"];
            actividad.RegistradoPor = (string)dr["RegistradoPor"];
            actividad.Fecha = /*(DateTime)*/dr["Fecha"] == DBNull.Value ? null : (DateTime?)dr["Fecha"];
            actividad.Comentario = dr["Comentario"] == DBNull.Value ? "" : (string)dr["Comentario"];
            //actividad.ActualizadoEl = (DateTime)dr["ActualizadoEl"];
            //actividad.ActualizadoPor = (string)dr["ActualizadoPor"];
            actividad.ProximaActividad = dr["ProximaActividad"] == DBNull.Value ? null : (DateTime?)dr["ProximaActividad"];
            actividad.ListaComentarios = comentarioCatalog.FindPagedItems(new ComentarioCriteria() { Actividad = (int)dr["Identificador"] }).ToList();

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    ActividadFoto item = new ActividadFoto()
                    {
                        Identifier = (int)dr["Identificador"],
                        Actividad = (int)dr["Actividad"],
                        Foto = (string)dr["Foto"],
                        Estatus = (bool)dr["Estatus"]
                    };
                    actividad.AddItemImagen(item);
                }
            }

            return actividad;
        }

        protected override DbCommand PrepareAddStatement(Actividad item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddActividad");
            this.Database.AddInParameter(command, "@Titulo", DbType.String, item.Nombre);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, item.Cliente);
            this.Database.AddInParameter(command, "@Fecha", DbType.DateTime, item.Fecha);
            this.Database.AddInParameter(command, "@RegistradoPor", DbType.String, item.RegistradoPor);
            this.Database.AddInParameter(command, "@ProximaActividad", DbType.DateTime, item.ProximaActividad);
            this.Database.AddInParameter(command, "@Comentario", DbType.String, item.Comentario);
            this.Database.AddOutParameter(command, "@IdActividad", DbType.Int32, 4);
            return command;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prGetActividad");
            this.Database.AddInParameter(command, "@Id", DbType.Int16, id);
            return command;
        }

        protected override DbCommand PrepareUpdateStatement(Actividad item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(ActividadCriteria Criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(Criteria);
            this.Database.AddInParameter(cmd, "@CodigoCliente", DbType.String, Criteria.CodigoCliente);
            this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, Criteria.Al);

            return cmd;
        }

        protected override void BeforeAddExecuted(DataContext<Actividad, int, ActividadCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }

        // metodos para seguimiento a cliente
        protected override void CommandAddComplete(DataContext<Actividad, int, ActividadCriteria> context)
        {
            context.Item.Identifier = (int)context.Command.Parameters["@IdActividad"].Value;

            context.Command.Parameters.Clear();
            context.Command.CommandText = "prAddActividadFotos";
            this.Database.AddInParameter(context.Command, "@IdActividad", DbType.Int32, context.Item.Identifier);
            this.Database.AddInParameter(context.Command, "@Imagen", DbType.String);
            DbParameter IdActividad = context.Command.Parameters["@IdActividad"];
            DbParameter Imagen = context.Command.Parameters["@Imagen"];

            foreach (var item in context.Item.ListaImagenes)
            {
                Imagen.Value = item.Foto;
                this.Database.ExecuteNonQuery(context.Command, context.Transaction);
                //la transaccion deberá esta abierta para cada insercion a la base de datos
            }

            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }


        //manejo de carga de fotos actividad
        public IList<ActividadFoto> GetFotosActividad(int idActividad)
        {
            List<ActividadFoto> detalle = new List<ActividadFoto>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetActividadImagen");
            this.Database.AddInParameter(cmd, "@Id", DbType.String, idActividad);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            while (dr.Read())
            {
                detalle.Add(new ActividadFoto
                {
                    Foto = (string)dr["Foto"],
                    Estatus = (bool)dr["Estatus"],
                    Actividad = (int)dr["Actividad"]
                });
            }
            return detalle;
        }


    }
}
