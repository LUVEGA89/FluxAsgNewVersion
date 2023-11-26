using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Requerimiento.Solicitud
{
    public class SolicitudManager : WikiCore.Data.Catalog<Solicitud, int, SolicitudCriteria>
    {
        private Buzon.Area.AreaManager _AreaManager;
        public TipoReq.TipoRequerimientoManager tipoRequerimientoManager;
        public Concepto.ConceptoManager conceptoManager;
        public SolicitudManager()
            : base()
        {
            _AreaManager = new Buzon.Area.AreaManager();
            tipoRequerimientoManager = new TipoReq.TipoRequerimientoManager();
            conceptoManager = new Concepto.ConceptoManager();
        }

        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override Solicitud LoadItem(IDataReader dr)
        {
            return new Solicitud()
            {
                Identifier = (int)dr["Identifier"],
                Comentarios = (string)dr["Comentarios"],
                Area = _AreaManager.Find((int)dr["Area"]),
                Requerimiento = tipoRequerimientoManager.Find((int)dr["Requerimiento"]),
                Concepto = conceptoManager.Find((int)dr["Concepto"]),
                FechaRequerida = (DateTime)dr["FechaRequerida"],
                FechaCompromiso = (DBNull.Value.Equals(dr["FechaCompromiso"])) ? (DateTime)dr["FechaRequerida"] : (DateTime)dr["FechaCompromiso"],
                RegistradoEl = (DateTime)dr["RegistradoEl"],
                RegistradorPor = (string)dr["RegistradoPor"],
                Estatus = (EstatusKind)dr["Estatus"],
                EstatusDescripcion = (string)dr["EstatusDescripcion"]
            };
        }

        protected override DbCommand PrepareAddStatement(Solicitud item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("Request.prAddSolicitud");
            this.Database.AddInParameter(command, "@Comentarios", DbType.String, item.Comentarios);
            this.Database.AddInParameter(command, "@Area", DbType.Int32, item.Area.Identifier);
            this.Database.AddInParameter(command, "@Requerimiento", DbType.Int32, item.Requerimiento.Identifier);
            this.Database.AddInParameter(command, "@Concepto", DbType.Int32, item.Concepto.Identifier);
            this.Database.AddInParameter(command, "@FechaRequerida", DbType.DateTime, item.FechaRequerida);
            this.Database.AddInParameter(command, "@RegistradoPor", DbType.String, item.RegistradorPor);
            this.Database.AddOutParameter(command, "@Identifier", DbType.Int32, 4);
            return command;
        }


        protected override void BeforeAddExecuted(DataContext<Solicitud, int, SolicitudCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }

        protected override void CommandAddComplete(DataContext<Solicitud, int, SolicitudCriteria> context)
        {
            context.Item.Identifier = (int)context.Command.Parameters["@Identifier"].Value;
            context.Command.Parameters.Clear();
            
            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }

        protected override void CommandAddException(DataContext<Solicitud, int, SolicitudCriteria> context, Exception ex)
        {
            if (context.Transaction != null)
            {
                DbConnection connection = context.Transaction.Connection;
                context.Transaction.Rollback();

                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            base.CommandAddException(context, ex);
        }

       

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("Request.prGetSolicitud");
            this.Database.AddInParameter(command, "@Identifier", DbType.Int32, id);
            return command;
        }

        protected override DbCommand PrepareUpdateStatement(Solicitud item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("Request.prUpdSolicitud");
            this.Database.AddInParameter(command, "@Identifier", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(command, "@FechaCompromiso", DbType.DateTime, item.FechaCompromiso);
            this.Database.AddInParameter(command, "@Estatus", DbType.Int32, item.Estatus);
            this.Database.AddInParameter(command, "@ModificadoPor", DbType.String, item.Estatus);          
            return command;
        }

        public List<Solicitud> GetSolicitudes(SolicitudCriteria criteria)
        {
            var lista = new List<Solicitud>();
            DbCommand command = this.Database.GetStoredProcCommand("Request.prFindSolicitud");
            this.Database.AddInParameter(command, "@RegistradorPor", DbType.String, criteria.RegistradoPor);
            this.Database.AddInParameter(command, "@ResponsableArea", DbType.String, criteria.ResponsableArea);

            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Solicitud item = new Solicitud();

                item.Identifier = (int)dr["Identifier"];
                item.Comentarios = (string)dr["Comentarios"];
                item.Area = _AreaManager.Find((int)dr["Area"]);
                item.Requerimiento = tipoRequerimientoManager.Find((int)dr["Requerimiento"]);
                item.Concepto = conceptoManager.Find((int)dr["Concepto"]);
                item.FechaRequerida = (DateTime)dr["FechaRequerida"];
                item.FechaCompromiso = (DBNull.Value.Equals(dr["FechaCompromiso"])) ? (DateTime)dr["FechaRequerida"] : (DateTime)dr["FechaCompromiso"];
                item.RegistradoEl = (DateTime)dr["RegistradoEl"];
                item.RegistradorPor = (string)dr["RegistradoPor"];
                item.EstatusDescripcion = (string)dr["EstatusDescripcion"];
                lista.Add(item);
            }
            return lista;
        }



        public List<Estatus> GetEstatus()
        {
            var lista = new List<Estatus>();
            DbCommand command = this.Database.GetStoredProcCommand("Request.prFindEstatus");           
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Estatus item = new Estatus();

                item.Identifier = (int)dr["Identifier"];
                item.Descripcion = (string)dr["Descripcion"];
                lista.Add(item);
            }
            return lista;
        }


        public List<Solicitud> GetSolicitudesReporte(SolicitudCriteria criteria)
        {
            var lista = new List<Solicitud>();
            DbCommand command = this.Database.GetStoredProcCommand("Request.prFindSolicitudReporte");
            if(criteria.Area == 0)
            {
                this.Database.AddInParameter(command, "@Area", DbType.Int32, null);
            }
            else
            {
                this.Database.AddInParameter(command, "@Area", DbType.Int32, criteria.Area);
            }            
            this.Database.AddInParameter(command, "@Del", DbType.Date, criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.Date, criteria.Al);

            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Solicitud item = new Solicitud();

                item.Identifier = (int)dr["Identifier"];
                item.Comentarios = (string)dr["Comentarios"];
                item.Area = _AreaManager.Find((int)dr["Area"]);
                item.Requerimiento = tipoRequerimientoManager.Find((int)dr["Requerimiento"]);
                item.Concepto = conceptoManager.Find((int)dr["Concepto"]);
                item.FechaRequerida = (DateTime)dr["FechaRequerida"];
                item.FechaCompromiso = (DBNull.Value.Equals(dr["FechaCompromiso"])) ? (DateTime)dr["FechaRequerida"] : (DateTime)dr["FechaCompromiso"];
                item.RegistradoEl = (DateTime)dr["RegistradoEl"];
                item.RegistradorPor = (string)dr["RegistradoPor"];
                item.EstatusDescripcion = (string)dr["EstatusDescripcion"];
                lista.Add(item);
            }
            return lista;
        }
    }
}
