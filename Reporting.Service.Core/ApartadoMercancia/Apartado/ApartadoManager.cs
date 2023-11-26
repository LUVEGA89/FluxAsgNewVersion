using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ApartadoMercancia.Apartado
{
    public class ApartadoManager : Catalog<Apartado, int, ApartadoCriteria>
    {
        public ApartadoManager()
        :base()
        {

        }

        protected override string FindPagedItemsProcedure => "prFindApartados";

        protected override Apartado LoadItem(IDataReader dr)
        {
            Apartado nuevo = new Apartado();
            nuevo.Identifier = int.Parse(dr["idApartado"].ToString());
            nuevo.cliente = (string)dr["cliente"];
            nuevo.agente = (string)dr["Agente"];
            nuevo.agente_email = (string)dr["Email"];
            nuevo.fechaApartado = (DateTime)dr["fechaApartado"];
            nuevo.fechaLiberacion = (DateTime)dr["fechaLiberacion"];
            nuevo.motivo = (string)dr["motivo"];
            nuevo.folioSAP = int.Parse(dr["folioSAP"].ToString());
            nuevo.status = int.Parse(dr["estatus"].ToString());
            nuevo.canal = (string)dr["canal"];
            return nuevo;
        }

        protected override void BeforeAddExecuted(DataContext<Apartado, int, ApartadoCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }

        protected override DbCommand PrepareAddStatement(Apartado item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddApartado");
            this.Database.AddInParameter(command, "@Cliente", DbType.String, item.cliente);
            this.Database.AddInParameter(command, "@Agente", DbType.String, item.agente);
            this.Database.AddInParameter(command, "@FechaApartado", DbType.DateTime, item.fechaApartado);
            this.Database.AddInParameter(command, "@FechaLiberacion", DbType.DateTime, item.fechaLiberacion);
            this.Database.AddInParameter(command, "@Motivo", DbType.String, item.motivo);
            this.Database.AddInParameter(command, "@Canal", DbType.String, item.canal);
            this.Database.AddOutParameter(command, "@Folio", DbType.Int32, 4);
            return command;
        }

        protected override void CommandAddComplete(DataContext<Apartado, int, ApartadoCriteria> context)
        {

            int apartado_id = (int)context.Command.Parameters["@Folio"].Value;
            //Inserta info de archivos
            context.Command.Parameters.Clear();
            context.Command.CommandText = "prAddApartadoEvidencia";
            this.Database.AddInParameter(context.Command, "@Ruta", DbType.String);
            this.Database.AddInParameter(context.Command, "@Archivo", DbType.String);
            this.Database.AddInParameter(context.Command, "@Ext", DbType.String);
            this.Database.AddInParameter(context.Command, "@Date", DbType.DateTime);
            this.Database.AddInParameter(context.Command, "@subPath", DbType.String);
            this.Database.AddInParameter(context.Command, "@Usuario", DbType.String);
            this.Database.AddInParameter(context.Command, "@idApart", DbType.Int32, apartado_id);
            DbParameter ruta = context.Command.Parameters["@Ruta"];
            DbParameter archivo = context.Command.Parameters["@Archivo"];
            DbParameter ext = context.Command.Parameters["@Ext"];
            DbParameter date = context.Command.Parameters["@Date"];
            DbParameter subPath = context.Command.Parameters["@subPath"];
            DbParameter usuario = context.Command.Parameters["@Usuario"];
            foreach (var file in context.Item.archivos)
            {
                ruta.Value = file.path;
                archivo.Value = file.fileName;
                ext.Value = file.fileExt;
                date.Value = file.dateInsert;
                subPath.Value = file.subPath;
                usuario.Value = file.usuario;
                this.Database.ExecuteNonQuery(context.Command, context.Transaction);
                //la transaccion deberá esta abierta para cada insercion a la base de datos
            }

            //Inserta info de productos
            context.Command.Parameters.Clear();
            context.Command.CommandText = "prAddApartadoMercancia";
            this.Database.AddInParameter(context.Command, "@SKU", DbType.String);
            this.Database.AddInParameter(context.Command, "@Piezas", DbType.Int16);
            this.Database.AddInParameter(context.Command, "@idApart", DbType.Int32, apartado_id);
            DbParameter SKU = context.Command.Parameters["@SKU"];
            DbParameter Piezas = context.Command.Parameters["@Piezas"];
            foreach (var product in context.Item.productos)
            {
                SKU.Value = product.SKU;
                Piezas.Value = product.piezas;
                this.Database.ExecuteNonQuery(context.Command, context.Transaction);
                //la transaccion deberá esta abierta para cada insercion a la base de datos
            }

            //Inserta archivos en la ruta especificada
            foreach (var evidencias in context.Item.archivos)
            {
                //Se guarda el archivo en el servidor de massriv
                var fileGuardado = evidencias.file;
                var rutaGuardado = evidencias.path + "\\" + evidencias.subPath;
                var filename = evidencias.fileName + "." + evidencias.fileExt;
                var path = Path.Combine(Path.GetFullPath(rutaGuardado), filename);
                fileGuardado.SaveAs(path);
            }
            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }

        protected override void CommandAddException(DataContext<Apartado, int, ApartadoCriteria> context, Exception ex)
        {
            if (this.CurrenctTransaction == null)
            {
                context.Transaction.Rollback();
                if (context.Transaction.Connection.State == ConnectionState.Open)
                {
                    context.Transaction.Connection.Close();
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
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetApartado");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int16, id);
            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(ApartadoCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            if(criteria.coincidenciaCanal != "")
                this.Database.AddInParameter(cmd, "@Canal", DbType.String, criteria.coincidenciaCanal);
            if (criteria.Inicio.ToShortDateString() != "01/01/0001" && criteria.Fin.ToShortDateString() != "01/01/0001")
            {
                this.Database.AddInParameter(cmd, "@fecIni", DbType.Date, criteria.Inicio);
                this.Database.AddInParameter(cmd, "@fecFin", DbType.Date, criteria.Fin);
            }
            this.Database.AddInParameter(cmd, "@Estado", DbType.Int16, criteria.estado);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Apartado item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpApartado");
            this.Database.AddInParameter(cmd, "@id", DbType.Int16, item.Identifier);
            if (item.status != 0)
                this.Database.AddInParameter(cmd, "@status", DbType.Int16, item.status);
            if (item.fechaLiberacion.ToShortDateString() != "01/01/0001")
                this.Database.AddInParameter(cmd, "@liberacion", DbType.Date, item.fechaLiberacion);
            return cmd;
        }

        public List<Papeleria.Papeleria> buscarProducto(string Producto, string Almacen)
        {
            List<Papeleria.Papeleria> Detalle = new List<Papeleria.Papeleria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prgetMercancia");
            this.Database.AddInParameter(cmd, "@Producto", DbType.String, Producto);
            if(Almacen!="")
                this.Database.AddInParameter(cmd, "@Almacen", DbType.String, Almacen);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Papeleria.Papeleria
                {
                    ItemCode = (string)dr["ItemCode"].ToString(),
                    ItemName = (string)dr["ItemName"].ToString(),
                    Stock = DBNull.Value.Equals(dr["Stock"]) ? 0 : (decimal)dr["Stock"]
                });
            }
            return Detalle;
        }

        public List<ApartadoProducto> findMercancia(int idApartado)
        {
            List<ApartadoProducto> mercancia = new List<ApartadoProducto>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindMercancia");
            this.Database.AddInParameter(cmd, "@idApartado", DbType.Int16, idApartado);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            cmd.Dispose();
            while (dr.Read())
            {
                ApartadoProducto nuevo = new ApartadoProducto();
                nuevo.Identifier = (int)dr["idMercancia"];
                nuevo.SKU = (string)dr["SKU"];
                nuevo.piezas = int.Parse(dr["piezas"].ToString());
                nuevo.piezasLiberadas = int.Parse(dr["piezas_Liberadas"].ToString());
                nuevo.piezasDisponibles = int.Parse(dr["piezas_Disponibles"].ToString());
                mercancia.Add(nuevo);
            }
            if (mercancia.Count != 0)
                return mercancia;
            else
                return null;
        }

        public bool updateWorkflow(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpApartadoWorkflow");
            this.Database.AddInParameter(cmd, "@Guid", DbType.Int16, id);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }

        //Función para obtener el listado de anexos del envio
        public List<Evidencia> GetEvidencias(int id)
        {
            List<Evidencia> anexos = new List<Evidencia>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetEvidencias");
            this.Database.AddInParameter(cmd, "@ID", DbType.Int16, id);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Evidencia nuevo = new Evidencia();
                nuevo.path = (string)dr["path"];
                nuevo.fileName = (string)dr["archivo"];
                nuevo.fileExt = (string)dr["ext"];
                nuevo.subPath = (string)dr["subPath"];

                anexos.Add(nuevo);
            }
            return anexos;
        }

        public bool UpdateMercancia(int idApartado, List<int> listaIds, List<int> listaCantidades)
        {
            bool respuesta = true;
            DbConnection connection = this.Database.CreateConnection();//Se crea la conexión 
            connection.Open();//Se abre la conexión
            DbTransaction transaccion = connection.BeginTransaction();//Se inicia la transacción
            try
            {
                DbCommand cmd = connection.CreateCommand();//Se crea un comando
                cmd.CommandType = CommandType.StoredProcedure;//Se le asigna que es tipo stored procedure
                cmd.Parameters.Clear();//Limpiar los parametros
                for (int i = 0; i < listaIds.Count; i++)
                {
                    if (listaCantidades[i] >= 1)//Si al menos se quiere liberar una pieza
                    {
                        cmd.CommandText = "prUpApartadoMercancia";//Se le pasa el nombre del store
                        this.Database.AddInParameter(cmd, "@idMercancia", DbType.Int16, listaIds[i]);//parametros
                        this.Database.AddInParameter(cmd, "@cantidad", DbType.Int16, listaCantidades[i]);//parametros
                        cmd.Transaction = transaccion;//Se asigna la transacción al comando
                        if (cmd.ExecuteNonQuery() == 1)
                            cmd.Parameters.Clear();//Se limpian los parametros para ejecutarlo nuevamente
                        else
                        {
                            transaccion.Rollback();//Se hace un rollback si no se ejecuta alguno
                            respuesta = false;//Regresa un falso
                        }
                    }
                    //Aquí se hace el update a la tabla apartado
                    cmd.CommandText = "prUpApartado";//Se le pasa el nombre del store
                    this.Database.AddInParameter(cmd, "@id", DbType.Int16, idApartado);//parametros
                    this.Database.AddInParameter(cmd, "@status", DbType.Int16, 8);//parametros
                    cmd.Transaction = transaccion;
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        transaccion.Commit();//Se guardan los cambios
                    }
                    else
                    {
                        transaccion.Rollback();//Se hace un rollback si no se ejecuta alguno
                        respuesta = false;//Regresa un falso
                    }
                }
            }
            catch(Exception ex)
            {
                string error = ex.Message;
                transaccion.Rollback();
                respuesta = false;
            }
            finally
            {
                connection.Close();
            }
            return respuesta;
        }
    }
}
