using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Mobile.Rutas
{
    public class RutasManager : Catalog<Ruta, int, RutaCriteria>
    {
        Choferes.ChoferesManager ChoferManager = new Choferes.ChoferesManager();
        PedidosChofer.PedidosChoferManager PedidoChoferManager = new PedidosChofer.PedidosChoferManager();

        protected override string FindPagedItemsProcedure => "mobile.prFindRutas";

        protected override Ruta LoadItem(IDataReader dr)
        {
            Ruta ruta = new Ruta();

            ruta.Identifier = (int)dr["Sequence"];
            ruta.Nombre = (string)dr["Nombre"];
            ruta.Fecha = (DateTime)dr["Fecha"];
            ruta.HoraSalida = (string)dr["HoraSalida"];
            ruta.HoraLlegada = (string)dr["HoraLlegada"];
            ruta.KmSalida = (decimal)dr["KmSalida"];
            ruta.KmLlegada = (decimal)dr["KmLlegada"];
            ruta.Status = (StatusRutas)(int)dr["Status"];
            ruta.Chofer = ChoferManager.Find((string)dr["Chofer"]);
            ruta.Pedidos = PedidoChoferManager.FindPagedItems(new PedidosChofer.PedidoChoferCriteria {Chofer = ruta.Chofer.Identifier, Ruta = ruta.Identifier }).ToList();

            return ruta;
        }

        protected override void BeforeAddExecuted(DataContext<Ruta, int, RutaCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }

        protected override DbCommand PrepareAddStatement(Ruta item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prAddRuta");

            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, item.Nombre);
            this.Database.AddInParameter(cmd, "@Fecha", DbType.DateTime, item.Fecha);
            this.Database.AddInParameter(cmd, "@HoraSalida", DbType.String, item.HoraSalida);
            this.Database.AddInParameter(cmd, "@HoraLlegada", DbType.String, item.HoraLlegada);
            this.Database.AddInParameter(cmd, "@Chofer", DbType.String, item.Chofer.Identifier);
            this.Database.AddInParameter(cmd, "@KmSalida", DbType.Decimal, item.KmSalida);
            this.Database.AddInParameter(cmd, "@KmLlegada", DbType.Decimal, item.KmLlegada);
            this.Database.AddInParameter(cmd, "@Status", DbType.Int32, item.Status);

            this.Database.AddOutParameter(cmd, "@IdRuta", DbType.Int32, 32);

            return cmd;
        }

        protected override void CommandAddComplete(DataContext<Ruta, int, RutaCriteria> context)
        {
            context.Item.Identifier = (int)context.Command.Parameters["@IdRuta"].Value;

            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prAddDetalleRuta");
            this.Database.AddInParameter(cmd, "@Ruta", DbType.Int32);
            this.Database.AddInParameter(cmd, "@Pedido", DbType.Int32);
            this.Database.AddInParameter(cmd, "@Status", DbType.Int32);

            DbParameter Ruta = cmd.Parameters["@Ruta"];
            DbParameter Pedido = cmd.Parameters["@Pedido"];
            DbParameter Status = cmd.Parameters["@Status"];

            Ruta.Value = context.Item.Identifier;
            foreach (DetallesRuta.DetalleRuta item in context.Item.DetalleRuta)
            {
                Pedido.Value = item.Pedido;
                Status.Value = item.Status;

                this.Database.ExecuteNonQuery(cmd, context.Transaction);
            }

            context.Transaction.Commit();
        }

        protected override void CommandAddException(DataContext<Ruta, int, RutaCriteria> context, Exception ex)
        {
            if (context.Transaction != null)
            {
                DbConnection connection = context.Transaction.Connection;

                context.Transaction.Rollback();

                if (connection != null && connection.State == ConnectionState.Open)
                    connection.Close();
            }
            base.CommandAddException(context, ex);
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prGetRuta");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Ruta item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("mobile.prUpdateRuta");

            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(cmd, "@Status", DbType.Int32, item.Status);
            this.Database.AddInParameter(cmd, "@KmSalida", DbType.Decimal, item.KmSalida);
            this.Database.AddInParameter(cmd, "@KmLlegada", DbType.Decimal, item.KmLlegada);
            this.Database.AddInParameter(cmd, "@HoraLlegada", DbType.String, item.HoraLlegada);


            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(RutaCriteria criteria)
        {
            return base.PrepareFindPagedItemsStatement(criteria);
        }
    }
}
