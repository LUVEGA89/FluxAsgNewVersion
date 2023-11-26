using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Mobile.Rutas.DetallesRuta
{
    public class DetalleRutaManager : Catalog<DetalleRuta, int, DetalleRutaCriteria>
    {
        Evidencias.EvidenciaManager evidenciaManager = new Evidencias.EvidenciaManager();

        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override DetalleRuta LoadItem(IDataReader dr)
        {
            DetalleRuta detalleruta = new DetalleRuta();

            detalleruta.Identifier = (int)dr["Sequence"];
            detalleruta.Pedido = (int)dr["Pedido"];
            detalleruta.Status = (StatusDetalleRuta)(int)dr["Status"];
            detalleruta.Ubicacion = (string)dr["Ubicacion"];
            detalleruta.Evidencias = evidenciaManager.FindPagedItems(new Evidencias.EvidenciaCriteria { Pedido = (int)dr["Pedido"] }).ToList();
            
            return detalleruta;
        }

        protected override DbCommand PrepareAddStatement(DetalleRuta item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prGetDetallesRuta");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(DetalleRuta item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prUpdateDetalleRuta");
            this.Database.AddInParameter(cmd, "@Pedido", DbType.Int32, item.Pedido);
            this.Database.AddInParameter(cmd, "@Status", DbType.Int32, item.Status);
            return cmd;
        }

        protected override void CommandUpdateComplete(DataContext<DetalleRuta, int, DetalleRutaCriteria> context)
        {
            base.CommandUpdateComplete(context);
        }

        public void UpdateDetalleRuta(DetalleRuta item)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();

            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prUpdateDetalleRuta");
                this.Database.AddInParameter(cmd, "@Pedido", DbType.Int32, item.Pedido);
                this.Database.AddInParameter(cmd, "@Status", DbType.Int32, item.Status);
                this.Database.AddInParameter(cmd, "@Ubicacion", DbType.String, item.Ubicacion);

                this.Database.ExecuteNonQuery(cmd, transaction);
                cmd.Parameters.Clear();

                DbCommand command = this.Database.GetStoredProcCommand("mobile.prAddEvidencia");
                this.Database.AddInParameter(command, "@Imagen", DbType.String);
                this.Database.AddInParameter(command, "@Tipo", DbType.Int32);
                this.Database.AddInParameter(command, "@Pedido", DbType.Int32);

                DbParameter Imagen = command.Parameters["@Imagen"];
                DbParameter Tipo = command.Parameters["@Tipo"];
                DbParameter Pedido = command.Parameters["@Pedido"];

                foreach (Evidencias.Evidencia item1 in item.Evidencias)
                {
                    Imagen.Value = item1.Imagen;
                    Tipo.Value = item1.Tipo;
                    Pedido.Value = item.Pedido;

                    this.Database.ExecuteNonQuery(command, transaction);
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    DbConnection connection1 = transaction.Connection;
                    transaction.Rollback();

                    if (connection1 != null && connection1.State == ConnectionState.Open)
                    {
                        connection1.Close();
                    }
                }

                throw new Exception(ex.Message);
            }
            
        }
    }
}
