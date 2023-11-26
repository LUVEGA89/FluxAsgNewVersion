using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ActividadesProductos
{
    public class ActividadesProductosManager : Catalog<ActividadProductos, int, ActividadProductoCriteria>
    {
        ComentarioCatalog comentarioCatalog = new ComentarioCatalog();
        public ActividadesProductosManager()
        {

        }
        public ActividadesProductosManager(string Database)
            : base(Database)
        {

        }
        protected override string FindPagedItemsProcedure
        {
            get { return "prFindActividadProductos"; }
        }
        //----------------- clases agregadas JIMERU  --------------------------//
       
        protected override DbCommand PrepareFindPagedItemsStatement(ActividadProductoCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            this.Database.AddInParameter(cmd, "@Producto", DbType.String, criteria.Producto);
            this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, criteria.Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, criteria.Al);
            this.Database.AddInParameter(cmd, "@Permiso", DbType.Int32, criteria.PermisosRol);
            return cmd;
        }

        protected override ActividadProductos LoadItem(IDataReader dr)
        {
            ActividadProductos actividadesProductos = new ActividadProductos();
            actividadesProductos.Identifier = (int)dr["Identificador"];
            actividadesProductos.Titulo = (string)dr["Titulo"];
            actividadesProductos.Comentario = (string)dr["Comentario"];
            actividadesProductos.Producto = (string)dr["Producto"];
            actividadesProductos.RegistradoEl = dr["RegistradoEl"] == DBNull.Value ? null : (DateTime?)dr["RegistradoEl"];
            actividadesProductos.RegistradoPor = (string)dr["RegistradoPor"];
            actividadesProductos.PermisosRol = (int)dr["PermisosRol"];

            actividadesProductos.ListaComentarios = comentarioCatalog.FindPagedItems(
                new ComentarioCriteria()
                {
                    Actividad = (int)dr["Identificador"]
                }).ToList();


            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    ActividadProductosFotos item = new ActividadProductosFotos()
                    {
                        Identifier = (int)dr["Identificador"],
                        Actividad = (int)dr["Actividad"],
                        Foto = (string)dr["Foto"],
                        Estatus = Convert.ToBoolean(dr["Estatus"])
                    };
                    actividadesProductos.AddItemImagen(item);
                }
            }
            return actividadesProductos;
            //throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prGetActividadProductos");
            this.Database.AddInParameter(command, "@Id", DbType.Int16, id);
            return command;
        }

        protected override DbCommand PrepareAddStatement(ActividadProductos item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddActividadProductos");
            this.Database.AddInParameter(command, "@Titulo",DbType.String,item.Titulo);
            this.Database.AddInParameter(command, "@Comentario",DbType.String,item.Comentario);
            this.Database.AddInParameter(command, "@Producto",DbType.String,item.Producto);
            this.Database.AddInParameter(command, "@RegistradoPor", DbType.String, item.RegistradoPor);
            this.Database.AddInParameter(command, "@PermisosRol", DbType.Int32, item.PermisosRol);
            this.Database.AddOutParameter(command, "@IdActividad", DbType.Int32,8);
            return command;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }
        
        protected override DbCommand PrepareUpdateStatement(ActividadProductos item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prUpdateActividadProductos");
            this.Database.AddInParameter(command, "@Id",DbType.Int32,item.Identifier);
            this.Database.AddInParameter(command, "@PermisosRol", DbType.Int32, item.PermisosRol);
            return command;
        }
        //antes de agregar los datos se establece la conexion a bds
        protected override void BeforeAddExecuted(DataContext<ActividadProductos, int, ActividadProductoCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }
        // metodos para seguimiento a productos
        protected override void CommandAddComplete(DataContext<ActividadProductos, int, ActividadProductoCriteria> context)
        {
            //se recupera el valor de la actividad llevada a cabo en el Add
            context.Item.Identifier = (int)context.Command.Parameters["@IdActividad"].Value;
            context.Command.Parameters.Clear();
            context.Command.CommandText = "prAddActividadProductosFotos";
            this.Database.AddInParameter(context.Command, "@Actividad", DbType.Int32, context.Item.Identifier);
            this.Database.AddInParameter(context.Command, "@Foto", DbType.String);
            DbParameter Actividad = context.Command.Parameters["@Actividad"];
            DbParameter Foto = context.Command.Parameters["@Foto"];

            foreach (var item in context.Item.ListaImagenes)
            {
                Foto.Value = item.Foto;
                this.Database.ExecuteNonQuery(context.Command, context.Transaction);
                //la transaccion deberá esta abierta para cada insercion a la base de datos
            }
            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }
        //Add ProductosCoparacion
        public bool agregarSKUcomparacion(List<ActividadProductosComparacion> productosComparacions)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            foreach (var item in productosComparacions)
            {
                DbCommand command = this.Database.GetStoredProcCommand("prAddActividadProductosComparacion");
                command.Connection = connection;
                this.Database.AddInParameter(command, "@Actividad", DbType.Int32, item.Actividad);
                this.Database.AddInParameter(command, "@PrecioLocal", DbType.Decimal, item.PrecioLocal);
                this.Database.AddInParameter(command, "@TipoPrecio", DbType.String, item.TipoPrecio);
                this.Database.AddInParameter(command, "@MinPiezas", DbType.Int32, item.MinPiezas);
                this.Database.AddInParameter(command, "@PrecioCompetencia", DbType.Decimal, item.PrecioCompetencia);
                this.Database.AddInParameter(command, "@Modelo", DbType.String, item.Modelo);
                this.Database.AddInParameter(command, "@Marca", DbType.String, item.Marca);
                command.ExecuteNonQuery();
            }
            return true;
        }
        //manejo de carga de fotos actividad
        public IList<ActividadProductosFotos> GetFotosActividad(int idActividad)
        {
            List<ActividadProductosFotos> detalle = new List<ActividadProductosFotos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetActividadProductosImagen");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, idActividad);
            IDataReader dr = this.Database.ExecuteReader(cmd);

            while (dr.Read())
            {
                detalle.Add(new ActividadProductosFotos
                {
                    Foto = (string)dr["Foto"],
                    Estatus = (bool)dr["Estatus"],
                    Actividad = (int)dr["Actividad"]
                });
            }
            return detalle;
        }
        //obtiene los titulos de la actividad
        public IList<TitulosActividad> GetTitulosActividad(int Id)
        {
            List<TitulosActividad> titulosActividad = new List<TitulosActividad>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetTitulosActividad");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, Id);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                titulosActividad.Add(new TitulosActividad {
                    Titulo = (string)dr["Titulo"],
                    Descripcion = (string)dr["Descripcion"]
            });
            }
            return titulosActividad;
        }
        //Obtiene las categorias de los SKU 
        public IList<CategoriasSKUProductos> GetCategoriasSKU(int Id)
        {
            List<CategoriasSKUProductos> categorias = new List<CategoriasSKUProductos>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetCategoriasSKU");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, Id);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                categorias.Add(new CategoriasSKUProductos
                {
                    Id=(int)dr["Id"],
                    Name=(string)dr["Name"],
                    ParentCategoryId=(int)dr["ParentCategoryId"]
                });
            }
            return categorias;
        }
        
        public IList<SKU> FindSKUProductos(string Texto)
        {
            List<SKU> Detalle = new List<SKU>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindSKUProductos");
            this.Database.AddInParameter(cmd, "@Texto", DbType.String, Texto);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new SKU
                {
                    Code = (string)dr["Code"],
                    Articulo = (string)dr["Articulo"],
                    Familia = (string)dr["Familia"],
                    Coincidencia = (string)dr["Coincidencia"],
                    Auxiliar = (string)dr["Auxiliar"]
                });
            }
            return Detalle;
        }
        
        public PriceProducto GetPriceProducto(string ProductoSKU)
        {
            PriceProducto price = new PriceProducto();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPriceProducto");
            this.Database.AddInParameter(cmd, "@Producto", DbType.String, ProductoSKU);
            IDataReader dr = this.Database.ExecuteReader(cmd);

            while (dr.Read())
            {
                price.PrecioLocal = (decimal)dr["PrecioLocal"];
                price.PrecioMayoreoTiendas = (decimal)dr["PrecioMayoreoTienda"];
                price.PrecioMenudeoTiendas = (decimal)dr["PrecioMenudeoTienda"];
                price.PrecioDistribuidorLocal = (decimal)dr["PrecioDistribuidorLocal"];
                price.PrecioDistribuidorForanea = (decimal)dr["PrecioDistribuidorForanea"];
                price.StockCedis = (int)dr["StockCedis"];
                price.TipoPrecio = (string)dr["TipoPrecio"];
            }
            //price.PrecioLocal = (decimal)dr["PrecioLocal"];
            return price;
        }

        public SKU GetDetalleSKU(string Code)
        {
            SKU Detalle = new SKU();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleSKUProductos");
            this.Database.AddInParameter(cmd, "@Code", DbType.String, Code);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Code = (string)dr["Code"];
                Detalle.Articulo = (string)dr["Articulo"];
                Detalle.Familia = (string)dr["Familia"];
                Detalle.Auxiliar = (string)dr["Auxiliar"];
            }
            return Detalle;
        }
    }
}
