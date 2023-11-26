using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;
using Reporting.Service.Core.NotaCredito.ConceptoDescuento;

namespace Reporting.Service.Core.NotaCredito
{
    public class NotaCreditoManager : Catalog<NotaCredito, int, NotaCreditoCriteria>
    {
        public NotaCreditoManager()
            : base("DefaultConnection")
        {

        }

        protected override string FindPagedItemsProcedure => "prFindNotasCredito";

        protected override NotaCredito LoadItem(IDataReader dr)
        {
            NotaCredito nc = new NotaCredito();

            try
            {
                nc.Identifier = (int)dr["Sequence"];
                nc.FolioOrigen = (string)dr["FolioOrigen"];
                nc.FolioDestino = (string)dr["FolioDestino"];
                nc.ConceptoDescuento = (DBNull.Value.Equals(dr["ConceptoDescuento"])) ? 0 : (int)dr["ConceptoDescuento"];
                nc.Canal = (string)dr["Canal"];
                nc.TipoDocumento = (string)dr["TipoDocumento"];
                nc.Valor = (int)dr["Valor"];
                nc.ConceptoDescuentoDetalle = (string)dr["ConceptoDescuentoDetalle"];
                nc.Comentario = (string)dr["Comentario"];
                nc.ComentarioDetalle = (string)dr["Comentario"];
                nc.Usuario = (string)dr["Usuario"];
                nc.Fecha = (DateTime)dr["Fecha"];
                nc.Cuenta = (string)dr["Cuenta"];
                nc.Email = (string)dr["Email"];
                nc.FolioSap = (string)dr["FolioSap"];
                nc.FolioPagoSAP = (string)dr["FolioPagoSAP"];
                nc.CardName = (string)dr["CardName"];
                nc.ClienteName = (string)dr["ClienteName"];
                nc.Importe = dr["Importe"] == DBNull.Value ? 0.0m : (decimal)dr["Importe"];
                nc.Vendedor = (string)dr["Vendedor"];

                string Coment = string.Empty;
                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        NotaCreditoItem nci = new NotaCreditoItem();

                        nci.ItemCode = (string)dr["ItemCode"];
                        nci.Cantidad = (int)dr["Cantidad"];
                        nci.Precio = (decimal)dr["Precio"];
                        nci.Descuento = (decimal)dr["Descuento"];
                        nci.CuentaContable = (string)dr["CuentaContable"];
                        nc.AddItem(nci);
                    }
                }
                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        Coment = Coment + (string)dr["Comentarios"] + "," + " ";
                    }
                    Coment.TrimEnd(' ');
                    Coment.TrimEnd(',');
                    nc.ComentarioDetalle += "<p>Detalle:</p> " + Coment + "<br/>";
                }
                //// para agregar las imagenes
                //if (dr.NextResult())
                //{
                //    while (dr.Read())
                //    {
                //        NotaCreditoImagen item = new NotaCreditoImagen();
                //        item.UserName = (string)dr["UsuarioName"];
                //        item.ImagenBase64 = (string)dr["Imagen"];
                //        item.Estatus = (bool)dr["Estatus"];
                //        item.RegistradoEl = (DateTime)dr["RegistradoEl"];
                //        item.Tipo = (EvidenciaKind)(int)dr["Tipo"];
                //        item.Extension = (string)dr["Extension"];
                //        item.FileType = (string)dr["FileType"];
                //        nc.AddImagen(item);
                //    }
                //}

            }
            catch (Exception ex)
            {

                throw new InvalidOperationException(ex.Message);
            }
            return nc;
        }


        protected NotaCredito LoadItemImagenes(IDataReader dr)
        {
            NotaCredito nc = new NotaCredito();

            try
            {
                nc.Identifier = (int)dr["Sequence"];
                nc.FolioOrigen = (string)dr["FolioOrigen"];
                nc.FolioDestino = (string)dr["FolioDestino"];
                nc.ConceptoDescuento = (DBNull.Value.Equals(dr["ConceptoDescuento"])) ? 0 : (int)dr["ConceptoDescuento"];
                nc.Canal = (string)dr["Canal"];
                nc.TipoDocumento = (string)dr["TipoDocumento"];
                nc.Valor = (int)dr["Valor"];
                nc.ConceptoDescuentoDetalle = (string)dr["ConceptoDescuentoDetalle"];
                nc.Comentario = (string)dr["Comentario"];
                nc.ComentarioDetalle = (string)dr["Comentario"];
                nc.Usuario = (string)dr["Usuario"];
                nc.Fecha = (DateTime)dr["Fecha"];
                nc.Cuenta = (string)dr["Cuenta"];
                nc.Email = (string)dr["Email"];
                nc.FolioSap = (string)dr["FolioSap"];
                nc.FolioPagoSAP = (string)dr["FolioPagoSAP"];
                nc.CardName = (string)dr["CardName"];
                nc.ClienteName = (string)dr["ClienteName"];
                nc.Importe = dr["Importe"] == DBNull.Value ? 0.0m : (decimal)dr["Importe"];
                nc.Vendedor = (string)dr["Vendedor"];

                string Coment = string.Empty;
                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        NotaCreditoItem nci = new NotaCreditoItem();

                        nci.ItemCode = (string)dr["ItemCode"];
                        nci.Cantidad = (int)dr["Cantidad"];
                        nci.Precio = (decimal)dr["Precio"];
                        nci.Descuento = (decimal)dr["Descuento"];
                        nci.CuentaContable = (string)dr["CuentaContable"];
                        nc.AddItem(nci);
                    }
                }
                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        Coment = Coment + (string)dr["Comentarios"] + "," + " ";
                    }
                    Coment.TrimEnd(' ');
                    Coment.TrimEnd(',');
                    nc.ComentarioDetalle += "<p>Detalle:</p> " + Coment + "<br/>";
                }
                // para agregar las imagenes
                if (dr.NextResult())
                {
                    while (dr.Read())
                    {
                        NotaCreditoImagen item = new NotaCreditoImagen();
                        item.UserName = (string)dr["UsuarioName"];
                        item.ImagenBase64 = (string)dr["Imagen"];
                        item.Estatus = (bool)dr["Estatus"];
                        item.RegistradoEl = (DateTime)dr["RegistradoEl"];
                        item.Tipo = (EvidenciaKind)(int)dr["Tipo"];
                        item.Extension = (string)dr["Extension"];
                        item.FileType = (string)dr["FileType"];
                        nc.AddImagen(item);
                    }
                }

            }
            catch (Exception ex)
            {

                throw new InvalidOperationException(ex.Message);
            }
            return nc;
        }

        protected override DbCommand PrepareAddStatement(NotaCredito item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddNotaCredito");
            this.Database.AddInParameter(command, "@Cliente", DbType.String, item.Cliente);
            this.Database.AddInParameter(command, "@FolioOrigen", DbType.String, item.FolioOrigen);
            this.Database.AddInParameter(command, "@FolioDestino", DbType.String, item.FolioDestino);
            this.Database.AddInParameter(command, "@TipoDocumento", DbType.String, item.TipoDocumento);
            this.Database.AddInParameter(command, "@Comentario", DbType.String, item.Comentario);
            this.Database.AddInParameter(command, "@Usuario", DbType.String, item.Usuario);
            this.Database.AddInParameter(command, "@ConceptoDescuento", DbType.Int32, item.ConceptoDescuento);
            this.Database.AddInParameter(command, "@Canal", DbType.String, item.Canal);
            this.Database.AddOutParameter(command, "@FolioSIE", DbType.Int32, 4);
            return command;
        }

        protected override void BeforeAddExecuted(DataContext<NotaCredito, int, NotaCreditoCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }

        protected override void CommandAddComplete(DataContext<NotaCredito, int, NotaCreditoCriteria> context)
        {
            context.Item.Identifier = (int)context.Command.Parameters["@FolioSIE"].Value;

            context.Command.Parameters.Clear();

            context.Command.CommandText = "prAddNotaCreditoItem";
            this.Database.AddInParameter(context.Command, "@NotaCredito", DbType.Int32, context.Item.Identifier);

            this.Database.AddInParameter(context.Command, "@ItemCode", DbType.String);
            this.Database.AddInParameter(context.Command, "@Cantidad", DbType.Int32);
            this.Database.AddInParameter(context.Command, "@Precio", DbType.Decimal);
            this.Database.AddInParameter(context.Command, "@Descuento", DbType.Decimal);
            this.Database.AddInParameter(context.Command, "@CuentaContable", DbType.String);

            DbParameter NotaCredito = context.Command.Parameters["@NotaCredito"];
            DbParameter ItemCode = context.Command.Parameters["@ItemCode"];
            DbParameter Cantidad = context.Command.Parameters["@Cantidad"];
            DbParameter Precio = context.Command.Parameters["@Precio"];
            DbParameter Descuento = context.Command.Parameters["@Descuento"];
            DbParameter CuentaContable = context.Command.Parameters["@CuentaContable"];

            foreach (var item in context.Item.Items)
            {
                ItemCode.Value = item.ItemCode;
                Cantidad.Value = item.Cantidad;
                Precio.Value = item.Precio;
                Descuento.Value = item.Descuento;
                CuentaContable.Value = item.CuentaContable;

                this.Database.ExecuteNonQuery(context.Command, context.Transaction);
                //la transaccion deberá esta abierta para cada insercion a la base de datos
            }

            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }

        protected override void CommandAddException(DataContext<NotaCredito, int, NotaCreditoCriteria> context, Exception ex)
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
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetNotaCredito");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, id);
            return cmd;
        }
        public NotaCredito FindNotaCreditoImagen(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetNotaCredito");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, id);
            this.Database.AddInParameter(cmd, "@CriteriaImagen", DbType.Int32, 1);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.Read())
            {
                return this.LoadItemImagenes(dr);
            }
            else
            {
                return null;
            }
        }


        protected override DbCommand PrepareUpdateStatement(NotaCredito item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(NotaCreditoCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            // filtro por tipo de usuario
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, criteria.TipoUsuario);
            if (criteria.Estatus != null)
            {
                this.Database.AddInParameter(cmd, "@Estatus", DbType.Int32, criteria.Estatus);
            }
            if (criteria.Inicio != null && criteria.Termino != null)
            {
                this.Database.AddInParameter(cmd, "@Inicio", DbType.DateTime, criteria.Inicio);
                this.Database.AddInParameter(cmd, "@Termino", DbType.DateTime, criteria.Termino);
            }
            if (!string.IsNullOrEmpty(criteria.Canal))
            {
                this.Database.AddInParameter(cmd, "@Canal", DbType.String, criteria.Canal);
            }
            if (!string.IsNullOrEmpty(criteria.TipoDocumento))
            {
                this.Database.AddInParameter(cmd, "@TipoDocumento", DbType.String, criteria.TipoDocumento);
            }                       
            return cmd;
        }

        public bool AddImagen(NotaCredito item)
        {
            try
            {
                foreach (var item1 in item.Imagenes)
                {
                    DbCommand command = this.Database.GetStoredProcCommand("prAddNotaCreditoImagen");
                    this.Database.AddInParameter(command, "@NotaCredito", DbType.Int32, item.Identifier);
                    this.Database.AddInParameter(command, "@RegistradoPor", DbType.String, item1.UserName);
                    this.Database.AddInParameter(command, "@Imagen64", DbType.String, item1.ImagenBase64);
                    this.Database.AddInParameter(command, "@Tipo", DbType.Int32, item1.Tipo);
                    this.Database.AddInParameter(command, "@Extension", DbType.String, item1.Extension);
                    this.Database.AddInParameter(command, "@FileType", DbType.String, item1.FileType);
                    this.Database.ExecuteNonQuery(command);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Ha fallado al guardar la imagen.", ex);
            }
        }

        public List<NotaCredito> GetNotaCreditoInventario(NotaCreditoCriteria criteria)
        {
            List<NotaCredito> lista = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindNotasCreditoCriteria");
            this.Database.AddInParameter(cmd, "@TipoDocumento", DbType.String, criteria.TipoDocumento);
            this.Database.AddInParameter(cmd, "@FacturaOrigen", DbType.Int32, criteria.FacturaOrigen);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                NotaCredito nc = new NotaCredito();
                nc.Identifier = (int)dr["Sequence"];
                nc.Cliente = (string)dr["Cliente"];
                nc.FolioOrigen = (string)dr["FolioOrigen"];
                nc.FolioDestino = (string)dr["FolioDestino"];
                nc.Valor = (int)dr["Valor"];
                nc.TipoDocumento = (string)dr["TipoDocumento"];
                nc.ConceptoDescuentoDetalle = (string)dr["ConceptoDescuentoDetalle"];
                nc.Comentario = (string)dr["Comentario"];
                nc.ComentarioDetalle = (string)dr["Comentario"];
                nc.Usuario = (string)dr["Usuario"];
                nc.Fecha = (DateTime)dr["Fecha"];
                nc.ConceptoDescuento = (DBNull.Value.Equals(dr["ConceptoDescuento"])) ? 0 : (int)dr["ConceptoDescuento"];
                nc.Cuenta = (string)dr["Cuenta"];
                nc.Canal = (string)dr["Canal"];
                nc.Email = (string)dr["Email"];
                nc.FolioSap = (string)dr["FolioSap"];
                nc.FolioPagoSAP = (string)dr["FolioPagoSAP"];
                nc.CardName = (string)dr["CardName"];
                nc.ClienteName = (string)dr["ClienteName"];
                lista.Add(nc);
            }
            cmd.Dispose();
            dr.Close();
            dr.Dispose();
            return lista;
        }

        public bool UpdateAnexo(int Id)
        {
            try
            {
                DbCommand command = this.Database.GetStoredProcCommand("prUpdateNotacreditoAnexo");
                this.Database.AddInParameter(command, "@Id", DbType.Int32, Id);
                this.Database.ExecuteNonQuery(command);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.StackTrace);
                return false;
            }
        }

        // notas de credito pendiente por anexar el anexo
        public List<NotaCredito> GetNotaCreditoPendienteAnexo(NotaCreditoCriteria criteria)
        {
            List<NotaCredito> lista = new List<NotaCredito>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindNotasCreditoCriteriaAnexo");
            this.Database.AddInParameter(cmd, "@Estatus", DbType.String, 4);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                NotaCredito nc = new NotaCredito();
                nc.Identifier = (int)dr["Sequence"];
                nc.Cliente = (string)dr["Cliente"];
                nc.FolioOrigen = (string)dr["FolioOrigen"];
                nc.FolioDestino = (string)dr["FolioDestino"];
                nc.Valor = (int)dr["Valor"];
                nc.TipoDocumento = (string)dr["TipoDocumento"];
                nc.Comentario = (string)dr["Comentario"];
                nc.Usuario = (string)dr["Usuario"];
                nc.Fecha = (DateTime)dr["Fecha"];
                nc.ConceptoDescuento = (DBNull.Value.Equals(dr["ConceptoDescuento"])) ? 0 : (int)dr["ConceptoDescuento"];
                nc.Cuenta = (string)dr["Cuenta"];
                nc.Canal = (string)dr["Canal"];
                nc.Email = (string)dr["Email"];
                nc.FolioSap = (string)dr["FolioSap"];
                nc.DocEntry = (int)dr["DocEntry"];
                nc.FolioPagoSAP = (string)dr["FolioPagoSAP"];
                nc.CardName = (string)dr["CardName"];
                nc.ClienteName = (string)dr["ClienteName"];
                lista.Add(nc);
            }
            cmd.Dispose();
            dr.Close();
            dr.Dispose();
            return lista;
        }



        #region ConceptosDescuento       
        public List<ConceptoDescuento.ConceptoDescuento> GetConceptoDescuento(string Cliente)
        {
            List<ConceptoDescuento.ConceptoDescuento> Detalle = new List<ConceptoDescuento.ConceptoDescuento>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetConceptosServicios");
            this.Database.AddInParameter(cmd, "@Cliente", DbType.String, Cliente);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                ConceptoDescuento.ConceptoDescuento item = new ConceptoDescuento.ConceptoDescuento();
                item.Identifier = (int)dr["Sequence"];
                item.Nombre = (string)dr["Nombre"];
                item.Descuento = (decimal)dr["Descuento"];
                item.Cuenta = (DBNull.Value.Equals(dr["Cuenta"])) ? " " : (string)dr["Cuenta"];
                item.Cliente = (DBNull.Value.Equals(dr["Cliente"])) ? " " : (string)dr["Cliente"];
                Detalle.Add(item);
            }
            return Detalle;
        }
        #endregion


        public bool AddNotaCreditoComentario(int NotaCredito, string Usuario, string Comentario, int Departamento)
        {
            try
            {

                DbCommand command = this.Database.GetStoredProcCommand("prAddNotaCreditoComentario");
                this.Database.AddInParameter(command, "@NotaCredito", DbType.Int32, NotaCredito);
                this.Database.AddInParameter(command, "@Usuario", DbType.String, Usuario);
                this.Database.AddInParameter(command, "@Comentario", DbType.String, Comentario);
                this.Database.AddInParameter(command, "@Departamento", DbType.Int32, Departamento);
                this.Database.ExecuteNonQuery(command);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region AlmacenPartidas

        public bool AddNotaCreditoArticuloAlmacenCode(NotaCreditoItemAlmacen item)
        {
            try
            {
                DbCommand command = this.Database.GetStoredProcCommand("prAddNotaCreditoItemAlmacenPartida");
                this.Database.AddInParameter(command, "@FolioNotacredito", DbType.Int32, item.FolioNotaCredito);
                this.Database.AddInParameter(command, "@ArticuloCode", DbType.String, item.Articulo);
                this.Database.AddInParameter(command, "@AlmacenCode", DbType.String, item.Almacen);
                this.Database.AddInParameter(command, "@Cantidad", DbType.Int32, item.Cantidad);
                this.Database.AddInParameter(command, "@Orden", DbType.Int32, item.Orden);
                this.Database.ExecuteNonQuery(command);

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool UpdateNotaCreditoArticuloAlmacenCode(NotaCreditoItemAlmacen item)
        {
            try
            {
                DbCommand command = this.Database.GetStoredProcCommand("prUpdateNotaCreditoItemAlmacenPartida");
                this.Database.AddInParameter(command, "@FolioNotacredito", DbType.Int32, item.FolioNotaCredito);
                this.Database.AddInParameter(command, "@ArticuloCode", DbType.String, item.Articulo);
                this.Database.AddInParameter(command, "@AlmacenCode", DbType.String, item.Almacen);
                this.Database.AddInParameter(command, "@Cantidad", DbType.Int32, item.Cantidad);
                this.Database.AddInParameter(command, "@Orden", DbType.Int32, item.Orden);
                this.Database.ExecuteNonQuery(command);

                return true;
            }
            catch
            {
                return false;
            }

        }


        public bool DeleteNotaCreditoArticuloAlmacenCode(NotaCreditoItemAlmacen item)
        {
            try
            {
                DbCommand command = this.Database.GetStoredProcCommand("prDeleteNotaCreditoItemAlmacenPartida");
                this.Database.AddInParameter(command, "@FolioNotacredito", DbType.Int32, item.FolioNotaCredito);
                this.Database.AddInParameter(command, "@ArticuloCode", DbType.String, item.Articulo);
                this.Database.AddInParameter(command, "@AlmacenCode", DbType.String, item.Almacen);
                this.Database.AddInParameter(command, "@Cantidad", DbType.Int32, item.Cantidad);
                this.Database.AddInParameter(command, "@Orden", DbType.Int32, item.Orden);
                this.Database.ExecuteNonQuery(command);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<NotaCreditoItemAlmacen> GetNotaCreditoItemAlmacen(int NotaCredito, string Articulo)
        {
            List<NotaCreditoItemAlmacen> lista = new List<NotaCreditoItemAlmacen>();

            DbCommand cmd = this.Database.GetStoredProcCommand("prGetNotaCreditoItemAlmacenPartida");
            this.Database.AddInParameter(cmd, "@NotaCredito", DbType.Int32, NotaCredito);
            this.Database.AddInParameter(cmd, "@Articulo", DbType.String, Articulo);

            IDataReader dr = this.Database.ExecuteReader(cmd);

            while (dr.Read())
            {
                NotaCreditoItemAlmacen item = new NotaCreditoItemAlmacen();
                item.Identifier = (int)dr["Identifier"];
                item.FolioNotaCredito = (int)dr["NotaCredito"];
                item.Articulo = (string)dr["Articulo"];
                item.Cantidad = (int)dr["Cantidad"];
                item.Almacen = (string)dr["Almacen"];
                item.Estatus = (int)dr["Estatus"];
                item.RegistradoEl = (DateTime)dr["FechaRegistro"];
                item.Orden = (int)dr["Orden"];

                lista.Add(item);
            }
            cmd.Dispose();
            dr.Close();
            dr.Dispose();
            return lista;
        }

        #endregion
    }
}
