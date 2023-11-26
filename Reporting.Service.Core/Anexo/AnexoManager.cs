using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Anexo
{
    public class AnexoManager : Catalog<Anexo, int, AnexoCriteria>
    {

        public AnexoManager()
            : base()
        {

        }

        public AnexoManager(string Database)
            : base(Database)
        {

        }

        public AnexoManager(Database Database)
            : base(Database)
        {

        }

        private int EmpresaSucursal;

        protected override string FindPagedItemsProcedure => "anx.FindAnexo";

        private Modulo.ModuloManager ModuloManager = new Modulo.ModuloManager();
        private Core.Anexo.Sucursal.SucursalManager SucursalManager = new Sucursal.SucursalManager();
        private Core.Anexo.Empresa.EmpresaManager EmpresaManager = new Empresa.EmpresaManager();


        protected override Anexo LoadItem(IDataReader dr)
        {
            Anexo item = new Anexo();

            item.Identifier = (int)dr["Identificador"];
            item.FolioSIE = (int)dr["FoioSIE"];
            item.FolioSAP = (int)dr["FolioSAP"];
            item.ErrorCodeSAP = (dr["ErrorCodeSAP"] == DBNull.Value) ? "" : (string)dr["ErrorCodeSAP"];
            item.RegistradoEl = (DateTime)dr["RegistradoEl"];
            item.Modulo = dr["Modulo"] == DBNull.Value ? null : ModuloManager.Find((int)dr["Modulo"]);
            item.Sucursal = dr["Sucursal"] == DBNull.Value ? null : SucursalManager.Find((int)dr["Surcursal"]);
            item.Empresa = dr["Empresa"] == DBNull.Value ? null : EmpresaManager.Find((int)dr["Empresa"]);
            item.Activo = (bool)dr["Activo"];

            return item;
        }

        protected override DbCommand PrepareAddStatement(Anexo item)
        {
            // valida si la empresa y la sucursal existe antes de registrar el anexo correspondiente


            DbCommand command = this.Database.GetStoredProcCommand("anx.prAddAnexo");
            this.Database.AddInParameter(command, "@FolioSIE", DbType.Int32, item.FolioSIE);
            this.Database.AddInParameter(command, "@FolioSAP", DbType.Int32, item.FolioSAP);
            this.Database.AddInParameter(command, "@Modulo", DbType.Int32, item.Modulo.Identifier);
            this.Database.AddInParameter(command, "@ErrorCodeSAP", DbType.String, item.ErrorCodeSAP);
            if (EmpresaSucursal != 0)
            {
                this.Database.AddInParameter(command, "@EmpresaSucursal", DbType.Int32, EmpresaSucursal);
            }
            else
            {
                throw new Exception("No se ha definido la empresa sucursal del Sistema: " + item.Modulo.Sistema + ", modulo de " + item.Modulo.Nombre);
            }            
            return command;
        }

        protected override void BeforeAddExecuted(DataContext<Anexo, int, AnexoCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }

        public int ReturnEmpresaSucursal(int Empresa, int Sucursal)
        {
            try
            {

                DbCommand command = this.Database.GetStoredProcCommand("anx.prAddEmpresaSucursal");
                if (Empresa != 0)
                {
                    this.Database.AddInParameter(command, "@Empresa", DbType.Int32, Empresa);
                }
                if (Sucursal != 0)
                {
                    this.Database.AddInParameter(command, "@Sucursal", DbType.Int32, Sucursal);
                }

                this.Database.AddOutParameter(command, "@EmpresaSucursalOut", DbType.Int32, 4);
                this.Database.ExecuteNonQuery(command);

                EmpresaSucursal = Convert.ToInt32(this.Database.GetParameterValue(command, "@EmpresaSucursalOut"));

                command.Dispose();

                return EmpresaSucursal;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        protected override void CommandAddComplete(DataContext<Anexo, int, AnexoCriteria> context)
        {
            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }

        protected override void CommandAddException(DataContext<Anexo, int, AnexoCriteria> context, Exception ex)
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
            DbCommand command = this.Database.GetStoredProcCommand("anx.prGetAnexo");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, id);
            return command;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(AnexoCriteria Criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(Criteria);

            this.Database.AddInParameter(cmd, "@Modulo", DbType.Int32, Criteria.Modulo);
            this.Database.AddInParameter(cmd, "@Sucursal", DbType.Int32, Criteria.Sucursal);
            this.Database.AddInParameter(cmd, "@Empresa", DbType.Int32, Criteria.Empresa);
            this.Database.AddInParameter(cmd, "@FolioSAP", DbType.Int32, Criteria.FolioSAP);
            this.Database.AddInParameter(cmd, "@FolioSIE", DbType.Int32, Criteria.FolioSIE);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Anexo item)
        {
            throw new NotImplementedException();
        }
    }
}
