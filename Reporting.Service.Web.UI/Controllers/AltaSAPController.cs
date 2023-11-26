using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.SAP;

namespace Reporting.Service.Web.UI.Controllers
{
    public class AltaSAPController : Controller
    {
        private string SkuAux;
        private SAPManager _manager;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public AltaSAPController()
        {
            _manager = new SAPManager();
        }
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
        {
            var jsonResult = this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });

            return jsonResult;
        }
        // GET: AltaSAP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Aprobacion()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Exitoso()
        {
            return View();
        }
        public ActionResult Rechazado()
        {
            return View();
        }

        public ActionResult Anexos()
        {
            return View();
        }

        public ActionResult ReporteBitacora()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AutorizarArticulo(int Sequence)
        {
            try
            {
                _manager.AutorizaArticulo(Sequence);

                var Email = ConfigurationManager.AppSettings["SAP.Alta.Productos.Sistemas.Email"];
                List<string> Copias = new List<string>();
                var ConfigAddress = ConfigurationManager.AppSettings["SAP.Alta.Productos.Sistemas.Copias.Email"];
                String[] Address = ConfigAddress.Split(char.Parse(";"));
                foreach (var item in Address)
                    Copias.Add(item);

                NotificarProductoAutorizado(Sequence, Email, Copias);


                return RedirectToAction("Exitoso");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }

        }
        [HttpGet]
        public ActionResult RechazaArticulo(int Sequence)
        {
            return RedirectToAction("Rechazado");
        }

        public JsonResult GetFamilias()
        {
            try
            {
                var result = _manager.GeFamilia();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetArticulos()
        {
            try
            {
                var result = _manager.GetArticulos();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetMisArticulosPendientes()
        {
            try
            {
                var result = _manager.GetArticulosByUser(User.Identity.GetUserId());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetClasificaciones(int Tipo)
        {
            try
            {
                var result = _manager.GetTipos(Tipo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GeneraCodigoBarras(int Sequence)
        {
            try
            {
                var Registrado = _manager.GeneraCodigoBarra(Sequence);

                return this.JsonResponse(Registrado);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult Sincronizar(int Sequence)
        {
            try
            {
                var producto = _manager.GetProducto(Sequence);

                //Parkoiwa
                if (producto.SincronizadoParkoiwa == 0)
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Parkoiwa"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Parkoiwa.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Parkoiwa.SapPassword"];

                    _manager.SyncSAPProduct(producto, DB, Usuario, Password, producto.EsNuevoParkoiwa == 1 ? true : false, EnumBaseDatos.Parkoiwa);
                }

                //Massriv
                if (producto.SincronizadoMassriv == 0)
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Massriv"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Massriv.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Massriv.SapPassword"];

                    _manager.SyncSAPProduct(producto, DB, Usuario, Password, producto.EsNuevoMassriv == 1 ? true : false, EnumBaseDatos.Massriv);
                }

                //Steuben
                if (producto.SincronizadoSteuben == 0)
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Steuben"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Steuben.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Steuben.SapPassword"];

                    _manager.SyncSAPProduct(producto, DB, Usuario, Password, producto.EsNuevoSteuben == 1 ? true : false, EnumBaseDatos.Steuben);
                }

                //Okku
                if (producto.SincronizadoOkku == 0)
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Okku"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Okku.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Okku.SapPassword"];

                    _manager.SyncSAPProduct(producto, DB, Usuario, Password, producto.EsNuevoOkku == 1 ? true : false, EnumBaseDatos.Okku);
                }

                var Email = ConfigurationManager.AppSettings["SAP.Alta.Productos.Sincronizado.Email"];
                List<string> Copias = new List<string>();
                var ConfigAddress = ConfigurationManager.AppSettings["SAP.Alta.Productos.Sincronizado.CC.Email"];
                String[] Address = ConfigAddress.Split(char.Parse(";"));
                foreach (var item in Address)
                    Copias.Add(item);

                NotificarProductoAutorizado(Sequence, Email, Copias);

                return this.JsonResponse("Los datos se sincronizaron correctamente");

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult SincronizarAnexo(int Sequence)
        {
            try
            {
                var Anexo = _manager.GetAnexo(Sequence);

                //if(Anexo.Parkoiwa == 0)
                //    _manager.SyncSAPAnexo(Anexo, "Pruebas_Parkoiwa", "sap_sis", "sissap", Server.MapPath("~/Documentos/"));

                //Parkoiwa
                if (Anexo.Parkoiwa == 0)
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Parkoiwa"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Parkoiwa.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Parkoiwa.SapPassword"];

                    _manager.SyncSAPAnexo(Anexo, DB, Usuario, Password, Server.MapPath("~/Documentos/"), EnumBaseDatos.Parkoiwa);
                }

                //Massriv
                if (Anexo.Massriv == 0)
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Massriv"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Massriv.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Massriv.SapPassword"];

                    _manager.SyncSAPAnexo(Anexo, DB, Usuario, Password, Server.MapPath("~/Documentos/"), EnumBaseDatos.Massriv);
                }

                //Steuben
                if (Anexo.Steuben == 0)
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Steuben"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Steuben.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Steuben.SapPassword"];

                    _manager.SyncSAPAnexo(Anexo, DB, Usuario, Password, Server.MapPath("~/Documentos/"), EnumBaseDatos.Steuben);
                }

                //Okku
                if (Anexo.Okku == 0)
                {
                    var DB = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Okku"];
                    var Usuario = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Okku.SapUser"];
                    var Password = ConfigurationManager.AppSettings["SAP.Alta.DbCompany.Okku.SapPassword"];

                    _manager.SyncSAPAnexo(Anexo, DB, Usuario, Password, Server.MapPath("~/Documentos/"), EnumBaseDatos.Okku);
                }


                return this.JsonResponse("Los datos se sincronizaron correctamente");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetProveedor()
        {
            try
            {
                var result = _manager.FindProveedor();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetNOM()
        {
            try
            {
                var result = _manager.GetNOM();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetAduanas()
        {
            try
            {
                var result = _manager.GetAduanas();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult FindProducto(string Sku)
        {
            try
            {
                var result = _manager.FindProducto(Sku);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult FindMiProducto(int Sequence)
        {
            try
            {
                var result = _manager.FindMiProducto(Sequence);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GETAnexos(string Sku)
        {
            try
            {
                var result = _manager.GetAnexos(Sku);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GETAnexosPendientes()
        {
            try
            {
                var result = _manager.GetAnexosPendientes();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GuardarArticulo(FormCollection collection)
        {
            try
            {
                string TipoProducto = string.Empty;

                string Sku = collection["Sku"].ToString();

                int TipoP = int.Parse(collection["TipoProducto"].ToString());
                switch (TipoP)
                {
                    case 1:
                        TipoProducto = "NUEVO";
                        break;
                    case 2:
                        TipoProducto = "DESCONTINUADO";
                        break;
                    case 3:
                        TipoProducto = "ACTIVO";
                        break;
                }

                string Marca = collection["Marca"].ToString();
                int Familia = int.Parse(collection["Familia"].ToString());
                string Categoria = collection["Categoria"].ToString();
                string Clasificacion = collection["Clasificacion"].ToString();
                string Tipo = collection["Tipo"].ToString();
                string TipoEmpaque = collection["TipoEmpaque"].ToString();
                string Inner = collection["Inner"].ToString();
                string Master = collection["Master"].ToString();
                string Descripcion = collection["Descripcion"].ToString();
                string DescripcionIngles = collection["DescripcionIngles"].ToString();
                string Accesorios = collection["Accesorios"].ToString();
                string SkuFabricante = collection["SkuFabricante"].ToString();
                string CodigoProveedor = collection["CodigoProveedor"].ToString();

                string Barcode = collection["Barcode"].ToString();
                string BarcodeInner = collection["BarcodeInner"].ToString();
                string BarcodeMaster = collection["BarcodeMaster"].ToString();

                decimal Largo = decimal.Parse(collection["Largo"].ToString());
                decimal Ancho = decimal.Parse(collection["Ancho"].ToString());
                decimal Alto = decimal.Parse(collection["Alto"].ToString());
                decimal Peso = decimal.Parse(collection["Peso"].ToString());

                string Nom = collection["Nom"].ToString();
                int CodigoSAT = int.Parse(collection["CodigoSat"].ToString());
                string Fraccion = collection["Fraccion"].ToString();
                int Aduana = int.Parse(collection["Aduana"].ToString());
                string DescripcionAduana = collection["DescripcionAduana"].ToString();

                int MaximoCompra = int.Parse(collection["MaximoCompra"].ToString() == "" ? "0" : collection["MaximoCompra"].ToString());
                int MinimoCompra = int.Parse(collection["MinimoCompra"].ToString() == "" ? "0" : collection["MinimoCompra"].ToString());

                var EspecificacionesImg = Request.Files["EspecificacionesImg"];
                var ProductoImg = Request.Files["ProductoImg"];

                if (EspecificacionesImg != null && EspecificacionesImg.ContentLength > 0)
                    SaveImageServer(EspecificacionesImg, Sku + ".jpg", @"\\SERWEBGRUPOMASS\Productos\Especificaciones");

                if (ProductoImg != null && ProductoImg.ContentLength > 0)
                {
                    SaveImageServer(ProductoImg, Sku + ".jpg", @"\\SERWEBGRUPOMASS\Products\1_SAP");
                    SaveImageServer(ProductoImg, Sku + ".jpg", @"\\192.168.2.194\imagenesSap");
                }

                var Estatus = "";
                if (Barcode == "")
                {
                    Estatus = "NUEVO";
                }
                else
                {
                    if (TipoP == 2)
                    {
                        Estatus = "DESCONTINUADO";
                    }
                    else
                    {
                        Estatus = "MODIFICADO";
                    }
                }

                // validar si existe el sku 
                var response = _manager.GetProductoBySku(Sku);
                if (response != null)
                {
                    #region validar infor recibida contra lo que esta registrado                    

                    response.UsuarioModifico = User.Identity.GetUserId();

                    if (response.TipoProducto != TipoProducto)
                    {
                        response.TipoProducto = TipoProducto;
                    }
                    else
                    {
                        response.TipoProducto = null;
                    }

                    if (response.Marca != Marca)
                    {
                        response.Marca = Marca;
                    }
                    else
                    {
                        response.Marca = null;
                    }

                    if (response.Familia != Familia.ToString())
                    {
                        response.Familia = Familia.ToString();
                    }
                    else
                    {
                        response.Familia = null;
                    }

                    if (response.Categoria != Categoria)
                    {
                        response.Categoria = Categoria;
                    }
                    else
                    {
                        response.Categoria = null;
                    }

                    if (response.Clasificacion != Clasificacion)
                    {
                        response.Clasificacion = Clasificacion;
                    }
                    else
                    {
                        response.Clasificacion = null;
                    }

                    if (response.Tipo != Tipo)
                    {
                        response.Tipo = Tipo;
                    }
                    else
                    {
                        response.Tipo = null;
                    }

                    if (response.TipoEmpaque != TipoEmpaque)
                    {
                        response.TipoEmpaque = TipoEmpaque;
                    }
                    else
                    {
                        response.TipoEmpaque = null;
                    }

                    if (response.CantInner != Inner)
                    {
                        response.CantInner = Inner;
                    }
                    else
                    {
                        response.CantInner = null;
                    }

                    if (response.CantMaster != Master)
                    {
                        response.CantMaster = Master;
                    }
                    else
                    {
                        response.CantMaster = null;
                    }

                    if (response.DescripcionComercial != Descripcion)
                    {
                        response.DescripcionComercial = Descripcion;
                    }
                    else
                    {
                        response.DescripcionComercial = null;
                    }

                    if (response.DescripcionIngles != DescripcionIngles)
                    {
                        response.DescripcionIngles = DescripcionIngles;
                    }
                    else
                    {
                        response.DescripcionIngles = null;
                    }

                    if (response.Accesorios != Accesorios)
                    {
                        response.Accesorios = Accesorios;
                    }
                    else
                    {
                        response.Accesorios = null;
                    }

                    if (response.SkuFabricante != SkuFabricante)
                    {
                        response.SkuFabricante = SkuFabricante;
                    }
                    else
                    {
                        response.SkuFabricante = null;
                    }

                    if (response.CodigoProveedor != CodigoProveedor)
                    {
                        response.CodigoProveedor = CodigoProveedor;
                    }
                    else
                    {
                        response.CodigoProveedor = null;
                    }

                    if (response.Barcode != Barcode)
                    {
                        response.Barcode = Barcode;
                    }
                    else
                    {
                        response.Barcode = null;
                    }

                    if (response.BarcodeInner != BarcodeInner)
                    {
                        response.BarcodeInner = BarcodeInner;
                    }
                    else
                    {
                        response.BarcodeInner = null;
                    }

                    if (response.BarcodeMaster != BarcodeMaster)
                    {
                        response.BarcodeMaster = BarcodeMaster;
                    }
                    else
                    {
                        response.BarcodeMaster = null;
                    }

                    if (response.Largo != Largo.ToString())
                    {
                        response.Largo = Largo.ToString();
                    }
                    else
                    {
                        response.Largo = null;
                    }

                    if (response.Ancho != Ancho.ToString())
                    {
                        response.Ancho = Ancho.ToString();
                    }
                    else
                    {
                        response.Ancho = null;
                    }

                    if (response.Alto != Alto.ToString())
                    {
                        response.Alto = Alto.ToString();
                    }
                    else
                    {
                        response.Alto = null;
                    }

                    if (response.Peso != Peso.ToString())
                    {
                        response.Peso = Peso.ToString();
                    }
                    else
                    {
                        response.Peso = null;
                    }

                    if (response.Nom != Nom)
                    {
                        response.Nom = Nom;
                    }
                    else
                    {
                        response.Nom = null;
                    }

                    if (response.CodigoSAT != CodigoSAT.ToString())
                    {
                        response.CodigoSAT = CodigoSAT.ToString();
                    }
                    else
                    {
                        response.CodigoSAT = null;
                    }

                    if (response.Franccion != Fraccion)
                    {
                        response.Franccion = Fraccion;
                    }
                    else
                    {
                        response.Franccion = null;
                    }

                    if (response.Aduanas != Aduana.ToString())
                    {
                        response.Aduanas = Aduana.ToString();
                    }
                    else
                    {
                        response.Aduanas = null;
                    }

                    if (response.DescripcionAduana != DescripcionAduana)
                    {
                        response.DescripcionAduana = DescripcionAduana;
                    }
                    else
                    {
                        response.DescripcionAduana = null;
                    }

                    if (response.Minimo != MinimoCompra.ToString())
                    {
                        response.Minimo = MinimoCompra.ToString();
                    }
                    else
                    {
                        response.Minimo = null;
                    }

                    if (response.Maximo != MaximoCompra.ToString())
                    {
                        response.Maximo = MaximoCompra.ToString();
                    }
                    else
                    {
                        response.Maximo = null;
                    }

                    if (response.Estatus != Estatus)
                    {
                        response.Estatus = Estatus;
                    }
                    else
                    {
                        response.Estatus = null;
                    }

                    

                    #endregion
                }

                var sequence = _manager.AddArticulo(Sku, TipoProducto, Marca, Familia, Categoria, Clasificacion, Tipo, TipoEmpaque, Inner, Master, Descripcion, DescripcionIngles, Accesorios, SkuFabricante, CodigoProveedor, MaximoCompra, MinimoCompra, Largo, Ancho, Alto, Peso, Nom, CodigoSAT, Fraccion, Aduana, DescripcionAduana, User.Identity.GetUserId(), Barcode, BarcodeInner, BarcodeMaster, Estatus);


                if(response.Sequence == 0 || string.IsNullOrEmpty(response.Sequence.ToString()))
                {
                    response.Sequence = sequence;
                    response.Sku = Sku;
                    response.UsuarioRegistro = User.Identity.GetUserId();
                }

                _manager.AddArticuloHist(response);

                var ListaCCEmail = new List<string>();

                // obtiene el correo del usuario en caso de que venga diferente
                //var Email = _manager.GetEmailById(User.Identity.GetUserId());
                //if (!string.IsNullOrEmpty(Email))
                //{
                //    ListaCCEmail.Add(Email);
                //}
                string AddressAutorizaSecundario = ConfigurationManager.AppSettings["SAP.Alta.Productos.Autoriza.Email.Secundario"].ToString();

                ListaCCEmail.Add(AddressAutorizaSecundario);

                NotificarProductoNuevo(sequence, ConfigurationManager.AppSettings["SAP.Alta.Productos.Autoriza.Email"], Estatus, ListaCCEmail);

                return this.JsonResponse(sequence);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        

        [HttpPost, ValidateInput(false)]
        public JsonResult GuardarAnexo(FormCollection collection)
        {
            try
            {
                string Sku = collection["SkuAnexo"].ToString();
                var Anexo = Request.Files["Anexo"];
                var Archivo = Anexo.FileName;
                var Nombre = "";

                var TipoArchivo = Anexo.ContentType == "application/pdf" ? "PDF" : "JPG";

                if (Anexo != null && Anexo.ContentLength > 0)
                {
                    if (TipoArchivo == "PDF")
                    {
                        Nombre = Archivo.Replace(".pdf", "");
                        SavePdf(Anexo, Archivo, Server.MapPath("~/Documentos/"));

                    }
                    else
                    {
                        Nombre = Archivo.Replace(".jpg", "");
                        SaveImageServer(Anexo, Archivo, Server.MapPath("~/Documentos/"));

                    }
                }

                var sequence = _manager.AddAnexo(Sku, Nombre, Archivo, TipoArchivo, User.Identity.GetUserId());

                return this.JsonResponse(sequence);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public void SaveImageServer(HttpPostedFileBase file, string Name, string Ruta)
        {
            if (file != null && file.ContentLength > 0)
            {
                var stream = file.InputStream;
                var path = Path.Combine(Ruta, Name);
                var img = Image.FromStream(stream);
                img.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

        }
        public void SavePdf(HttpPostedFileBase file, string Name, string Ruta)
        {
            file.SaveAs(Path.Combine(Ruta, Name));
        }
        static string GetMimeType(string ext)
        {
            //    CodecName FilenameExtension FormatDescription MimeType 
            //    .BMP;*.DIB;*.RLE BMP ==> image/bmp 
            //    .JPG;*.JPEG;*.JPE;*.JFIF JPEG ==> image/jpeg 
            //    *.GIF GIF ==> image/gif 
            //    *.TIF;*.TIFF TIFF ==> image/tiff 
            //    *.PNG PNG ==> image/png 
            switch (ext.ToLower())
            {
                case ".bmp":
                case ".dib":
                case ".rle":
                    return "image/bmp";

                case ".jpg":
                case ".jpeg":
                case ".jpe":
                case ".fif":
                    return "image/jpeg";

                case "gif":
                    return "image/gif";
                case ".tif":
                case ".tiff":
                    return "image/tiff";
                case "png":
                    return "image/png";
                default:
                    return "image/jpeg";
            }
        }
        static ImageCodecInfo GetEncoderInfo(string mimeType)
        {

            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();

            ImageCodecInfo encoder = (from enc in encoders
                                      where enc.MimeType == mimeType
                                      select enc).First();
            return encoder;

        }

        private string ArmarHtmlBitacoraArticulo(Producto item)
        {
            #region ArmarHtml
            StringBuilder builder = new StringBuilder();
            string html = string.Empty;
            if (item != null)
            {
                string htmlBitacora = CrearHtmlBitacoraCampoNotNull(item);

                html = @"<p style='text-align: center;'>
                                <span style='font-size: 12pt;'>
                                    <strong>
                                        <span style='color: #ff0000;'>
                                            Notificacion: Se ha realizado modificaciones al articulo
                                        </span>
                                    </strong>
                                </span>
                            </p>
                            <p style='text-align: center;'>
                                <span style='font-size: 12pt;'>
                                    <strong>
                                        <span style='color: #ff0000;'>
                                            Detalle de bitacora de datos modificados
                                        </span>
                                    </strong>
                                </span>
                            </p>
                            <table>        
                                <tr><td>Articulo editado por : " + item.UsuarioModifico + @"</td></tr>
                                " + htmlBitacora + @"
                            </table>";
            }
            #endregion
            return html;
        }

        private string CrearHtmlBitacoraCampoNotNull(Producto response)
        {
            StringBuilder builder = new StringBuilder();
            if (!string.IsNullOrEmpty(response.TipoProducto))
            {
                builder.Append("<tr><td>Tipo de Producto: " + response.TipoProducto + @"</td></tr>");
            }
            if (!string.IsNullOrEmpty(response.Marca))
            {
                builder.Append("<tr><td>Marca: " + response.Marca + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Familia) && response.Familia != "0")
            {
                builder.Append("<tr><td>Familia: " + response.Familia + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Categoria))
            {
                builder.Append("<tr><td>Categoria: " + response.Categoria + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Clasificacion))
            {
                builder.Append("<tr><td>Clasificacion: " + response.Clasificacion + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Tipo))
            {
                builder.Append("<tr><td>Tipo: " + response.Tipo + @"</td></tr>");
            }
            if (!string.IsNullOrEmpty(response.TipoEmpaque))
            {
                builder.Append("<tr><td>Tipo de Empaque: " + response.TipoEmpaque + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.CantInner))
            {
                builder.Append("<tr><td>TINNER (PZA): " + response.CantInner + @" PZA</td></tr>");
            }
            if (!string.IsNullOrEmpty(response.CantMaster))
            {
                builder.Append("<tr><td>MASTER (PZA): " + response.CantMaster + @" PZA</td></tr>");
            }
            if (!string.IsNullOrEmpty(response.DescripcionComercial))
            {
                builder.Append("<tr><td>Descripcion Comercial: " + response.DescripcionComercial + @"</td></tr>");
            }
            if (!string.IsNullOrEmpty(response.DescripcionIngles))
            {
                builder.Append("<tr><td>Descripcion en Ingles: " + response.DescripcionIngles + @"</td></tr>");
            }
            if (!string.IsNullOrEmpty(response.Accesorios))
            {
                builder.Append("<tr><td>Accesorios: " + response.Accesorios + @"</td></tr>");
            }
            if (!string.IsNullOrEmpty(response.SkuFabricante))
            {
                builder.Append("<tr><td>Descripcion SKU Fabricante: " + response.SkuFabricante + @"</td></tr>");
            }
            if (!string.IsNullOrEmpty(response.CodigoProveedor))
            {
                builder.Append("<tr><td>CÓDIGO DEL PROVEEDOR: " + response.CodigoProveedor + @" </td></tr>");
            }
            if (!string.IsNullOrEmpty(response.Barcode))
            {
                builder.Append("<tr><td>Descripcion SKU Fabricante: " + response.Barcode + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.BarcodeInner))
            {
                builder.Append("<tr><td>BarCode Inner: " + response.BarcodeInner + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.BarcodeMaster))
            {
                builder.Append("<tr><td>BarCode Master: " + response.BarcodeMaster + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Largo))
            {
                builder.Append("<tr><td>Sku Largo: " + response.Largo + @" cm.</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Ancho))
            {
                builder.Append("<tr><td>Sku Ancho: " + response.Ancho + @" cm.</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Alto))
            {
                builder.Append("<tr><td>Sku Alto: " + response.Alto + @" cm.</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Peso))
            {
                builder.Append("<tr><td>Peso en Gramos: " + response.Peso + @" g.</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Nom))
            {
                builder.Append("<tr><td>Norma MEX Vigente: " + response.Nom + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.CodigoSAT) && response.CodigoSAT != "0")
            {
                builder.Append("<tr><td>No. Codigo SAT: " + response.CodigoSAT + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Franccion))
            {
                builder.Append("<tr><td>Fracción Arancelaria: " + response.Franccion + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Aduanas))
            {
                builder.Append("<tr><td>Grupo de Aduana: " + response.Aduanas + @" % IGI</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.DescripcionAduana))
            {
                builder.Append("<tr><td>Descripción de Aduana: " + response.DescripcionAduana + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Minimo) && response.Minimo != "0")
            {
                builder.Append("<tr><td>Minimo: " + response.Minimo + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Maximo) && response.Maximo != "0")
            {
                builder.Append("<tr><td>Maximo: " + response.Maximo + @"</td></tr>");
            }

            if (!string.IsNullOrEmpty(response.Estatus))
            {
                builder.Append("<tr><td>Estatus del producto: " + response.Estatus + @"</td></tr>");
            }
            return builder.ToString();
        }

        public bool NotificarProductoNuevo(int Sequence, string Email, string Estatus, List<string> Copias = null)
        {

            string ruta = Server.MapPath("~/Reports/AltaSAP.rpt");

            var result = _manager.GetProductoDT(Sequence);

            string Sku = result.Rows[0]["Sku"].ToString();
            var resultBitacora = _manager.GetProductoHistoryBySku(Sku, User.Identity.GetUserId());

            string htmlbitacora = ArmarHtmlBitacoraArticulo(resultBitacora);
            if (resultBitacora != null)
            {


            }
            string FileName = string.Format(Estatus + " de Producto [" + Sku + "] #" + Sequence.ToString() + ".pdf");

            ReportDocument report = new ReportDocument();
            report.FileName = ruta;
            report.Load(ruta);
            report.Database.Tables[0].SetDataSource(result);

            report.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Documentos/" + FileName + ""));

            Services.Email.Service EmailSer = new Services.Email.Service();
            var Enviado = EmailSer.SendEmail(Email, Email, Estatus + " producto [ " + Sku + "]",
                @"<table width='608'>
                            <tbody>
                                <tr>
                                    <td>
                                        <span style='font-size: 12pt;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/encabezado.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='text-align: left;'><br /><br/>
                                        <p style='text-align: center;'>
                                            <span style='font-size: 12pt;'>
                                                <strong>
                                                    <span style='color: #ff0000;'>
                                                        " + Estatus + @" producto registrado.<br />
                                                    </span>
                                                </strong>
                                            </span>
                                        </p>
                                        <p style='text-align: justify;'>
                                            <span style='font-size: 12pt; font-family: helvetica;'>
                                                 Estimado usuario,<br /><br />
                                                 Por este medio le informamos que se ha registrado el producto con el SKU [" + Sku + @"] y requiere de su aprobación para continuar con el proceso.<br /><br />
                                                 Nota: Adjunto podrá encontrar el pdf con los detalles del producto.
                                            </span>
                                        </p>
                                        " + htmlbitacora + @"
                                        <br /><br /><br />
                                    </td>
                                </tr>
								<tr>
									<td>
									<table>
									<tr>
									<td><a href='" + this.Url.Action("AutorizarArticulo", "AltaSAP", null, this.Request.Url.Scheme) + @"?Sequence=" + Sequence.ToString() + @"' style='display: block;width: 115px;height: 25px;background: #5FE15B;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;line-height: 25px;'>APROBAR</a></td>
									<td><a href='" + this.Url.Action("RechazaArticulo", "AltaSAP", null, this.Request.Url.Scheme) + @"?Sequence=" + Sequence.ToString() + @"' style='display: block;width: 115px;height: 25px;background: #E92C41;padding: 10px;text-align: center;border-radius: 5px;color: white;font-weight: bold;line-height: 25px;'>RECHAZAR</a></td>
									</tr>
									</table>
									<td>
								</tr>
                                <tr>
                                    <td><br /><br /><br />
                                        <span style='font-size: 12pt; font-family: helvetica;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/footer.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>", Copias, FileName, Server.MapPath("~/Documentos/" + FileName + ""));
            return true;
        }
        public bool NotificarProductoAutorizado(int Sequence, string Email, List<string> Copias)
        {

            string ruta = Server.MapPath("~/Reports/AltaSAP.rpt");

            var result = _manager.GetProductoDT(Sequence);

            string Sku = result.Rows[0]["Sku"].ToString();
            string Estatus = result.Rows[0]["Estatus"].ToString();
            string FileName = string.Format("Producto " + Estatus + " - Autorizado [" + Sku + "] #" + Sequence.ToString() + ".pdf");

            ReportDocument report = new ReportDocument();
            report.FileName = ruta;
            report.Load(ruta);
            report.Database.Tables[0].SetDataSource(result);

            report.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Documentos/" + FileName + ""));

            Services.Email.Service EmailSer = new Services.Email.Service();
            var Enviado = EmailSer.SendEmail(Email, Email, "Producto " + Estatus + " autorizado [ " + Sku + "]",
                @"<table width='608'>
                            <tbody>
                                <tr>
                                    <td>
                                        <span style='font-size: 12pt;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/encabezado.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='text-align: left;'><br /><br />
                                        <p style='text-align: center;'>
                                            <span style='font-size: 12pt;'>
                                                <strong>
                                                    <span style='color: #ff0000;'>
                                                        Producto registrado en SAP.<br />
                                                    </span>
                                                </strong>
                                            </span>
                                        </p>
                                        <p style='text-align: justify;'>
                                            <span style='font-size: 12pt; font-family: helvetica;'>
                                                 Estimado usuario,<br /><br />
                                                 Por este medio le informamos que se ha registrado el producto con el SKU [" + Sku + @"].<br /><br />
                                                 Nota: Adjunto podrá encontrar el pdf con los detalles del producto.
                                            </span>
                                        </p>
                                        <br /><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td><br /><br /><br />
                                        <span style='font-size: 12pt; font-family: helvetica;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/footer.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>", Copias, FileName, Server.MapPath("~/Documentos/" + FileName + ""));
            return true;
        }


        public JsonResult GetReporteBitacoraArticulos(DateTime Del, DateTime Al)
        {
            try
            {
                ////Primero obtenemos el día actual
                //DateTime date = DateTime.Now;

                ////Asi obtenemos el primer dia del mes actual
                //DateTime FechaInicio = new DateTime(date.Year, date.Month-1, 1);
                ////Y de la siguiente forma obtenemos el ultimo dia del mes
                ////agregamos 1 mes al objeto anterior y restamos 1 día.
                //DateTime FechaFin = FechaInicio.AddMonths(1).AddSeconds(-1);


                var result = _manager.GetReporteArticulosModificacion(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

    }

    

}
