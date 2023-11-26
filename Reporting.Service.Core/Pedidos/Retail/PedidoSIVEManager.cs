using Microsoft.Practices.EnterpriseLibrary.Data;
using Reporting.Service.Core.Evidencia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Pedidos.Retail
{
    public class PedidoSIVEManager : Catalog<PedidoSIVE, int, PedidoSIVECriteria>
    {
        public PedidoSIVEManager()
            : base()
        {

        }

        public PedidoSIVEManager(string Database)
            : base(Database)
        {

        }

        public PedidoSIVEManager(Database Database)
            : base()
        {

        }

        protected override string FindPagedItemsProcedure => "prFindPedidoSIVE";        

        protected override PedidoSIVE LoadItem(IDataReader dr)
        {
            PedidoSIVE item = new PedidoSIVE();
            item.Identifier = (int)dr["Identificador"];
            item.SubTotal = (decimal)dr["Subtotal"];
            item.Impuestos = (decimal)dr["Impuestos"];
            item.Total = (decimal)dr["Total"];
            item.RegistradoEl = (DateTime)dr["RegistradoEl"];
            item.EstadoSIVE = (QuoteStatus)dr["Estado"];

            item.EstadoSIE = DBNull.Value.Equals(dr["EstadoSIE"]) ? 0 : (PedidoSIVEKind)dr["EstadoSIE"];

            Cliente cliente = new Cliente();
            cliente.Codigo = (string)dr["Codigo"];
            cliente.Nombre = (string)dr["ClienteName"];
            item.Cliente = cliente;


            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    PedidoItem pedido = new PedidoItem();
                    pedido.Identifier = (int)dr["Identificador"];
                    pedido.Producto = (int)dr["Producto"];
                    pedido.SKU = (string)dr["SKU"];
                    pedido.Cantidad = (decimal)dr["Cantidad"];
                    pedido.Precio = (decimal)dr["Precio"];
                    pedido.Importe = (decimal)dr["Importe"];
                    pedido.Monto = (decimal)dr["Monto"];
                    item.AddItemPedido(pedido);
                }
            }

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    PedidoSIVEComentario itemComent = new PedidoSIVEComentario();
                    itemComent.Identifier = (int)dr["Identificador"];
                    itemComent.RegistradoPor = (string)dr["RegistradoPor"];
                    itemComent.RegistradoEl = (DateTime)dr["RegistradoEl"];
                    itemComent.Accion = (string)dr["Accion"];
                    itemComent.Comentario = (string)dr["Comentario"];
                    itemComent.Estatus = (bool)dr["Estatus"];
                    item.AddItemComentario(itemComent);
                }
            }

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Core.Evidencia.Evidencia itemEvidencia = new Evidencia.Evidencia();
                    item.Identifier = (int)dr["Identificador"];
                    itemEvidencia.FolioSIE = (int)dr["FolioSIE"];
                    itemEvidencia.RegistradoEl = (DateTime)dr["RegistradoEl"];
                    itemEvidencia.RegistradoPor = (string)dr["Registrado"];
                    itemEvidencia.FileByte = dr["FileByte"] == DBNull.Value ? "" : (string)dr["FileByte"];
                    itemEvidencia.FileName = (string)dr["FileName"];
                    itemEvidencia.Extension = (string)dr["Extension"];
                    itemEvidencia.Activo = (bool)dr["Activo"];
                    itemEvidencia.TipoEvidencia = (EvidenciaKind)dr["TipoEvidencia"];
                    item.Evidencia = itemEvidencia;
                }
            }            
            return item;
        }

        protected override DbCommand PrepareAddStatement(PedidoSIVE item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddPedidoSIVE");

            this.Database.AddInParameter(cmd, "@PedidoSIVE", DbType.Int32, item.PedidosSIVE);
            this.Database.AddInParameter(cmd, "@EstadoSIVE", DbType.Int32, item.EstadoSIVE);
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, item.Cliente.Codigo);
            this.Database.AddInParameter(cmd, "@EstadoSIE", DbType.Int32, item.EstadoSIE);
            this.Database.AddOutParameter(cmd, "@IdPedidoSIVE", DbType.Int32, 4);

            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(PedidoSIVECriteria Criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(Criteria);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Criteria.TipoConsulta);
            return cmd;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prGetPedido");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, id);
            return command;
        }

        protected override void BeforeAddExecuted(DataContext<PedidoSIVE, int, PedidoSIVECriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();

        }

        protected override void CommandAddComplete(DataContext<PedidoSIVE, int, PedidoSIVECriteria> context)
        {
            context.Item.Identifier = (int)context.Command.Parameters["@IdPedidoSIVE"].Value;

            context.Command.Parameters.Clear();
            context.Command.CommandText = "prAddPedidoSIVEComentario";
            this.Database.AddInParameter(context.Command, "@PedidoSIVE", DbType.Int32, context.Item.Identifier);

            this.Database.AddInParameter(context.Command, "@Accion", DbType.String);
            this.Database.AddInParameter(context.Command, "@RegistradoPor", DbType.String);
            this.Database.AddInParameter(context.Command, "@Comentario", DbType.String);

            DbParameter PedidoSIVE = context.Command.Parameters["@PedidoSIVE"];
            DbParameter Accion = context.Command.Parameters["@Accion"];
            DbParameter RegistradoPor = context.Command.Parameters["@RegistradoPor"];
            DbParameter Comentario = context.Command.Parameters["@Comentario"];

            foreach (var item in context.Item.Items)
            {
                Accion.Value = item.Accion;
                RegistradoPor.Value = item.RegistradoPor;
                Comentario.Value = item.Comentario;

                this.Database.ExecuteNonQuery(context.Command, context.Transaction);
                //la transaccion deberá esta abierta para cada insercion a la base de datos
            }


            if (context.Item.EstadoSIE == PedidoSIVEKind.CreditoCobranza)
            {
                context.Command.Parameters.Clear();
                context.Command.CommandText = "prUpdatePedidoSIVESAP";
                this.Database.AddInParameter(context.Command, "@PedidoSIVE", DbType.Int32, context.Item.Identifier);
                this.Database.AddInParameter(context.Command, "@EstadoSIVE", DbType.Int32);
                DbParameter EstadoSIVE = context.Command.Parameters["@EstadoSIVE"];
                EstadoSIVE.Value = QuoteStatus.InProcess;

                this.Database.ExecuteNonQuery(context.Command, context.Transaction);
            }

            // rechazar el pedido por credito y cobranza SIE y afecta en SIVE rechazar
            if (context.Item.EstadoSIE == PedidoSIVEKind.Rechazado)
            {
                context.Command.Parameters.Clear();
                context.Command.CommandText = "prUpdatePedidoSIVESAP";
                this.Database.AddInParameter(context.Command, "@PedidoSIVE", DbType.Int32, context.Item.Identifier);
                this.Database.AddInParameter(context.Command, "@EstadoSIVE", DbType.Int32);
                DbParameter EstadoSIVE = context.Command.Parameters["@EstadoSIVE"];
                EstadoSIVE.Value = QuoteStatus.Rejected;

                this.Database.ExecuteNonQuery(context.Command, context.Transaction);
            }

            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }

        protected override void CommandAddException(DataContext<PedidoSIVE, int, PedidoSIVECriteria> context, Exception ex)
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


        protected override DbCommand PrepareUpdateStatement(PedidoSIVE item)
        {
            throw new NotImplementedException();
        }
    }
}
