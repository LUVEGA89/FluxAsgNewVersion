using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;

namespace Reporting.Service.Core.SAP
{
    public class SAPManager : DataRepository
    {
        public List<Tipos> GeFamilia()
        {
            List<Tipos> Tipo = new List<Tipos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSAPFamilias");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Tipo.Add(new Tipos
                {
                    Codigo = int.Parse(dr["Codigo"].ToString()),
                    Nombre = dr["Nombre"].ToString()
                });

            }
            return Tipo;
        }

        public List<Tipos> GetNOM()
        {
            List<Tipos> Tipo = new List<Tipos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSAPNOM");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Tipo.Add(new Tipos
                {
                    Nombre = dr["Nombre"].ToString(),
                    Descripcion = dr["Descripcion"].ToString()
                });

            }
            return Tipo;
        }

        public List<Tipos> GetAduanas()
        {
            List<Tipos> Tipo = new List<Tipos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSAPAduanas");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Tipo.Add(new Tipos
                {
                    Nombre = dr["Nombre"].ToString(),
                    Codigo = int.Parse(dr["Codigo"].ToString())
                });

            }
            return Tipo;
        }

        public List<Tipos> GetTipos(int Tipo)
        {
            List<Tipos> Listado = new List<Tipos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSAPClasificaciones");
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Listado.Add(new Tipos
                {
                    Codigo = int.Parse(dr["Codigo"].ToString()),
                    Nombre = dr["Nombre"].ToString()
                });

            }
            return Listado;
        }
        public bool GeneraCodigoBarra(int Sequence)
        {
            List<Tipos> Listado = new List<Tipos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddSAPBarCodeArticulo");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            try
            {
                this.Database.ExecuteReader(cmd);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool AutorizaArticulo(int Sequence)
        {
            List<Tipos> Listado = new List<Tipos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateSAPArticulo");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.String, Sequence);
            try
            {
                this.Database.ExecuteReader(cmd);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Tipos> FindProveedor()
        {
            List<Tipos> Listado = new List<Tipos>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prFindSAPProveedor");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Listado.Add(new Tipos
                {
                    Descripcion = dr["Nombre"].ToString(),
                    Nombre = dr["Codigo"].ToString()
                });

            }
            return Listado;
        }

        public int AddArticulo(string Sku, string TipoProducto, string Marca, int Familia, string Categoria, string Clasificacion, string Tipo, string TipoEmpaque, string Inner, string Master, string Descripcion, string DescripcionIngles, string Accesorios, string SkuFabricante, string CodigoProveedor, int MaximoCompra, int MinimoCompra, decimal Largo, decimal Ancho, decimal Alto, decimal Peso, string Nom, int CodigoSAT, string Fraccion, decimal Aduana, string DescripcionAduana, string Usuario, string Barcode, string BarcodeInner, string BarcodeMaster, string Estatus)
        {
            int Sequence = 0;
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddSAPArticulo");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            this.Database.AddInParameter(cmd, "@TipoProducto", DbType.String, TipoProducto);
            this.Database.AddInParameter(cmd, "@Marca", DbType.String, Marca);
            this.Database.AddInParameter(cmd, "@Familia", DbType.Int32, Familia);
            this.Database.AddInParameter(cmd, "@Categoria", DbType.String, Categoria);
            this.Database.AddInParameter(cmd, "@Clasificacion", DbType.String, Clasificacion);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.String, Tipo);
            this.Database.AddInParameter(cmd, "@TipoEmpaque", DbType.String, TipoEmpaque);
            this.Database.AddInParameter(cmd, "@Inner", DbType.String, Inner);
            this.Database.AddInParameter(cmd, "@Master", DbType.String, Master);
            this.Database.AddInParameter(cmd, "@Descripcion", DbType.String, Descripcion);
            this.Database.AddInParameter(cmd, "@DescripcionIngles", DbType.String, DescripcionIngles);
            this.Database.AddInParameter(cmd, "@Accesorios", DbType.String, Accesorios);
            this.Database.AddInParameter(cmd, "@SkuFabricante", DbType.String, SkuFabricante);
            this.Database.AddInParameter(cmd, "@CodigoProveedor", DbType.String, CodigoProveedor);
            this.Database.AddInParameter(cmd, "@MaximoCompra", DbType.Int32, MaximoCompra);
            this.Database.AddInParameter(cmd, "@MinimoCompra", DbType.Int32, MinimoCompra);
            this.Database.AddInParameter(cmd, "@Largo", DbType.Decimal, Largo);
            this.Database.AddInParameter(cmd, "@Ancho", DbType.Decimal, Ancho);
            this.Database.AddInParameter(cmd, "@Alto", DbType.Decimal, Alto);
            this.Database.AddInParameter(cmd, "@Peso", DbType.Decimal, Peso);
            this.Database.AddInParameter(cmd, "@Nom", DbType.String, Nom);
            this.Database.AddInParameter(cmd, "@CodigoSAT", DbType.Int32, CodigoSAT);
            this.Database.AddInParameter(cmd, "@Fraccion", DbType.String, Fraccion);
            this.Database.AddInParameter(cmd, "@Aduana", DbType.Int32, Aduana);
            this.Database.AddInParameter(cmd, "@DescripcionAduana", DbType.String, DescripcionAduana);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);
            this.Database.AddInParameter(cmd, "@Barcode", DbType.String, Barcode);
            this.Database.AddInParameter(cmd, "@BarcodeInner", DbType.String, BarcodeInner);
            this.Database.AddInParameter(cmd, "@BarcodeMaster", DbType.String, BarcodeMaster);
            this.Database.AddInParameter(cmd, "@Estatus", DbType.String, Estatus);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {

                Sequence = int.Parse(dr["Sequence"].ToString());

            }
            return Sequence;
        }

        public int AddAnexo(string Sku, string Nombre, string Archivo, string Tipo, string Usuario)
        {
            int Sequence = 0;
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddSAPAnexos");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, Nombre);
            this.Database.AddInParameter(cmd, "@Archivo", DbType.String, Archivo);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.String, Tipo);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, Usuario);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {

                Sequence = int.Parse(dr["Sequence"].ToString());

            }
            return Sequence;
        }

        public Producto FindProducto(string Sku)
        {
            Producto producto = new Producto();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSAPDetalleProducto");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                producto.Sku = dr["Sku"].ToString();
                producto.Familia = dr["Familia"].ToString();
                producto.Marca = dr["Marca"].ToString();
                producto.Categoria = dr["Categoria"].ToString();
                producto.Tipo = dr["Tipo"].ToString();
                producto.Clasificacion = dr["Clasificacion"].ToString();
                producto.TipoEmpaque = dr["TipoEmpaque"].ToString();
                producto.CantInner = dr["CantInner"].ToString();
                producto.CantMaster = dr["CantMaster"].ToString();
                producto.DescripcionComercial = dr["DescripcionComercial"].ToString();
                producto.DescripcionIngles = dr["DescripcionIngles"].ToString();
                producto.Accesorios = dr["Accesorios"].ToString();
                producto.SkuFabricante = dr["SkuFabricante"].ToString();
                producto.CodigoProveedor = dr["CodigoProveedor"].ToString();
                producto.Largo = dr["Largo"].ToString();
                producto.Ancho = dr["Ancho"].ToString();
                producto.Alto = dr["Alto"].ToString();
                producto.Peso = dr["Peso"].ToString();
                producto.Maximo = dr["Maximo"].ToString();
                producto.Minimo = dr["Minimo"].ToString();
                producto.Nom = dr["Nom"].ToString();
                producto.CodigoSAT = dr["CodigoSAT"].ToString();
                producto.Franccion = dr["Franccion"].ToString();
                producto.Aduanas = dr["Aduanas"].ToString();
                producto.Barcode = dr["Barcode"].ToString();
                producto.BarcodeInner = dr["BarcodeInner"].ToString();
                producto.BarcodeMaster = dr["BarcodeMaster"].ToString();

            }
            return producto;
        }
        public Producto FindMiProducto(int Sequence)
        {
            Producto producto = new Producto();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSAPDetalleMiProducto");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                producto.Sku = dr["Sku"].ToString();
                producto.Familia = dr["Familia"].ToString();
                producto.Marca = dr["Marca"].ToString();
                producto.Categoria = dr["Categoria"].ToString();
                producto.Tipo = dr["Tipo"].ToString();
                producto.Clasificacion = dr["Clasificacion"].ToString();
                producto.TipoEmpaque = dr["TipoEmpaque"].ToString();
                producto.CantInner = dr["CantInner"].ToString();
                producto.CantMaster = dr["CantMaster"].ToString();
                producto.DescripcionComercial = dr["DescripcionComercial"].ToString();
                producto.DescripcionIngles = dr["DescripcionIngles"].ToString();
                producto.Accesorios = dr["Accesorios"].ToString();
                producto.SkuFabricante = dr["SkuFabricante"].ToString();
                producto.CodigoProveedor = dr["CodigoProveedor"].ToString();
                producto.Largo = dr["Largo"].ToString();
                producto.Ancho = dr["Ancho"].ToString();
                producto.Alto = dr["Alto"].ToString();
                producto.Peso = dr["Peso"].ToString();
                producto.Maximo = dr["Maximo"].ToString();
                producto.Minimo = dr["Minimo"].ToString();
                producto.Nom = dr["Nom"].ToString();
                producto.CodigoSAT = dr["CodigoSAT"].ToString();
                producto.Franccion = dr["Franccion"].ToString();
                producto.Aduanas = dr["Aduanas"].ToString();
                producto.Barcode = dr["Barcode"].ToString();
                producto.BarcodeInner = dr["BarcodeInner"].ToString();
                producto.BarcodeMaster = dr["BarcodeMaster"].ToString();
                producto.DescripcionAduana = dr["DescripcionAduana"].ToString();

            }
            return producto;
        }

        public Producto GetProducto(int Sequence)
        {
            Producto producto = new Producto();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGETSAPArticulo");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                producto.Sequence = int.Parse(dr["Sequence"].ToString());
                producto.Sku = dr["Sku"].ToString();
                producto.Familia = dr["Familia"].ToString();
                producto.FamiliaM = dr["FamiliaM"].ToString();
                producto.Marca = dr["Marca"].ToString();
                producto.Categoria = dr["Categoria"].ToString();
                producto.Tipo = dr["Tipo"].ToString();
                producto.Clasificacion = dr["Clasificacion"].ToString();
                producto.TipoEmpaque = dr["TipoEmpaque"].ToString();
                producto.CantInner = dr["PInner"].ToString();
                producto.CantMaster = dr["PMaster"].ToString();
                producto.DescripcionComercial = dr["Descripcion"].ToString();
                producto.DescripcionIngles = dr["DescripcionIngles"].ToString();
                producto.Accesorios = dr["Accesorios"].ToString();
                producto.SkuFabricante = dr["SkuFabricante"].ToString();
                producto.CodigoProveedor = dr["CodigoProveedor"].ToString();
                producto.Largo = dr["Largo"].ToString();
                producto.Ancho = dr["Ancho"].ToString();
                producto.Alto = dr["Alto"].ToString();
                producto.Peso = dr["Peso"].ToString();
                producto.Maximo = dr["Maximo"].ToString();
                producto.Minimo = dr["Minimo"].ToString();
                producto.Nom = dr["Nom"].ToString();
                producto.CodigoSAT = dr["CodigoSAT"].ToString();
                producto.Franccion = dr["FraccionArancelaria"].ToString();
                producto.Aduanas = dr["GrupoAduanal"].ToString();
                producto.DescripcionAduana = dr["DescripcionAduana"].ToString();
                producto.Barcode = dr["Barcode"].ToString();
                producto.BarcodeInner = dr["BarcodeInner"].ToString();
                producto.BarcodeMaster = dr["BarcodeMaster"].ToString();
                producto.EsNuevoParkoiwa = int.Parse(dr["EsNuevoParkoiwa"].ToString());
                producto.EsNuevoMassriv = int.Parse(dr["EsNuevoMassriv"].ToString());
                producto.EsNuevoSteuben = int.Parse(dr["EsNuevoSteuben"].ToString());
                producto.EsNuevoOkku = int.Parse(dr["EsNuevoOkku"].ToString());

                producto.SincronizadoParkoiwa = int.Parse(dr["SincronizadoParkoiwa"].ToString());
                producto.SincronizadoMassriv = int.Parse(dr["SincronizadoMassriv"].ToString());
                producto.SincronizadoSteuben = int.Parse(dr["SincronizadoSteuben"].ToString());
                producto.SincronizadoOkku = int.Parse(dr["SincronizadoOkku"].ToString());

            }
            return producto;
        }



        public DataTable GetProductoDT(int Sequence)
        {
            DataTable dt = new DataTable();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGETSAPArticulo");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            dt.Load(this.Database.ExecuteReader(cmd));

            return dt;
        }
        public Anexo GetAnexo(int Sequence)
        {
            Anexo anexo = new Anexo();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGETSAPAnexoDetalle");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                anexo.Sequence = int.Parse(dr["Sequence"].ToString());
                anexo.Sku = dr["Sku"].ToString();
                anexo.Nombre = dr["Nombre"].ToString();
                anexo.Archivo = dr["Archivo"].ToString();
                anexo.Tipo = dr["Tipo"].ToString();
                anexo.Parkoiwa = int.Parse(dr["Parkoiwa"].ToString());
                anexo.Massriv = int.Parse(dr["Massriv"].ToString());
                anexo.Steuben = int.Parse(dr["Steuben"].ToString());
                anexo.Okku = int.Parse(dr["Okku"].ToString());

            }
            return anexo;
        }

        public void UpdateProductoSAP(int Sequence, EnumBaseDatos Base)
        {
            List<Anexo> Listado = new List<Anexo>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateSAPArticuloSincronizado");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
            this.Database.AddInParameter(cmd, "@BaseSAP", DbType.Int32, Base);
            IDataReader dr = this.Database.ExecuteReader(cmd);

        }
        public List<Anexo> GetAnexosPendientes()
        {
            List<Anexo> Listado = new List<Anexo>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGETSAPAnexosPendientes");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Listado.Add(new Anexo
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    Sku = dr["Sku"].ToString(),
                    Nombre = dr["Nombre"].ToString(),
                    Archivo = dr["Archivo"].ToString()
                });

            }
            return Listado;
        }

        public List<Anexo> GetAnexos(string Sku)
        {
            List<Anexo> Listado = new List<Anexo>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGETSAPAnexo");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Listado.Add(new Anexo
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    Sku = dr["Sku"].ToString(),
                    Nombre = dr["Nombre"].ToString(),
                    Archivo = dr["Archivo"].ToString(),
                    Estado = int.Parse(dr["Estado"].ToString())
                });

            }
            return Listado;
        }
        public List<Producto> GetArticulos()
        {
            List<Producto> Listado = new List<Producto>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSAPArticulosPendientes");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Listado.Add(new Producto
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    Sku = dr["Sku"].ToString(),
                    DescripcionComercial = dr["Descripcion"].ToString(),
                    Tipo = dr["TipoProducto"].ToString(),
                    Marca = dr["Marca"].ToString(),
                    SkuFabricante = dr["SkuFabricante"].ToString(),
                    Barcode = dr["Barcode"].ToString(),
                });

            }
            return Listado;
        }

        public List<Producto> GetArticulosByUser(string usuario)
        {
            List<Producto> Listado = new List<Producto>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetSAPArticulosPendientesByUser");
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, usuario);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Listado.Add(new Producto
                {
                    Sequence = int.Parse(dr["Sequence"].ToString()),
                    Sku = dr["Sku"].ToString(),
                    DescripcionComercial = dr["Descripcion"].ToString(),
                    Tipo = dr["TipoProducto"].ToString(),
                    Marca = dr["Marca"].ToString(),
                    SkuFabricante = dr["SkuFabricante"].ToString(),
                    Barcode = dr["Barcode"].ToString(),
                });

            }
            return Listado;
        }

        public string SyncSAPProduct(Producto producto, string DB, string Usuario, string Password, bool EsNuevo, EnumBaseDatos TipoBase)
        {
            string FolioSAP = "";
            int nResult;
            int lRetCode;
            SAPbobsCOM.Company oCompany = new SAPbobsCOM.Company();
            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish;
            oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
            oCompany.UseTrusted = false;
            oCompany.LicenseServer = ConfigurationManager.AppSettings["SAP.Alta.LicenseServer"];// "MASSRIV2007:30000";
            oCompany.Server = ConfigurationManager.AppSettings["SAP.Alta.DbServer"];// "MASSRIV2007";
            oCompany.DbUserName = ConfigurationManager.AppSettings["SAP.Alta.DbUser"];// "sa";
            oCompany.DbPassword = ConfigurationManager.AppSettings["SAP.Alta.DbPassword"];// "Passw0rd";

            oCompany.CompanyDB = DB;
            oCompany.UserName = Usuario;
            oCompany.Password = Password;
            oCompany.Disconnect();
            nResult = oCompany.Connect();

            if (nResult == 0)
            {
                SAPbobsCOM.Items oProduct = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                SAPbobsCOM.Recordset rs = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                //Intenta Buscar en caso de que el producto ya exista.
                if (!EsNuevo)
                    oProduct.GetByKey(producto.Sku);

                oProduct.ItemCode = producto.Sku;
                oProduct.Picture = producto.Sku + ".jpg";
                oProduct.ItemName = producto.DescripcionComercial;

                oProduct.ForeignName = producto.DescripcionIngles;

                if (DB == "Parkoiwa2009")
                    oProduct.Mainsupplier = producto.CodigoProveedor;

                oProduct.PurchasePackagingUnit = producto.TipoEmpaque;
                oProduct.SupplierCatalogNo = producto.SkuFabricante;

                oProduct.BarCode = producto.Barcode;


                oProduct.PurchaseUnitHeight = int.Parse(producto.Alto);
                oProduct.PurchaseUnitWidth = int.Parse(producto.Ancho);
                oProduct.PurchaseUnitWeight = int.Parse(producto.Peso);
                oProduct.PurchaseUnitLength = int.Parse(producto.Largo);

                oProduct.PurchaseItemsPerUnit = 1;
                oProduct.PurchaseQtyPerPackUnit = 1;
                oProduct.SalesItemsPerUnit = 1;
                oProduct.SalesQtyPerPackUnit = 1;

                oProduct.SalesPackagingUnit = "H87";
                oProduct.PurchaseUnit = "PZA";
                oProduct.SalesUnit = "PZA";

                oProduct.PurchaseLengthUnit = 2;
                oProduct.PurchaseWeightUnit = 2;
                oProduct.PurchaseWidthUnit = 2;
                oProduct.PurchaseHeightUnit = 2;

                if (TipoBase == EnumBaseDatos.Parkoiwa || TipoBase == EnumBaseDatos.Massriv)
                {
                    if (TipoBase == EnumBaseDatos.Massriv)
                    {
                        oProduct.ItemsGroupCode = int.Parse(producto.FamiliaM);
                    }
                    else
                    {
                        oProduct.ItemsGroupCode = int.Parse(producto.Familia);
                    }

                    oProduct.CustomsGroupCode = int.Parse(producto.Aduanas);
                    //NO usar en dos bases de datos Okku y Steuben
                    oProduct.InventoryItem = BoYesNoEnum.tYES;
                    oProduct.SRIAndBatchManageMethod = BoManageMethod.bomm_OnEveryTransaction;
                    oProduct.ManageSerialNumbers = BoYesNoEnum.tNO;
                    oProduct.ManageBatchNumbers = BoYesNoEnum.tYES;
                }

                oProduct.UserFields.Fields.Item("U_Maximo").Value = producto.Maximo;
                oProduct.UserFields.Fields.Item("U_Minimo").Value = producto.Minimo;
                oProduct.UserFields.Fields.Item("U_Marca").Value = producto.Marca;
                oProduct.UserFields.Fields.Item("U_Categoria").Value = producto.Categoria;
                oProduct.UserFields.Fields.Item("U_Tipo_categoria").Value = producto.Tipo;
                oProduct.UserFields.Fields.Item("U_Clasificacion").Value = producto.Clasificacion;
                oProduct.UserFields.Fields.Item("U_CantInner").Value = producto.CantInner;
                oProduct.UserFields.Fields.Item("U_CantMaster").Value = producto.CantMaster;
                oProduct.UserFields.Fields.Item("U_Accesorios").Value = producto.Accesorios;
                oProduct.UserFields.Fields.Item("U_NOM").Value = producto.Nom;
                if (DB == "Massriv2007")
                {
                    oProduct.UserFields.Fields.Item("U_catego1").Value = "0";
                }
                

                if (TipoBase == EnumBaseDatos.Massriv)
                    oProduct.UserFields.Fields.Item("U_cod_SAT").Value = producto.CodigoSAT;
                else
                    oProduct.UserFields.Fields.Item("U_Cod_SAT").Value = producto.CodigoSAT;

                oProduct.UserFields.Fields.Item("U_Fraccionarancelari").Value = producto.Franccion;
                oProduct.UserFields.Fields.Item("U_CBInner").Value = producto.BarcodeInner;
                oProduct.UserFields.Fields.Item("U_CBMaster").Value = producto.BarcodeMaster;

                //Añadimos la factura a SAP
                if (EsNuevo)
                    lRetCode = oProduct.Add();
                else
                    lRetCode = oProduct.Update();

                if (lRetCode == 0)
                {

                    // Validar que sea el mismo folio de referenia en SIE que el registrado en SAP...
                    rs.DoQuery("SELECT DocEntry FROM dbo.OITM WHERE ItemCode = '" + producto.Sku + "'");
                    while (!(rs.EoF))
                    {
                        int folio = rs.Fields.Item(0).Value;
                        FolioSAP = folio.ToString();
                        rs.MoveNext();
                    }

                    //Aqui se cambia el estatus al registro de SIE 
                    this.UpdateProductoSAP(producto.Sequence, TipoBase);
                }
                else
                {
                    int temp_int = 0;
                    string temp_string = "";
                    oCompany.GetLastError(out temp_int, out temp_string);
                    
                    throw new InvalidOperationException(oCompany.GetLastErrorDescription());

                }


            }
            else
            {
                throw new InvalidOperationException(oCompany.GetLastErrorDescription());
            }

            return FolioSAP;
        }

        public string GetEmailById(string UserId)
        {
            string Email = string.Empty;
            DbCommand cmd = this.Database.GetStoredProcCommand("dbo.prGetEmailById");
            this.Database.AddInParameter(cmd, "@Id", DbType.String, UserId);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Email = (string)dr["Email"];
            }
            return Email;
        }

        public bool SyncSAPAnexo(Anexo anexo, string DB, string Usuario, string Password, string Url, EnumBaseDatos TipoBase)
        {
            bool FolioSAP = false;
            int nResult;
            int lRetCode;
            SAPbobsCOM.Company oCompany = new SAPbobsCOM.Company();
            oCompany.language = SAPbobsCOM.BoSuppLangs.ln_Spanish;
            oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
            oCompany.UseTrusted = false;
            oCompany.LicenseServer = ConfigurationManager.AppSettings["SAP.Alta.LicenseServer"];// "MASSRIV2007:30000";
            oCompany.Server = ConfigurationManager.AppSettings["SAP.Alta.DbServer"];// "MASSRIV2007";
            oCompany.DbUserName = ConfigurationManager.AppSettings["SAP.Alta.DbUser"];// "sa";
            oCompany.DbPassword = ConfigurationManager.AppSettings["SAP.Alta.DbPassword"];// "Passw0rd";       
            oCompany.CompanyDB = DB;
            oCompany.UserName = Usuario;
            oCompany.Password = Password;
            oCompany.Disconnect();
            nResult = oCompany.Connect();

            if (nResult == 0)
            {
                SAPbobsCOM.Attachments2 oAtt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
                SAPbobsCOM.Items oProduct = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);

                oAtt.Lines.Add();
                oAtt.Lines.FileName = anexo.Nombre;
                oAtt.Lines.FileExtension = anexo.Tipo == "JPG" ? "jpg" : "pdf";
                oAtt.Lines.SourcePath = Url;
                oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;

                lRetCode = oAtt.Add();
                if (lRetCode == 0)
                {
                    int SeqAttEntry = -1;
                    SeqAttEntry = int.Parse(oCompany.GetNewObjectKey());
                    //Busca el producto
                    oProduct.GetByKey(anexo.Sku);
                    //Adjunta archivo al producto
                    oProduct.AttachmentEntry = SeqAttEntry;
                    //Actualiza el anexo
                    int RCodeProducto = oProduct.Update();

                    if (RCodeProducto == 0)
                    {
                        FolioSAP = true;
                        //Actualiza SIE
                        this.UpdateAnexoSAP(anexo.Sequence, TipoBase);
                    }
                    else
                    {
                        throw new InvalidOperationException(oCompany.GetLastErrorDescription());
                    }
                }
                else
                {
                    throw new InvalidOperationException(oCompany.GetLastErrorDescription());
                }

            }
            else
            {
                throw new InvalidOperationException(oCompany.GetLastErrorDescription());
            }

            return FolioSAP;
        }

        public void UpdateAnexoSAP(int Sequence, EnumBaseDatos Base)
        {
            List<Anexo> Listado = new List<Anexo>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateSAPAnexoSincronizado");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
            this.Database.AddInParameter(cmd, "@BaseSAP", DbType.Int32, Base);
            IDataReader dr = this.Database.ExecuteReader(cmd);

        }


        #region BitacoraEdicionesArticulo
        public Producto GetProductoBySku(string Sku)
        {
            Producto producto = new Producto();
            DbCommand cmd = this.Database.GetStoredProcCommand("dbo.prGetArticuloSAP");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                producto.Sequence = int.Parse(dr["Sequence"].ToString());
                producto.Sku = dr["Sku"].ToString();
                producto.Familia = dr["Familia"].ToString();
                producto.TipoProducto = dr["TipoProducto"].ToString();
                //producto.FamiliaM = dr["FamiliaM"].ToString();
                producto.Marca = dr["Marca"].ToString();
                producto.Categoria = dr["Categoria"].ToString();
                producto.Tipo = dr["Tipo"].ToString();
                producto.Clasificacion = dr["Clasificacion"].ToString();
                producto.TipoEmpaque = dr["TipoEmpaque"].ToString();
                producto.CantInner = dr["PInner"].ToString();
                producto.CantMaster = dr["PMaster"].ToString();
                producto.DescripcionComercial = dr["Descripcion"].ToString();
                producto.DescripcionIngles = dr["DescripcionIngles"].ToString();
                producto.Accesorios = dr["Accesorios"].ToString();
                producto.SkuFabricante = dr["SkuFabricante"].ToString();
                producto.CodigoProveedor = dr["CodigoProveedor"].ToString();
                producto.Largo = dr["Largo"].ToString();
                producto.Ancho = dr["Ancho"].ToString();
                producto.Alto = dr["Alto"].ToString();
                producto.Peso = dr["Peso"].ToString();
                producto.Maximo = dr["Maximo"].ToString();
                producto.Minimo = dr["Minimo"].ToString();
                producto.Nom = dr["Nom"].ToString();
                producto.CodigoSAT = dr["CodigoSAT"].ToString();
                producto.Franccion = dr["FraccionArancelaria"].ToString();
                producto.Aduanas = dr["GrupoAduanal"].ToString();
                producto.DescripcionAduana = dr["DescripcionAduana"].ToString();
                producto.Barcode = dr["Barcode"].ToString();
                producto.BarcodeInner = dr["BarcodeInner"].ToString();
                producto.BarcodeMaster = dr["BarcodeMaster"].ToString();
                producto.Estatus = dr["Estatus"].ToString();

                //producto.EsNuevoParkoiwa = int.Parse(dr["EsNuevoParkoiwa"].ToString());
                //producto.EsNuevoMassriv = int.Parse(dr["EsNuevoMassriv"].ToString());
                //producto.EsNuevoSteuben = int.Parse(dr["EsNuevoSteuben"].ToString());
                //producto.EsNuevoOkku = int.Parse(dr["EsNuevoOkku"].ToString());

                producto.SincronizadoParkoiwa = int.Parse(dr["SincronizadoParkoiwa"].ToString());
                producto.SincronizadoMassriv = int.Parse(dr["SincronizadoMassriv"].ToString());
                producto.SincronizadoSteuben = int.Parse(dr["SincronizadoSteuben"].ToString());
                producto.SincronizadoOkku = int.Parse(dr["SincronizadoOkku"].ToString());


                producto.RegistradoEl = (DateTime)dr["RegistradoEl"];
                producto.ModificadoEl = (DateTime)dr["ModificadoEl"];

                producto.UsuarioRegistro = (string)dr["UsuarioRegistro"];
                producto.UsuarioModifico = (string)dr["UsuarioModifico"];
            }
            return producto;
        }

        public void AddArticuloHist(Producto item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddSAPArticuloHist");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, item.Sku);

            this.Database.AddInParameter(cmd, "@TipoProducto", DbType.String, item.TipoProducto);
            this.Database.AddInParameter(cmd, "@Marca", DbType.String, item.Marca);
            this.Database.AddInParameter(cmd, "@Familia", DbType.Int32, item.Familia);
            this.Database.AddInParameter(cmd, "@Categoria", DbType.String, item.Categoria);
            this.Database.AddInParameter(cmd, "@Clasificacion", DbType.String, item.Clasificacion);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.String, item.Tipo);
            this.Database.AddInParameter(cmd, "@TipoEmpaque", DbType.String, item.TipoEmpaque);
            this.Database.AddInParameter(cmd, "@Inner", DbType.String, item.CantInner);
            this.Database.AddInParameter(cmd, "@Master", DbType.String, item.CantMaster);
            this.Database.AddInParameter(cmd, "@Descripcion", DbType.String, item.DescripcionComercial);
            this.Database.AddInParameter(cmd, "@DescripcionIngles", DbType.String, item.DescripcionIngles);
            this.Database.AddInParameter(cmd, "@Accesorios", DbType.String, item.Accesorios);
            this.Database.AddInParameter(cmd, "@SkuFabricante", DbType.String, item.SkuFabricante);
            this.Database.AddInParameter(cmd, "@CodigoProveedor", DbType.String, item.CodigoProveedor);
            this.Database.AddInParameter(cmd, "@MaximoCompra", DbType.Int32, item.Maximo);
            this.Database.AddInParameter(cmd, "@MinimoCompra", DbType.Int32, item.Minimo);
            this.Database.AddInParameter(cmd, "@Largo", DbType.Decimal, item.Largo);
            this.Database.AddInParameter(cmd, "@Ancho", DbType.Decimal, item.Ancho);
            this.Database.AddInParameter(cmd, "@Alto", DbType.Decimal, item.Alto);
            this.Database.AddInParameter(cmd, "@Peso", DbType.Decimal, item.Peso);
            this.Database.AddInParameter(cmd, "@Nom", DbType.String, item.Nom);
            this.Database.AddInParameter(cmd, "@CodigoSAT", DbType.Int32, item.CodigoSAT);
            this.Database.AddInParameter(cmd, "@Fraccion", DbType.String, item.Franccion);
            this.Database.AddInParameter(cmd, "@Aduana", DbType.Int32, item.Aduanas);
            this.Database.AddInParameter(cmd, "@DescripcionAduana", DbType.String, item.DescripcionAduana);
            this.Database.AddInParameter(cmd, "@Usuario", DbType.String, item.UsuarioModifico);
            this.Database.AddInParameter(cmd, "@Barcode", DbType.String, item.Barcode);
            this.Database.AddInParameter(cmd, "@BarcodeInner", DbType.String, item.BarcodeInner);
            this.Database.AddInParameter(cmd, "@BarcodeMaster", DbType.String, item.BarcodeMaster);
            this.Database.AddInParameter(cmd, "@Estatus", DbType.String, item.Estatus);

            this.Database.AddInParameter(cmd, "@SequenceAnterior", DbType.Int32, item.Sequence);

            this.Database.ExecuteNonQuery(cmd);
        }


        public Producto GetProductoHistoryBySku(string Sku, string ModificadoPor)
        {
            Producto producto = new Producto();
            DbCommand cmd = this.Database.GetStoredProcCommand("dbo.prGetArticuloSAPHist");
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            this.Database.AddInParameter(cmd, "@UsuarioModifico", DbType.String, ModificadoPor);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                producto.Sequence = int.Parse(dr["Sequence"].ToString());
                producto.Sku = dr["Sku"].ToString();
                producto.Familia = dr["Familia"].ToString();
                producto.TipoProducto = dr["TipoProducto"].ToString();
                //producto.FamiliaM = dr["FamiliaM"].ToString();
                producto.Marca = dr["Marca"].ToString();
                producto.Categoria = dr["Categoria"].ToString();
                producto.Tipo = dr["Tipo"].ToString();
                producto.Clasificacion = dr["Clasificacion"].ToString();
                producto.TipoEmpaque = dr["TipoEmpaque"].ToString();
                producto.CantInner = dr["PInner"].ToString();
                producto.CantMaster = dr["PMaster"].ToString();
                producto.DescripcionComercial = dr["Descripcion"].ToString();
                producto.DescripcionIngles = dr["DescripcionIngles"].ToString();
                producto.Accesorios = dr["Accesorios"].ToString();
                producto.SkuFabricante = dr["SkuFabricante"].ToString();
                producto.CodigoProveedor = dr["CodigoProveedor"].ToString();
                producto.Largo = dr["Largo"].ToString();
                producto.Ancho = dr["Ancho"].ToString();
                producto.Alto = dr["Alto"].ToString();
                producto.Peso = dr["Peso"].ToString();
                producto.Maximo = dr["Maximo"].ToString();
                producto.Minimo = dr["Minimo"].ToString();
                producto.Nom = dr["Nom"].ToString();
                producto.CodigoSAT = dr["CodigoSAT"].ToString();
                producto.Franccion = dr["FraccionArancelaria"].ToString();
                producto.Aduanas = dr["GrupoAduanal"].ToString();
                producto.DescripcionAduana = dr["DescripcionAduana"].ToString();
                producto.Barcode = dr["Barcode"].ToString();
                producto.BarcodeInner = dr["BarcodeInner"].ToString();
                producto.BarcodeMaster = dr["BarcodeMaster"].ToString();
                producto.Estatus = dr["Estatus"].ToString();

                //producto.EsNuevoParkoiwa = int.Parse(dr["EsNuevoParkoiwa"].ToString());
                //producto.EsNuevoMassriv = int.Parse(dr["EsNuevoMassriv"].ToString());
                //producto.EsNuevoSteuben = int.Parse(dr["EsNuevoSteuben"].ToString());
                //producto.EsNuevoOkku = int.Parse(dr["EsNuevoOkku"].ToString());

                producto.SincronizadoParkoiwa = int.Parse(dr["SincronizadoParkoiwa"].ToString());
                producto.SincronizadoMassriv = int.Parse(dr["SincronizadoMassriv"].ToString());
                producto.SincronizadoSteuben = int.Parse(dr["SincronizadoSteuben"].ToString());
                producto.SincronizadoOkku = int.Parse(dr["SincronizadoOkku"].ToString());


                producto.RegistradoEl = (DateTime)dr["RegistradoEl"];
                producto.ModificadoEl = (DateTime)dr["ModificadoEl"];

                producto.UsuarioRegistro = (string)dr["UsuarioRegistro"];
                producto.UsuarioModifico = (string)dr["UsuarioModifico"];
            }
            return producto;
        }

        public List<Producto> GetReporteArticulosModificacion(DateTime Del, DateTime Al)
        {
            List<Producto> Items = new List<Producto>();
            DbCommand cmd = this.Database.GetStoredProcCommand("dbo.prFindArticuloSAPHistReporte");
            this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, Al);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Producto producto = new Producto();
                producto.Sequence = int.Parse(dr["Sequence"].ToString());
                producto.Sku = dr["Sku"].ToString();
                producto.Familia = dr["Familia"].ToString();
                producto.TipoProducto = dr["TipoProducto"].ToString();
                producto.FamiliaM = dr["FamiliaM"].ToString();
                producto.Marca = dr["Marca"].ToString();
                producto.Categoria = dr["Categoria"].ToString();
                producto.Tipo = dr["Tipo"].ToString();
                producto.Clasificacion = dr["Clasificacion"].ToString();
                producto.TipoEmpaque = dr["TipoEmpaque"].ToString();
                producto.CantInner = dr["PInner"].ToString();
                producto.CantMaster = dr["PMaster"].ToString();
                producto.DescripcionComercial = dr["Descripcion"].ToString();
                producto.DescripcionIngles = dr["DescripcionIngles"].ToString();
                producto.Accesorios = dr["Accesorios"].ToString();
                producto.SkuFabricante = dr["SkuFabricante"].ToString();
                producto.CodigoProveedor = dr["CodigoProveedor"].ToString();
                producto.Largo = dr["Largo"].ToString();
                producto.Ancho = dr["Ancho"].ToString();
                producto.Alto = dr["Alto"].ToString();
                producto.Peso = dr["Peso"].ToString();
                producto.Maximo = dr["Maximo"].ToString();
                producto.Minimo = dr["Minimo"].ToString();
                producto.Nom = dr["Nom"].ToString();
                producto.CodigoSAT = dr["CodigoSAT"].ToString();
                producto.Franccion = dr["FraccionArancelaria"].ToString();
                producto.Aduanas = dr["GrupoAduanal"].ToString();
                producto.DescripcionAduana = dr["DescripcionAduana"].ToString();
                producto.Barcode = dr["Barcode"].ToString();
                producto.BarcodeInner = dr["BarcodeInner"].ToString();
                producto.BarcodeMaster = dr["BarcodeMaster"].ToString();
                producto.Estatus = dr["Estatus"].ToString();

                //producto.EsNuevoParkoiwa = int.Parse(dr["EsNuevoParkoiwa"].ToString());
                //producto.EsNuevoMassriv = int.Parse(dr["EsNuevoMassriv"].ToString());
                //producto.EsNuevoSteuben = int.Parse(dr["EsNuevoSteuben"].ToString());
                //producto.EsNuevoOkku = int.Parse(dr["EsNuevoOkku"].ToString());

                producto.SincronizadoParkoiwa = int.Parse(dr["SincronizadoParkoiwa"].ToString());
                producto.SincronizadoMassriv = int.Parse(dr["SincronizadoMassriv"].ToString());
                producto.SincronizadoSteuben = int.Parse(dr["SincronizadoSteuben"].ToString());
                producto.SincronizadoOkku = int.Parse(dr["SincronizadoOkku"].ToString());


                producto.RegistradoEl = (DateTime)dr["RegistradoEl"];
                producto.ModificadoEl = (DateTime)dr["ModificadoEl"];

                producto.UsuarioRegistro = (string)dr["UsuarioRegistro"];
                producto.UsuarioModifico = (string)dr["UsuarioModifico"];

                Items.Add(producto);
            }
            return Items;
        }
        #endregion

    }
}
