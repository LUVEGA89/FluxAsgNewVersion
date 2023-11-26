using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Evidencia
{
    public class EvidenciaManager : Catalog<Evidencia, int, EvidenciaCriteria>
    {

        public EvidenciaManager()
            : base()
        {

        }

        public EvidenciaManager(string Database)
            : base(Database)
        {

        }

        public EvidenciaManager(Database Database)
            : base(Database)
        {

        }

        protected override string FindPagedItemsProcedure => "prFindEvidencia";

        Modulo.ModuloManager ModuloManager = new Modulo.ModuloManager();

        protected override Evidencia LoadItem(IDataReader dr)
        {
            Evidencia item = new Evidencia();

            item.Identifier = (int)dr["Sequence"];
            item.FolioSIE = (int)dr["FolioSIE"];
            item.RegistradoEl = (DateTime)dr["RegistradoEl"];
            item.RegistradoPor = (string)dr["RegistradoPor"];
            item.FileByte = dr["FileByte"] == DBNull.Value ? "" : (string)dr["FileByte"];
            item.FileName = dr["FileName"] == DBNull.Value ? "N" : (string)dr["FileName"];
            item.Extension = (string)dr["Extension"];
            item.Activo = (bool)dr["Activo"];
            item.Modulo = dr["EvidenciaModulo"] == DBNull.Value ? null : ModuloManager.Find((int)dr["EvidenciaModulo"]);
            item.TipoEvidencia = (EvidenciaKind)dr["TipoEvidencia"];

            return item;
        }

        protected override DbCommand PrepareAddStatement(Evidencia item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddEvidencia");
            this.Database.AddInParameter(command, "@FolioSIE", DbType.Int32, item.FolioSIE);
            this.Database.AddInParameter(command, "@EvidenciaModulo", DbType.Int32, item.Modulo.Identifier);
            this.Database.AddInParameter(command, "@RegistradoEl", DbType.DateTime, item.RegistradoEl);
            this.Database.AddInParameter(command, "@RegistradoPor", DbType.String, item.RegistradoPor);
            if (string.IsNullOrEmpty(item.FileByte) && item.FileByte != null)
            {
                this.Database.AddInParameter(command, "@FileByte", DbType.String, item.FileByte);
            }
            this.Database.AddInParameter(command, "@FileName", DbType.String, item.FileName);
            this.Database.AddInParameter(command, "@Extension", DbType.String, item.Extension);
            this.Database.AddInParameter(command, "@TipoEvidencia", DbType.Int32, item.TipoEvidencia);
            return command;
        }

        protected override void BeforeAddExecuted(DataContext<Evidencia, int, EvidenciaCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }

        protected override void CommandAddComplete(DataContext<Evidencia, int, EvidenciaCriteria> context)
        {
            //context.Item.Identifier = (int)context.Command.Parameters["@IdPedidoSIVE"].Value;            
            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }

        protected override void CommandAddException(DataContext<Evidencia, int, EvidenciaCriteria> context, Exception ex)
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
        protected override DbCommand PrepareFindPagedItemsStatement(EvidenciaCriteria Criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(Criteria);
            this.Database.AddInParameter(cmd, "@FolioSIE", DbType.Int32, Criteria.FolioSIE);
            return cmd;
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prGetEvidencia");
            this.Database.AddInParameter(command, "@Identifier", DbType.Int32, id);
            return command;
        }

        protected override DbCommand PrepareUpdateStatement(Evidencia item)
        {
            throw new NotImplementedException();
        }

        public List<Evidencia> GetEvidencias(int FolioSIE)
        {
            List<Evidencia> items = new List<Evidencia>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetEvidencias");
            this.Database.AddInParameter(command, "@FolioSIE", DbType.Int32, FolioSIE);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                items.Add(this.LoadItem(dr));
            }
            dr.Close();
            dr.Dispose();
            command.Dispose();

            return items;
        }
    }
}
