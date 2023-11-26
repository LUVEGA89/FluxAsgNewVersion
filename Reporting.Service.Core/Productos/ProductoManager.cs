using Reporting.Service.Core.Clientes;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reporting.Service.Core.Productos.Llegadas;

namespace Reporting.Service.Core.Productos
{
    public class ProductoManager : DataRepository
    {
        public ProductoManager()
            :base()
        {

        }

        public ProductoManager(string cadena)
            : base(cadena)
        {

        }
        public List<Productos> GetArticulos8020(DateTime Del, DateTime Al)
        {
            List<Productos> Detalle = new List<Productos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGet8020TiendasSAPHistorialClasificado");
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, 2);
            this.Database.AddInParameter(cmd, "@TipoCorpo", DbType.Int32, 1);
            this.Database.AddInParameter(cmd, "@Only8020", DbType.Int32, 1);
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Productos
                {
                    Sku = (string)dr["Sku"],
                    PeriodoActual = (decimal)dr["Periodo Actual"],
                    PeriodoAnterior = (decimal)dr["Periodo Anterior"],
                    Crecimiento = (decimal)dr["% Crecimiento"],
                });
            }
            return Detalle;
        }
        public DataTable GetArticulos8020Excel(DateTime Del, DateTime Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGet8020TiendasSAPHistorialClasificado");
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, 2);
            this.Database.AddInParameter(cmd, "@TipoCorpo", DbType.Int32, 1);
            this.Database.AddInParameter(cmd, "@Only8020", DbType.Int32, 1);
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;

        }
        public List<Productos> GetDecrementoArticulos(DateTime Del, DateTime Al)
        {
            List<Productos> Detalle = new List<Productos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDecrementoVentaArticulos");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Productos
                {
                    Sku = (string)dr["SKU"],
                    PeriodoActual = (decimal)dr["CantidadPeriodoActual"],
                    Monto = (decimal)dr["Monto Periodo Actual"],
                    PeriodoAnterior = (decimal)dr["CantidadPeriodoAnterior"],
                    MontoAnterior = (decimal)dr["Monto Periodo Anterior"],
                    Stock = (decimal)dr["Stock"],
                    Estado = (string)dr["Status"]
                });
            }
            return Detalle;
        }
        public List<Clientes.Cliente> GetPiezasPorCliente(DateTime Del, DateTime Al)
        {
            List<Clientes.Cliente > Detalle = new List<Clientes.Cliente>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetVentasPiezasCliente");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Clientes.Cliente
                {
                    Nombre = (string)dr["Cliente"],
                    Cantidad = (decimal)dr["Cantidad"],
                    Monto = (decimal)dr["Monto"],
                });
            }
            return Detalle;
        }
        public DataTable GetPiezasPorClienteExcel(DateTime Del, DateTime Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetVentasPiezasCliente");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;
        }
        public List<Productos> GetPiezasPorSku(DateTime Del, DateTime Al)
        {
            List<Productos> Detalle = new List<Productos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetVentasPiezasSku");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Productos
                {
                    Sku = (string)dr["SKU"],
                    Cantidad = (decimal)dr["Cantidad"],
                    Monto = (decimal)dr["Monto"],
                });
            }
            return Detalle;
        }
        public DataTable GetPiezasPorSkuExcel(DateTime Del, DateTime Al)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetVentasPiezasSku");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;
        }
        public DataTable GetDecrementoArticulosExcel(DateTime Del, DateTime Al)
        {
            List<Productos> Detalle = new List<Productos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDecrementoVentaArticulos");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;
        }
        public List<Productos> GetDecrementoCliente(DateTime Del, DateTime Al)
        {
            List<Productos> Detalle = new List<Productos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDecrementoVentaCliente");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Productos
                {
                    Cliente = (string)dr["Cliente"],
                    PeriodoActual = (decimal)dr["Actual"],
                    PeriodoAnterior = (decimal)dr["Anterior"],
                    Estado = (string)dr["Estado"],
                    Tipo = (string)dr["Tipo"],
                    Participacion = (decimal)dr["Porcentaje"],
                });
            }
            return Detalle;
        }
        public DataTable GetDecrementoClienteExcel(DateTime Del, DateTime Al)
        {
            List<Productos> Detalle = new List<Productos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDecrementoVentaCliente");
            this.Database.AddInParameter(cmd, "@Del", DbType.Date, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.Date, Al);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);

            return dt;
        }

        //AQUI EMPIEZAN LOS METODOS DE EL MODIFICADOR DE PRECIOS
        public List<ProductosSap> CoreOtherListPriceSearch(string ItemCode)
        {
            List<ProductosSap> Detalle = new List<ProductosSap>();
            DbCommand cmd = this.Database.GetStoredProcCommand("uspGetListPriceSearch");
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                ProductosSap item = new ProductosSap();

                item.ItemCode = (string)dr["ItemCode"];
                item.NombreProducto = (string)dr["NombreProducto"];
                item.TipoPrecio = (string)dr["TipoPrecio"];
                item.FamSIAT = (string)dr["FamSIAT"];
                item.SequenceFamilia = (int)dr["SequenceFamilia"];
                

                item.PzaMay = (int)dr["PzaMay"];
                item.PzaMayCorp = (int)dr["PzaMayCorp"];

                item.CMex = (decimal)dr["CMex"];

                item.STGP10 = (decimal)dr["STGP10"];


                item.Lista40 = (decimal)dr["Lista40"];
                item.Lista40_iva = (decimal)dr["Lista40_iva"];
                item.Mbvsstgp_40 = (decimal)dr["Mbvsstgp_40"];
                item.Fac40 = (decimal)dr["Fac40"];
                item.Lista39 = (decimal)dr["Lista39"];
                item.Lista39_iva = (decimal)dr["Lista39_iva"];
                item.Fac39 = (decimal)dr["Fac39"];
                item.Lista38 = (decimal)dr["Lista38"];
                item.Lista38_iva = (decimal)dr["Lista38_iva"];
                item.Fac38 = (decimal)dr["Fac38"];

                item.Lista15 = (decimal)dr["Lista15"];
                item.Lista15_iva = (decimal)dr["Lista15_iva"];
                item.Mbvsstgp_15 = (decimal)dr["Mbvsstgp_15"];
                item.Fac15 = (decimal)dr["Fac15"];

                item.Lista3 = (decimal)dr["Lista3"];
                item.Lista3_iva = (decimal)dr["Lista3_iva"];
                item.Mbvsstgp_3 = (decimal)dr["Mbvsstgp_3"];
                item.Fac3 = (decimal)dr["Fac3"];

                item.Lista2 = (decimal)dr["Lista2"];
                item.Lista2_iva = (decimal)dr["Lista2_iva"];
                item.Mbvsstgp_2 = (decimal)dr["Mbvsstgp_2"];
                item.Fac2 = (decimal)dr["Fac2"];

                item.Lista6 = (decimal)dr["Lista6"];
                item.Lista6_iva = (decimal)dr["Lista6_iva"];
                item.Mbvsstgp_6 = (decimal)dr["Mbvsstgp_6"];
                item.Fac6 = (decimal)dr["Fac6"];

                item.PMin33 = (decimal)dr["PMin33"];
                item.Lista33 = (decimal)dr["Lista33"];
                item.Lista33_iva = (decimal)dr["Lista33_iva"];
                item.Mbvsstgp_33 = (decimal)dr["Mbvsstgp_33"];

                item.PMin22 = (decimal)dr["PMin22"];
                item.Lista22 = (decimal)dr["Lista22"];
                item.Lista22_iva = (decimal)dr["Lista22_iva"];
                item.Mbvsstgp_22 = (decimal)dr["Mbvsstgp_22"];

                item.PMin42 = (decimal)dr["PMin42"];
                item.Lista42 = (decimal)dr["Lista42"];
                item.Lista42_iva = (decimal)dr["Lista42_iva"];
                item.Mbvsstgp_42 = (decimal)dr["Mbvsstgp_42"];

                item.PMin25 = (decimal)dr["PMin25"];
                item.Lista25 = (decimal)dr["Lista25"];
                item.Lista25_iva = (decimal)dr["Lista25_iva"];
                item.Mbvsstgp_25 = (decimal)dr["Mbvsstgp_25"];

                item.PMin14 = (decimal)dr["PMin14"];
                item.Lista14 = (decimal)dr["Lista14"];
                item.Lista14_iva = (decimal)dr["Lista14_iva"];
                item.Mbvsstgp_14 = (decimal)dr["Mbvsstgp_14"];

                item.PMin48 = (decimal)dr["PMin48"];
                item.Lista48 = (decimal)dr["Lista48"];
                item.Lista48_iva = (decimal)dr["Lista48_iva"];
                item.Mbvsstgp_48 = (decimal)dr["Mbvsstgp_48"];

                item.PMin47 = (decimal)dr["PMin47"];
                item.Lista47 = (decimal)dr["Lista47"];
                item.Lista47_iva = (decimal)dr["Lista47_iva"];
                item.Mbvsstgp_47 = (decimal)dr["Mbvsstgp_47"];

                item.PMin29 = (decimal)dr["PMin29"];
                item.Lista29 = (decimal)dr["Lista29"];
                item.Lista29_iva = (decimal)dr["Lista29_iva"];
                item.Mbvsstgp_29 = (decimal)dr["Mbvsstgp_29"];

                item.PMin28 = (decimal)dr["PMin28"];
                item.Lista28 = (decimal)dr["Lista28"];
                item.Lista28_iva = (decimal)dr["Lista28_iva"];
                item.Mbvsstgp_28 = (decimal)dr["Mbvsstgp_28"];
                             
                


                Detalle.Add(item);
            }
            return Detalle;
        }
        public List<ProductosSap> CoreOtherListPriceSearchReference(string ItemCode)
        {
            List<ProductosSap> Detalle = new List<ProductosSap>();
            DbCommand cmd = this.Database.GetStoredProcCommand("uspGetListPriceSearch");
            this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new ProductosSap
                {
                    ItemCode = (string)dr["ItemCode"],
                    Sequence = (string)dr["Sequence"].ToString(),
                    Nombre = (string)dr["Nombre"].ToString(),
                    MayoreoDesde = DBNull.Value.Equals(dr["MayoreoDesde"]) ? 0 : (int)dr["MayoreoDesde"],
                    MayoreoDistribuidorDesde = DBNull.Value.Equals(dr["MayoreoDistribuidorDesde"]) ? 0 : (int)dr["MayoreoDistribuidorDesde"],

                    Lista10 = DBNull.Value.Equals(dr["Lista10"]) ? 0 : (decimal)dr["Lista10"],
                    Lista9 = DBNull.Value.Equals(dr["Lista9"]) ? 0 : (decimal)dr["Lista9"],
                    Lista15 = DBNull.Value.Equals(dr["Lista15"]) ? 0 : (decimal)dr["Lista15"],
                    Frn = DBNull.Value.Equals(dr["Frn"]) ? 0 : (decimal)dr["Frn"],

                    Lista33_iva = DBNull.Value.Equals(dr["Lista33_iva"]) ? 0 : (decimal)dr["Lista33_iva"],
                    Lista33 = DBNull.Value.Equals(dr["Lista33"]) ? 0 : (decimal)dr["Lista33"],
                    Mbvsstgp_33 = DBNull.Value.Equals(dr["Mbvsstgp_33"]) ? 0 : (decimal)dr["Mbvsstgp_33"],

                    Lista25_iva = DBNull.Value.Equals(dr["Lista25_iva"]) ? 0 : (decimal)dr["Lista25_iva"],
                    Lista25 = DBNull.Value.Equals(dr["Lista25"]) ? 0 : (decimal)dr["Lista25"],
                    Mbvsstgp_25 = DBNull.Value.Equals(dr["Mbvsstgp_25"]) ? 0 : (decimal)dr["Mbvsstgp_25"],

                    Lista14_iva = DBNull.Value.Equals(dr["Lista14_iva"]) ? 0 : (decimal)dr["Lista14_iva"],
                    Lista14 = DBNull.Value.Equals(dr["Lista14"]) ? 0 : (decimal)dr["Lista14"],
                    Mbvsstgp_14 = DBNull.Value.Equals(dr["Mbvsstgp_14"]) ? 0 : (decimal)dr["Mbvsstgp_14"],

                    Lista29_iva = DBNull.Value.Equals(dr["Lista29_iva"]) ? 0 : (decimal)dr["Lista29_iva"],
                    Lista29 = DBNull.Value.Equals(dr["Lista29"]) ? 0 : (decimal)dr["Lista29"],
                    Mbvsstgp_29 = DBNull.Value.Equals(dr["Mbvsstgp_29"]) ? 0 : (decimal)dr["Mbvsstgp_29"],

                    Lista28_iva = DBNull.Value.Equals(dr["Lista28_iva"]) ? 0 : (decimal)dr["Lista28_iva"],
                    Lista28 = DBNull.Value.Equals(dr["Lista28"]) ? 0 : (decimal)dr["Lista28"],
                    Mbvsstgp_28 = DBNull.Value.Equals(dr["Mbvsstgp_28"]) ? 0 : (decimal)dr["Mbvsstgp_28"]
                });
            }
            return Detalle;
        }
        //AQUI COMIENZA LOS METODOS DE PRODUCTOS CHINA        
        public List<ProductoChina> GetProductosChina(string Sku, string Familia, string Categoria, string Clasificacion, string Tipo)
        {
            try
            {
                List<ProductoChina> Detalle = new List<ProductoChina>();
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetProductosSkuChina");
                if (!string.IsNullOrEmpty(Sku))
                    this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
                if (Familia != "SELECCIONE")
                    this.Database.AddInParameter(cmd, "@Familia", DbType.String, Familia);
                if (Categoria != "SELECCIONE")
                    this.Database.AddInParameter(cmd, "@Categoria", DbType.String, Categoria);
                if (Clasificacion != "SELECCIONE")
                    this.Database.AddInParameter(cmd, "@Clasificacion", DbType.String, Clasificacion);
                if (Tipo != "SELECCIONE")
                    this.Database.AddInParameter(cmd, "@Tipo", DbType.String, Tipo);
                cmd.CommandTimeout = 600;
                IDataReader dr = this.Database.ExecuteReader(cmd);
                while (dr.Read())
                {
                    Detalle.Add(new ProductoChina
                    {
                        SKU = dr["SKU"] == DBNull.Value ? "" : (string)dr["SKU"],
                        Marca = dr["Marca"] == DBNull.Value ? "" : (string)dr["Marca"],
                        Articulo = dr["Articulo"] == DBNull.Value ? "" : (string)dr["Articulo"],
                        Familia = dr["Familia"] == DBNull.Value ? "" : (string)dr["Familia"],
                        Activo = dr["Activo"] == DBNull.Value ? 0 : (int)dr["Activo"],
                        Costo_USD = dr["Costo_USD"] == DBNull.Value ? 0.00m : (decimal)dr["Costo_USD"],
                        Stock_CEDIS = dr["Stock_CEDIS"] == DBNull.Value ? 0 : (int)dr["Stock_CEDIS"],
                        Descripcion = dr["Descripcion"] == DBNull.Value ? "" : (string)dr["Descripcion"],
                        TipoPrecio = dr["TipoPrecio"] == DBNull.Value ? "" : (string)dr["TipoPrecio"],
                        PrecioUltra = dr["PrecioUltra"] == DBNull.Value ? 0.0m : (decimal)dr["PrecioUltra"],
                        Canal = (string)dr["Canal"],
                        StatusC = (string)dr["StatusC"],
                        Cliente = (string)dr["Cliente"]
                    });
                }
                return Detalle;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message } Inner Exception: {(ex.InnerException != null ? ex.InnerException.StackTrace : "")}");
            }
        }
        public ProductoChina GetDetalleProductosChina(string Sku)
        {
            ProductoChina Detalle = new ProductoChina();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetDetalleProductoChina");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.SKU = (string)dr["SKU"];
                Detalle.Articulo = (string)dr["Articulo"];
                Detalle.Familia = (string)dr["Familia"];
                Detalle.SubFamilias = (string)dr["SubFamilias"];
                Detalle.Empaque = (string)dr["Empaque"];
                Detalle.Inner = (string)dr["Inner"];
                Detalle.Master = (string)dr["Master"];
                Detalle.Codigo = (string)dr["CardCode"];
                Detalle.Proveedor = (string)dr["Proveedor"];
                Detalle.Costo_USD = (decimal)dr["Costo_USD"];
                Detalle.Costo_MXN = (decimal)dr["Costo_MXN"];
                Detalle.Stock_CEDIS = (int)dr["Stock_CEDIS"];
                Detalle.Accesorios = (string)dr["Accesorios"];
                Detalle.Categoria = (string)dr["Categoria"];
                Detalle.Tipo = (string)dr["Tipo"];
                Detalle.Clasificacion = (string)dr["Clasificacion"];
                Detalle.Igi = (string)dr["Igi"];
                Detalle.Nom = (string)dr["Nom"];
                Detalle.NomVigencia = dr["NomVigencia"] == DBNull.Value ? null :(DateTime?)dr["NomVigencia"];
                Detalle.UltimaFechaLlegada = dr["UltimaFechaLlegada"] == DBNull.Value ? null : (DateTime?)dr["UltimaFechaLlegada"];
                Detalle.CantidadRecibida = (int)dr["CantidadRecibida"];
                Detalle.Marca = (string)dr["Marca"];
            }
            return Detalle;
        }
        public IList<ListaPrecio> GetPreciosFacturaChina(string Sku)
        {
            List<ListaPrecio> Detalle = new List<ListaPrecio>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPreciosFacturaChina");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new ListaPrecio
                {
                    Nombre = (string)dr["Factura"],
                    Precio = (decimal)dr["Precio"]
                });
            }
            return Detalle;
        }
        public IList<ListaPrecio> GetPreciosCartaFacturaChina(string Sku)
        {
            List<ListaPrecio> Detalle = new List<ListaPrecio>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetPreciosCartaFacturaChina");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new ListaPrecio
                {
                    Nombre = (string)dr["Carta"],
                    Precio = (decimal)dr["Precio"]
                });
            }
            return Detalle;
        }
        public IList<ListaPrecio> GetListasPreciosChina(string Sku)
        {
            List<ListaPrecio> Detalle = new List<ListaPrecio>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetListasPreciosChina");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new ListaPrecio
                {
                    Nombre = (string)dr["Lista"],
                    Precio = (decimal)dr["Precio"]
                });
            }
            return Detalle;
        }
        public IList<OrdenesCompra> GetOrdenesCompra(string Sku)
        {
            List<OrdenesCompra> Detalle = new List<OrdenesCompra>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetOrdenesCompra");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new OrdenesCompra
                {
                    Folio = (int)dr["OC"],
                    Fecha = (DateTime)dr["Fecha"],
                    Proveedor = (string)dr["Proveedor"],
                    Cantidad = (int)dr["Cantidad"],
                    Precio = (decimal)dr["Precio"]
                });
            }
            return Detalle;
        }
        public IList<Envios> GetEnvios(string Sku)
        {
            List<Envios> Detalle = new List<Envios>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetEnviosProductosChina");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Envios
                {
                    Folio = (int)dr["Folio"],
                    Fecha = (DateTime)dr["Fecha"],
                    Codigo = (string)dr["Codigo"],
                    Proveedor = (string)dr["Proveedor"],
                    Cantidad = (int)dr["Cantidad"],
                    Precio = (decimal)dr["Precio"],
                    LlegaPuerto = (DateTime)dr["LlegaPuerto"],
                    LlegaCedis = (DateTime)dr["LlegaCedis"]
                });
            }
            return Detalle;
        }
        public List<Familia> GetFamiliasSAP()
        {
            List<Familia> familias = new List<Familia>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetFamiliasSAP");

            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                familias.Add(new Familia() { Nombre = (string)dr["Nombre"] });
            }
            return familias;
        }

        public List<Categoria.Categoria> GetCategoriasSAP(string Familia)
        {
            List<Categoria.Categoria> categorias = new List<Categoria.Categoria>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCategoriasSAP");
            this.Database.AddInParameter(cmd, "@Familia", DbType.String, Familia);

            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                categorias.Add(new Categoria.Categoria() { nameCate = (string)dr["Nombre"] });
            }
            return categorias;
        }

        public List<Clasificaciones.Clasificacion> GetClasificacionesSAP(string Categoria)
        {
            List<Clasificaciones.Clasificacion> clasificaciones = new List<Clasificaciones.Clasificacion>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetClasificacionesSAP");
            this.Database.AddInParameter(cmd, "@Categoria", DbType.String, Categoria);

            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                clasificaciones.Add(new Clasificaciones.Clasificacion() { Nombre = (string)dr["Nombre"] });
            }
            return clasificaciones;
        }

        public List<Tipos.Tipo> GetTiposSAP(string Categoria, string Clasificacion)
        {
            List<Tipos.Tipo> tipos = new List<Tipos.Tipo>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTiposSAP");
            this.Database.AddInParameter(cmd, "@Categoria", DbType.String, Categoria);
            this.Database.AddInParameter(cmd, "@Clasificacion", DbType.String, Clasificacion);

            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                tipos.Add(new Tipos.Tipo() { Nombre = (string)dr["Nombre"] });
            }
            return tipos;
        }

        public List<ComentarioSkuChina> Comentarios(string Sku)
        {
            List<ComentarioSkuChina> comentarios = new List<ComentarioSkuChina>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetComentariosSkuChina");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);

            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                comentarios.Add(new ComentarioSkuChina {Fecha =(DateTime)dr["Fecha"], Usuario = (string)dr["Usuario"], Comentario = (string)dr["Comentario"]});
            }
            return comentarios;
        }

        public bool PutComment(string Sku, string Comentario,string Usuario)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddComentSkuChina");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            this.Database.AddInParameter(cmd, "@Comentario", DbType.String, Comentario);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);

            int afectados = this.Database.ExecuteNonQuery(cmd);
            if (afectados > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<ProductoLlegada> GetProductosDetalleLlegadas(int EnProduccion)
        {
            DbCommand storedProcCommand = this.Database.GetStoredProcCommand("prGetProximasLlegadasImportaciones");
            this.Database.AddInParameter(storedProcCommand, "@EnProduccion", DbType.Int32, (object)EnProduccion);

            IDataReader dataReader = this.Database.ExecuteReader(storedProcCommand);

            List<ProductoLlegada> productoList = new List<ProductoLlegada>();
            while (dataReader.Read())
                productoList.Add(new ProductoLlegada()
                {
                    Sku = dataReader["Sku"] == DBNull.Value ? "" : (string)dataReader["Sku"],
                    Nombre = dataReader["Articulo"] == DBNull.Value ? "" : (string)dataReader["Articulo"],
                    Contenedor = dataReader["Contedor"] == DBNull.Value ? "Sin Contenedor" : (string)dataReader["Contedor"],
                    Proveedor = dataReader["Contedor"] == DBNull.Value ? "No especificado" : (string)dataReader["Proveedor"],
                    Envio = dataReader["Envio"] == DBNull.Value ? 0 : (int)dataReader["Envio"],
                    FechaLlegada = dataReader["FechaAproxCEDIS"] == DBNull.Value ? "Sin fecha" : (string)dataReader["FechaAproxCEDIS"],
                    Cantidad = dataReader["QtyPendiente"] == DBNull.Value ? 0 : (int)dataReader["QtyPendiente"],
                    Precio = dataReader["Precio"] == DBNull.Value ? 0 : (decimal)dataReader["Precio"],
                    PrecioFlete = dataReader["PrecioFlete"] == DBNull.Value ? 0.00m : (decimal)dataReader["PrecioFlete"],
                    Nom = dataReader["NOM"] == DBNull.Value ? "" : (string)dataReader["NOM"],
                    VencimientoNom = dataReader["VencimientoNom"] == DBNull.Value ? null : (DateTime?)dataReader["VencimientoNom"],
                    FraccionArancelaria = dataReader["FraccionArancelaria"] == DBNull.Value ? "" : (string)dataReader["FraccionArancelaria"],
                    Certificado = dataReader["Certificado"] == DBNull.Value ? "" : (string)dataReader["Certificado"],
                    EmisionCertificado = dataReader["EmisionCertificado"] == DBNull.Value ? null : (DateTime?)dataReader["EmisionCertificado"],

                });
            return productoList;
        }

    }
}