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
    public class NuevoSKUSugeridoCatalog : Catalog<NuevoSKUSugerido, int, NuevoSKUSugeridoCriteria>
    {
        SKUComparacionCatalog SKUComparacion = new SKUComparacionCatalog();
        public NuevoSKUSugeridoCatalog()
            : base()
        {
        }
        public NuevoSKUSugeridoCatalog(string database)
            : base(database)
        {
        }

        protected override string FindPagedItemsProcedure
        {
            get { return "prFindNuevoSKUSugerido"; }
        }

        protected override NuevoSKUSugerido LoadItem(IDataReader dr)
        {
            NuevoSKUSugerido nuevoSKU = new NuevoSKUSugerido();
            nuevoSKU.Identifier = (int)dr["Identificador"];
            nuevoSKU.SKUCode = (string)dr["SKUCode"];
            nuevoSKU.SKUName = (string)dr["SKUName"];
            nuevoSKU.FechaRegistro = (DateTime)dr["FechaRegistro"];
            nuevoSKU.RegistradoPor = (string)dr["RegistradoPor"];
            nuevoSKU.Empaque = (string)dr["TipoEmpaque"];
            nuevoSKU.Marca = (string)dr["Marca"];
            nuevoSKU.Categoria = (string)dr["Categoria"];
            nuevoSKU.Subcategoria = (string)dr["Subcategoria"];
            nuevoSKU.Auxiliar = (string)dr["Auxiliar"];
            nuevoSKU.Estatus = (bool)dr["Estatus"];
            //dr["RegistradoEl"] == DBNull.Value ? null : (DateTime?)dr["RegistradoEl"];
            nuevoSKU.ListaImagenes = dr["Identificador"] == DBNull.Value ? null : this.GetImagenesNuevoSKU((int)dr["Identificador"]).ToList();
            nuevoSKU.ListaPreciosCompetencias = dr["Identificador"] == DBNull.Value ? null :  this.GetPreciosCompetenciaNuevoSKU((int)dr["Identificador"]).ToList();
            return nuevoSKU;
        }

        protected override void BeforeAddExecuted(DataContext<NuevoSKUSugerido, int, NuevoSKUSugeridoCriteria> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }
        protected override DbCommand PrepareAddStatement(NuevoSKUSugerido item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddNuevoSKUSugerido");
            this.Database.AddInParameter(command, "@SKUCode", DbType.String, item.SKUCode);
            this.Database.AddInParameter(command, "@SKUName", DbType.String, item.SKUName);
            this.Database.AddInParameter(command, "@RegistradoPor", DbType.String, item.RegistradoPor);
            this.Database.AddInParameter(command, "@Marca", DbType.String, item.Marca);
            this.Database.AddInParameter(command, "@Categoria", DbType.String, item.Categoria);
            this.Database.AddInParameter(command, "@Subcategoria", DbType.String, item.Subcategoria);
            this.Database.AddInParameter(command, "@Empaque", DbType.String, item.Empaque);
            this.Database.AddOutParameter(command, "@IdNuevoSKU", DbType.Int32, 8);
            return command;
        }

        protected override void CommandAddComplete(DataContext<NuevoSKUSugerido, int, NuevoSKUSugeridoCriteria> context)
        {
            context.Item.Identifier = (int)context.Command.Parameters["@IdNuevoSKU"].Value;
            context.Command.Parameters.Clear();
            context.Command.CommandText = "prAddImagenNuevoSKU";
            this.Database.AddInParameter(context.Command, "@IdSKU", DbType.Int32, context.Item.Identifier);
            this.Database.AddInParameter(context.Command, "@Imagen", DbType.String);
            DbParameter IdSKU = context.Command.Parameters["@IdSKU"];
            DbParameter Imagen = context.Command.Parameters["@Imagen"];
            foreach (var item in context.Item.ListaImagenes)
            {
                Imagen.Value = item.Imagen;
                this.Database.ExecuteNonQuery(context.Command, context.Transaction);
            }
            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prGetNuevoSKUSugerido");
            this.Database.AddInParameter(command, "@Id", DbType.Int16, id);
            return command;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(NuevoSKUSugeridoCriteria Criteria)
        {
            DbCommand command = base.PrepareFindPagedItemsStatement(Criteria);
            if (Criteria.Del.HasValue)
            {
                this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            }

            if (Criteria.Al.HasValue)
            {
                this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            }

            if (!string.IsNullOrEmpty(Criteria.SKUCode))
            {
                this.Database.AddInParameter(command, "@SKUCode", DbType.String, Criteria.SKUCode);
            }
            return command;
        }

        protected override DbCommand PrepareUpdateStatement(NuevoSKUSugerido item)
        {
            throw new NotImplementedException();
        }

        public IList<SKU> FindSKUProductosSugeridos(string Texto)
        {
            List<SKU> Detalle = new List<SKU>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindSKUProductosNuevos");
            this.Database.AddInParameter(cmd, "@Texto", DbType.String, Texto);
            cmd.CommandTimeout = 0;
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

        public bool AddPreciosCompetencia(PreciosCompetencia preciosCompetencias)
        {
            try
            {
                DbConnection connection = this.Database.CreateConnection();
                connection.Open();
                DbCommand command = this.Database.GetStoredProcCommand("prAddPreciosCompetencia");
                command.Connection = connection;
                this.Database.AddInParameter(command, "@IdSKU", DbType.Int32, preciosCompetencias.IdSKU);
                this.Database.AddInParameter(command, "@TipoPrecio", DbType.String, preciosCompetencias.TipoPrecio);
                this.Database.AddInParameter(command, "@Precio", DbType.Decimal, preciosCompetencias.Precio);
                this.Database.AddInParameter(command, "@NumPiezas", DbType.Int32, preciosCompetencias.NumPiezas);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar datos", ex);
                return false;
            }
        }

        public CategoriasSKUProductos GetCategoriasSKUParaDetalle(int Id)
        {
            CategoriasSKUProductos categorias = new CategoriasSKUProductos();
            DbCommand command = this.Database.GetStoredProcCommand("prGetCategoriaSKUParaDetalle");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, Id);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                categorias.Id = (int)dr["Id"];
                categorias.Name = (string)dr["Name"];
                categorias.ParentCategoryId = (int)dr["ParentCategoryId"];
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();
            return categorias;
        }

        public IList<ImagenesNuevoSKU> GetImagenesNuevoSKU(int id)
        {
            List<ImagenesNuevoSKU> imagenesNuevoSKUs = new List<ImagenesNuevoSKU>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetImagenesSKUNuevos");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, id);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                ImagenesNuevoSKU item = new ImagenesNuevoSKU();
                item.Identifier = (int)dr["Identificador"];
                item.Imagen = (string)dr["Imagen"];
                imagenesNuevoSKUs.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();
            return imagenesNuevoSKUs;
        }

        public IList<PreciosCompetencia> GetPreciosCompetenciaNuevoSKU(int id)
        {
            List<PreciosCompetencia> preciosCompetencias = new List<PreciosCompetencia>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetPreciosCompetencia");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, id);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                PreciosCompetencia item = new PreciosCompetencia();
                item.Identifier = (int)dr["Identificador"];
                item.TipoPrecio = (string)dr["TipoPrecio"];
                item.Precio = (decimal)dr["Precio"];
                item.NumPiezas = (int)dr["NumPiezas"];
                preciosCompetencias.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();
            return preciosCompetencias;
        }
    }
}
