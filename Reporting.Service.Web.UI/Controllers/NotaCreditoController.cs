using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.Almacen.AlmacenPartida;
using Reporting.Service.Core.Common;
using Reporting.Service.Core.NotaCredito;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;

namespace Reporting.Service.Web.UI.Controllers
{
    public class NotaCreditoController : JsonController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext context;
        public NotaCreditoController()
        {
            context = new ApplicationDbContext();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        // GET: NotaCredito
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            CommonManager manager = new CommonManager();
            var result = manager.GetDetalleUsuario(User.Identity.GetUserId());

            UserModel model = new UserModel();
            model.Roles = GetRoles();
            model.Area = result.Area;
            model.Nombre = result.Usuario;
            model.CodigoEmpleado = result.CodigoEmpleado;

            return View(model);

        }

        public ActionResult Gestion()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            CommonManager manager = new CommonManager();
            var result = manager.GetDetalleUsuario(User.Identity.GetUserId());
            UserModel model = new UserModel();
            model.Roles = GetRoles();
            model.Area = result.Area;
            model.Nombre = result.Usuario;
            model.CodigoEmpleado = result.CodigoEmpleado;
            return View(model);
        }

        public IList<Rol> GetRoles()
        {
            List<Rol> roles = new List<Rol>();
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                foreach (string Item in s)
                {
                    switch (Item.ToString())
                    {
                        case "Administrador":
                            roles.Add(Rol.Administrador);
                            break;
                        case "Compras":
                            roles.Add(Rol.Compras);
                            break;
                        case "Tráfico":
                            roles.Add(Rol.Trafico);
                            break;
                        case "Ventas":
                            roles.Add(Rol.Ventas);
                            break;
                        case "Credito":
                            roles.Add(Rol.Credito);
                            break;
                        case "Dirección":
                            roles.Add(Rol.Direccion);
                            break;
                        case "Tiendas":
                            roles.Add(Rol.Tiendas);
                            break;
                        case "Finanzas":
                            roles.Add(Rol.Finanzas);
                            break;
                        case "Ecommerce":
                            roles.Add(Rol.Ecommerce);
                            break;
                        case "Business":
                            roles.Add(Rol.Business);
                            break;
                        case "Asistente":
                            roles.Add(Rol.Asistente);
                            break;
                        case "Precios":
                            roles.Add(Rol.Precios);
                            break;
                        case "Regional":
                            roles.Add(Rol.Regional);
                            break;
                        case "Inventarios":
                            roles.Add(Rol.Inventarios);
                            break;
                        case "Papeleria":
                            roles.Add(Rol.Papeleria);
                            break;
                        case "AdministracionPapeleria":
                            roles.Add(Rol.AdministracionPapeleria);
                            break;
                        case "Conciliacion":
                            roles.Add(Rol.Conciliacion);
                            break;
                        case "PedidosTienda":
                            roles.Add(Rol.PedidosTienda);
                            break;
                        case "Almacén":
                            roles.Add(Rol.Almacen);
                            break;
                        case "Gerencia":
                            roles.Add(Rol.Gerencia);
                            break;
                        case "Retail":
                            roles.Add(Rol.Retail);
                            break;
                        case "RetailAdmin":
                            roles.Add(Rol.RetailAdmin);
                            break;
                    }
                }
            }
            return roles;
        }

        public ActionResult NotasCreditoReporte()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            CommonManager manager = new CommonManager();
            var result = manager.GetDetalleUsuario(User.Identity.GetUserId());

            UserModel model = new UserModel();
            model.Roles = GetRoles();
            model.Area = result.Area;
            model.Nombre = result.Usuario;
            model.CodigoEmpleado = result.CodigoEmpleado;

            return View(model);
            //return View();
        }

        public JsonResult GetReporte(NotaCreditoCriteria criteria)
        {
            try
            {
                NotaCreditoManager manager = new NotaCreditoManager();
                var result = manager.FindPagedItems(criteria);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // para add imagen 
        // getImagenes Carrucel nota credito by folio
        public JsonResult GetImagenenNotaCredito(string FolioNC)
        {
            try
            {
                Core.NotaCredito.NotaCreditoManager manager = new Core.NotaCredito.NotaCreditoManager();
                var result = manager.FindNotaCreditoImagen(int.Parse(FolioNC));
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // guardar evidencia Nota de credito en ver imagenes
        public JsonResult AddNotaCreditoEvidencia(FormCollection collection)
        {
            bool Add = false;
            Core.NotaCredito.NotaCredito item = new Core.NotaCredito.NotaCredito();
            try
            {
                item.Identifier = int.Parse(collection["NotaCreditoId"].ToString());
                var lista = new List<Core.NotaCredito.NotaCreditoImagen>();
                foreach (var key in this.Request.Files.AllKeys)
                {
                    var file = this.Request.Files[key];
                    if (file != null && file.ContentLength > 0)
                    {
                        EvidenciaKind tipo;
                        string FileExtension = Path.GetExtension(file.FileName);
                        string mimeType = MimeMapping.GetMimeMapping(file.FileName);
                        switch (mimeType)
                        {
                            case "image/png":
                            case "image/jpeg":
                            case "image/x-icon":
                            case "image/gif":
                            case "image/bmp":
                                tipo = EvidenciaKind.Imagen;
                                break;
                            case "application/pdf":
                                tipo = EvidenciaKind.PDF;
                                break;
                            default:
                                tipo = EvidenciaKind.Desconocido;
                                throw new Exception("La evidencia ingresado no contiene un formato valido.");
                        }
                        HttpPostedFileBase imageEncoder = file;
                        var stream = imageEncoder.InputStream;
                        byte[] ImageByte;
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            ImageByte = ms.ToArray();
                        }
                        NotaCreditoImagen itemImagen = new NotaCreditoImagen();
                        itemImagen.UserName = User.Identity.Name;
                        itemImagen.ImagenBase64 = Convert.ToBase64String(ImageByte);
                        itemImagen.Tipo = tipo;
                        itemImagen.Extension = FileExtension;
                        itemImagen.FileType = mimeType;

                        lista.Add(itemImagen);
                        stream.Close();
                        stream.Dispose();
                    }
                }
                item.Imagenes = lista;
                Core.NotaCredito.NotaCreditoManager manager = new Core.NotaCredito.NotaCreditoManager();
                if (item.Imagenes.Count > 0)
                {
                    Add = manager.AddImagen(item);
                }
                return this.JsonResponse(Add);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // add evidencia gerencia
        public JsonResult AddNotaCreditoEvidenciaGerencia(FormCollection collection)
        {
            bool Add = false;
            Core.NotaCredito.NotaCredito item = new Core.NotaCredito.NotaCredito();
            try
            {
                item.Identifier = int.Parse(collection["NotaCreditoIdGerencia"].ToString());
                var lista = new List<Core.NotaCredito.NotaCreditoImagen>();
                foreach (var key in this.Request.Files.AllKeys)
                {
                    var file = this.Request.Files[key];
                    if (file != null && file.ContentLength > 0)
                    {
                        EvidenciaKind tipo;
                        string FileExtension = Path.GetExtension(file.FileName);
                        string mimeType = MimeMapping.GetMimeMapping(file.FileName);
                        switch (mimeType)
                        {
                            case "image/png":
                            case "image/jpeg":
                            case "image/x-icon":
                            case "image/gif":
                            case "image/bmp":
                                tipo = EvidenciaKind.Imagen;
                                break;
                            case "application/pdf":
                                tipo = EvidenciaKind.PDF;
                                break;
                            default:
                                tipo = EvidenciaKind.Desconocido;
                                throw new Exception("La evidencia ingresado no contiene un formato valido.");
                        }
                        HttpPostedFileBase imageEncoder = file;
                        var stream = imageEncoder.InputStream;
                        byte[] ImageByte;
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            ImageByte = ms.ToArray();

                        }
                        NotaCreditoImagen itemImagen = new NotaCreditoImagen();

                        itemImagen.UserName = User.Identity.Name;
                        itemImagen.ImagenBase64 = Convert.ToBase64String(ImageByte);
                        itemImagen.Tipo = tipo;
                        itemImagen.Extension = FileExtension;
                        itemImagen.FileType = mimeType;
                        lista.Add(itemImagen);
                    }
                }
                item.Imagenes = lista;
                Core.NotaCredito.NotaCreditoManager manager = new Core.NotaCredito.NotaCreditoManager();
                if (item.Imagenes.Count > 0)
                {
                    Add = manager.AddImagen(item);
                }
                return this.JsonResponse(Add);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // add evidencia inventario
        public JsonResult AddNotaCreditoEvidenciaInventario(FormCollection collection)
        {
            bool Add = false;
            Core.NotaCredito.NotaCredito item = new Core.NotaCredito.NotaCredito();
            try
            {
                item.Identifier = int.Parse(collection["NotaCreditoIdInventario"].ToString());
                var lista = new List<Core.NotaCredito.NotaCreditoImagen>();
                foreach (var key in this.Request.Files.AllKeys)
                {
                    var file = this.Request.Files[key];
                    if (file != null && file.ContentLength > 0)
                    {
                        EvidenciaKind tipo;
                        string FileExtension = Path.GetExtension(file.FileName);
                        string mimeType = MimeMapping.GetMimeMapping(file.FileName);
                        switch (mimeType)
                        {
                            case "image/png":
                            case "image/jpeg":
                            case "image/x-icon":
                            case "image/gif":
                            case "image/bmp":
                                tipo = EvidenciaKind.Imagen;
                                break;
                            case "application/pdf":
                                tipo = EvidenciaKind.PDF;
                                break;
                            default:
                                tipo = EvidenciaKind.Desconocido;
                                throw new Exception("La evidencia ingresado no contiene un formato valido.");
                        }
                        HttpPostedFileBase imageEncoder = file;
                        var stream = imageEncoder.InputStream;
                        byte[] ImageByte;
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            ImageByte = ms.ToArray();

                        }
                        NotaCreditoImagen itemImagen = new NotaCreditoImagen();
                        itemImagen.UserName = User.Identity.Name;
                        itemImagen.ImagenBase64 = Convert.ToBase64String(ImageByte);
                        itemImagen.Tipo = tipo;
                        itemImagen.Extension = FileExtension;
                        itemImagen.FileType = mimeType;
                        lista.Add(itemImagen);
                    }
                }
                item.Imagenes = lista;
                Core.NotaCredito.NotaCreditoManager manager = new Core.NotaCredito.NotaCreditoManager();
                if (item.Imagenes.Count > 0)
                {
                    Add = manager.AddImagen(item);
                }
                return this.JsonResponse(Add);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        // add evidencia direccion
        public JsonResult AddNotaCreditoEvidenciaDireccion(FormCollection collection)
        {
            bool Add = false;
            Core.NotaCredito.NotaCredito item = new Core.NotaCredito.NotaCredito();
            try
            {
                item.Identifier = int.Parse(collection["NotaCreditoIdDireccion"].ToString());

                var lista = new List<Core.NotaCredito.NotaCreditoImagen>();
                foreach (var key in this.Request.Files.AllKeys)
                {
                    var file = this.Request.Files[key];
                    if (file != null && file.ContentLength > 0)
                    {
                        EvidenciaKind tipo;
                        string FileExtension = Path.GetExtension(file.FileName);
                        string mimeType = MimeMapping.GetMimeMapping(file.FileName);
                        switch (mimeType)
                        {
                            case "image/png":
                            case "image/jpeg":
                            case "image/x-icon":
                            case "image/gif":
                            case "image/bmp":
                                tipo = EvidenciaKind.Imagen;
                                break;
                            case "application/pdf":
                                tipo = EvidenciaKind.PDF;
                                break;
                            default:
                                tipo = EvidenciaKind.Desconocido;
                                throw new Exception("La evidencia ingresado no contiene un formato valido.");
                        }
                        HttpPostedFileBase imageEncoder = file;
                        var stream = imageEncoder.InputStream;
                        byte[] ImageByte;
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            ImageByte = ms.ToArray();

                        }
                        NotaCreditoImagen itemImagen = new NotaCreditoImagen();
                        itemImagen.UserName = User.Identity.Name;
                        itemImagen.ImagenBase64 = Convert.ToBase64String(ImageByte);
                        itemImagen.Tipo = tipo;
                        itemImagen.Extension = FileExtension;
                        itemImagen.FileType = mimeType;
                        lista.Add(itemImagen);
                    }
                }
                item.Imagenes = lista;
                Core.NotaCredito.NotaCreditoManager manager = new Core.NotaCredito.NotaCreditoManager();
                if (item.Imagenes.Count > 0)
                {
                    Add = manager.AddImagen(item);
                }
                return this.JsonResponse(Add);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        // add evidencia credito cobranza
        public JsonResult AddNotaCreditoEvidenciaCreditoCobranza(FormCollection collection)
        {
            bool Add = false;
            Core.NotaCredito.NotaCredito item = new Core.NotaCredito.NotaCredito();
            try
            {
                item.Identifier = int.Parse(collection["NotaCreditoIdCobranza"].ToString());
                var lista = new List<Core.NotaCredito.NotaCreditoImagen>();
                foreach (var key in this.Request.Files.AllKeys)
                {
                    var file = this.Request.Files[key];
                    if (file != null && file.ContentLength > 0)
                    {
                        EvidenciaKind tipo;
                        string FileExtension = Path.GetExtension(file.FileName);
                        string mimeType = MimeMapping.GetMimeMapping(file.FileName);
                        switch (mimeType)
                        {
                            case "image/png":
                            case "image/jpeg":
                            case "image/x-icon":
                            case "image/gif":
                            case "image/bmp":
                                tipo = EvidenciaKind.Imagen;
                                break;
                            case "application/pdf":
                                tipo = EvidenciaKind.PDF;
                                break;
                            default:
                                tipo = EvidenciaKind.Desconocido;
                                throw new Exception("La evidencia ingresado no contiene un formato valido.");
                        }
                        HttpPostedFileBase imageEncoder = file;
                        var stream = imageEncoder.InputStream;
                        byte[] ImageByte;
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            ImageByte = ms.ToArray();

                        }
                        NotaCreditoImagen itemImagen = new NotaCreditoImagen();
                        itemImagen.UserName = User.Identity.Name;
                        itemImagen.ImagenBase64 = Convert.ToBase64String(ImageByte);
                        itemImagen.Tipo = tipo;
                        itemImagen.Extension = FileExtension;
                        itemImagen.FileType = mimeType;
                        lista.Add(itemImagen);
                    }
                }
                item.Imagenes = lista;
                Core.NotaCredito.NotaCreditoManager manager = new Core.NotaCredito.NotaCreditoManager();
                if (item.Imagenes.Count > 0)
                {
                    Add = manager.AddImagen(item);
                }
                return this.JsonResponse(Add);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        // nota credito Retail 
        public JsonResult AddNotaCreditoEvidenciaRetail(FormCollection collection)
        {
            bool Add = false;
            Core.NotaCredito.NotaCredito item = new Core.NotaCredito.NotaCredito();
            try
            {
                item.Identifier = int.Parse(collection["NotaCreditoIdentifier"].ToString());
                var lista = new List<Core.NotaCredito.NotaCreditoImagen>();
                foreach (var key in this.Request.Files.AllKeys)
                {
                    var file = this.Request.Files[key];
                    if (file != null && file.ContentLength > 0)
                    {
                        EvidenciaKind tipo;
                        string FileExtension = Path.GetExtension(file.FileName);
                        string mimeType = MimeMapping.GetMimeMapping(file.FileName);
                        switch (mimeType)
                        {
                            case "image/png":
                            case "image/jpeg":
                            case "image/x-icon":
                            case "image/gif":
                            case "image/bmp":
                                tipo = EvidenciaKind.Imagen;
                                break;
                            case "application/pdf":
                                tipo = EvidenciaKind.PDF;
                                break;
                            default:
                                tipo = EvidenciaKind.Desconocido;
                                throw new Exception("La evidencia ingresado no contiene un formato valido.");
                        }
                        HttpPostedFileBase imageEncoder = file;
                        var stream = imageEncoder.InputStream;
                        byte[] ImageByte;
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            ImageByte = ms.ToArray();
                        }
                        NotaCreditoImagen itemImagen = new NotaCreditoImagen();
                        itemImagen.UserName = User.Identity.Name;
                        itemImagen.ImagenBase64 = Convert.ToBase64String(ImageByte);
                        itemImagen.Tipo = tipo;
                        itemImagen.Extension = FileExtension;
                        itemImagen.FileType = mimeType;

                        lista.Add(itemImagen);
                        stream.Close();
                        stream.Dispose();
                    }
                }
                item.Imagenes = lista;
                Core.NotaCredito.NotaCreditoManager manager = new Core.NotaCredito.NotaCreditoManager();
                if (item.Imagenes.Count > 0)
                {
                    Add = manager.AddImagen(item);
                }
                return this.JsonResponse(Add);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }



        public JsonResult GetNotaCreditoInventario(NotaCreditoCriteria criteria)
        {
            try
            {

                NotaCreditoManager manager = new NotaCreditoManager();

                var result = manager.GetNotaCreditoInventario(criteria);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetNotaCreditoAdmin(NotaCreditoCriteria criteria)
        {
            try
            {
                criteria.ItemsPerPage = 10000;
                criteria.TipoUsuario = UserKind.Administrador;
                NotaCreditoManager manager = new NotaCreditoManager();
                var result = manager.FindPagedItems(criteria);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetNotaCreditoDireccion(NotaCreditoCriteria criteria)

        {            
            try
            {
                //criteria.ItemsPerPage = 10000;
                criteria.TipoUsuario = UserKind.Direccion;
                NotaCreditoManager manager = new NotaCreditoManager();
                var result = manager.FindPagedItems(criteria);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetNotaCreditoRetailSerivicio(NotaCreditoCriteria Criteria)
        {
            try
            {
                Criteria.ItemsPerPage = 10000;
                Criteria.TipoUsuario = UserKind.Retail;
                Criteria.OrderType = WikiCore.Data.OrderType.Descending;
                NotaCreditoManager manager = new NotaCreditoManager();
                var result = manager.FindPagedItems(Criteria);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetNotaCreditoItem(int NotaCredito)
        {
            try
            {
                NotaCreditoManager manager = new NotaCreditoManager();
                var result = manager.Find(NotaCredito);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetNotaCreditoStatusSAP()
        {
            try
            {
                NotaCreditoManager manager = new NotaCreditoManager();
                var result = manager.FindPagedItems(new NotaCreditoCriteria()
                {
                    Estatus = 3,
                });

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }





        // PASAR LAS NOTAS DE CREDITO A SAP      
        #region NotaCreditov2
        public ActionResult Index1()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            CommonManager manager = new CommonManager();
            var result = manager.GetDetalleUsuario(User.Identity.GetUserId());

            UserModel model = new UserModel();
            model.Roles = GetRoles();
            model.Area = result.Area;
            model.Nombre = result.Usuario;
            model.CodigoEmpleado = result.CodigoEmpleado;

            return View(model);

        }
        public JsonResult GetConceptosDescuento(string Cliente)
        {
            try
            {
                NotaCreditoManager manager = new NotaCreditoManager();
                var result = manager.GetConceptoDescuento(Cliente);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        #endregion

        #region AddNotaCredito
        public JsonResult AddNotaCredito(NotaCredito item)
        {
            try
            {
                NotaCreditoManager manager = new NotaCreditoManager();
                var result = manager.Add(item);
                if (result)
                {
                    SendNotaCredito(item);
                }
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        #endregion

        #region CorreoNotaCredito
        private void SendNotaCredito(NotaCredito context)
        {
            try
            {
                StringBuilder buider = new StringBuilder();
                string _tipoDocumento = string.Empty, _Asunto = string.Empty;
                switch (context.TipoDocumento)
                {
                    case "I":
                        _tipoDocumento = "Inventario";
                        _Asunto = "NOTA DE CREDITO INVENTARIO";
                        break;
                    case "S":
                        _tipoDocumento = "Servicio";
                        _Asunto = $"NOTA DE CRÉDITO SERVICIO ({context.ConceptoDescuentoDetalle})";
                        break;
                }

                buider.AppendLine("<div>");
                buider.AppendLine($"<h4 style='font-family: arial, sans-serif;'>Cliente:  {context.ClienteName} </h4>");
                buider.AppendLine($"<h5 style='font-family: arial, sans-serif; margin: 0px; padding: 0px;'>Folio origen: {context.FolioOrigen} </h4>");
                buider.AppendLine($"<h5 style='font-family: arial, sans-serif; margin: 0px; padding: 0px;'>Folio destino: " + context.FolioDestino + "</h4>");
                buider.AppendLine($"<h5 style='font-family: arial, sans-serif; margin: 0px; padding: 0px;'>Tipo de documento: {_tipoDocumento} </h4>");
                buider.AppendLine($"<h5 style='font-family: arial, sans-serif; margin: 0px; padding: 0px;'>Concepto de descuento: {context.ConceptoDescuentoDetalle} </h4>");
                buider.AppendLine($"<h5 style='font-family: arial, sans-serif; margin: 0px; padding: 0px;'>Comentario: {context.Comentario} </h4>");
                buider.AppendLine($"<h5 style='font-family: arial, sans-serif; margin: 0px 0px 15px 0px; padding: 0px 0px 15px 0px;'>Fecha de creación: {DateTime.Now} </h4>");
                buider.AppendLine("<table style='font-family: arial, sans-serif; border-collapse: collapse; width: 100%;'>");
                buider.AppendLine("<thead>");
                buider.AppendLine("<tr style='text-align: center; padding: 8px; background-color: rgb(221, 75, 57); color:#FFFFFF'>");
                buider.AppendLine("<th>SKU</th>");
                buider.AppendLine("<th>Cantidad</th>");
                buider.AppendLine("<th>Precio</th>");

                if (context.TipoDocumento == "S")
                {
                    buider.AppendLine("<th>Descuento</th>");
                    buider.AppendLine("<th>Descuento Total</th>");
                }
                if (context.TipoDocumento == "I")
                {
                    buider.AppendLine("<th>Subtotal</th>");
                }

                buider.AppendLine("</tr>");
                buider.AppendLine("</thead>");
                buider.AppendLine("<tbody style='border: 2px solid black; border-collapse: separate; border-spacing: 4px; text-align: center'>");

                decimal SubTotal = 0m, Total = 0m;

                foreach (var item in context.Items)
                {

                    buider.AppendLine("<tr style='text-align: center'>");
                    buider.AppendLine($"<td>{item.ItemCode}</td>");
                    buider.AppendLine($"<td>{item.Cantidad}</td>");
                    buider.AppendLine($"<td>$ {item.Precio.ToString("N3")}</td>");

                    switch (context.TipoDocumento)
                    {
                        case "I":
                            SubTotal = item.Cantidad * item.Precio;

                            buider.AppendLine($"<td>$ {SubTotal.ToString("N3")} </td>");

                            Total += SubTotal;
                            break;
                        case "S":
                            buider.AppendLine($"<td>$ {item.Descuento.ToString("N3")}</td>");

                            SubTotal = item.Cantidad * item.Descuento;

                            buider.AppendLine($"<td>$ {SubTotal.ToString("N3")} </td>");

                            Total += SubTotal;
                            break;
                    }

                    //buider.AppendLine($"<td>$ {Math.Round(SubTotal,2)} </td>");
                    buider.AppendLine("</tr>");
                }

                buider.AppendLine("</tbody>");
                buider.AppendLine("</table>");

                buider.AppendLine("<h4 style='font-family: arial, sans-serif; text-align: right;'>SUBTOTAL SIN IVA: $ " + Total.ToString("N3") + "</h4>");
                buider.AppendLine("<br/>");
                buider.AppendLine($"<h4 style='font-family: arial, sans-serif; text-align: left;'>FOLIO SIE: {context.Identifier}</h4>");
                buider.AppendLine($"<h4 style='font-family: arial, sans-serif; text-align: left;'>Registrado por: {context.Usuario}</h4>");
                buider.AppendLine("</div>");

                Services.Email.Service email = new Services.Email.Service();
                bool EstatusEmail = email.SendEmail(context.Usuario, context.Usuario, _Asunto, buider.ToString(), null, null, null, "Notificaciones Nota de Crédito ");
                if (EstatusEmail)
                {

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mensage {ex.Message}");
                Console.WriteLine($"Mensage {ex.StackTrace}");
            }
        }
        #endregion

        // guardar evidencia Nota de credito en ver imagenes
        public JsonResult AddCreditNoteEvidence(FormCollection collection)
        {
            bool Add = false;
            Core.NotaCredito.NotaCredito item = new Core.NotaCredito.NotaCredito();
            try
            {
                item.Identifier = int.Parse(collection["NotaCreditoIdentifier"].ToString());
                var lista = new List<Core.NotaCredito.NotaCreditoImagen>();
                foreach (var key in this.Request.Files.AllKeys)
                {
                    var file = this.Request.Files[key];
                    if (file != null && file.ContentLength > 0)
                    {
                        EvidenciaKind tipo;
                        string FileExtension = Path.GetExtension(file.FileName);
                        string mimeType = MimeMapping.GetMimeMapping(file.FileName);
                        switch (mimeType)
                        {
                            case "image/png":
                            case "image/jpeg":
                            case "image/x-icon":
                            case "image/gif":
                            case "image/bmp":
                                tipo = EvidenciaKind.Imagen;
                                break;
                            case "application/pdf":
                                tipo = EvidenciaKind.PDF;
                                break;
                            default:
                                tipo = EvidenciaKind.Desconocido;
                                throw new Exception("La evidencia ingresado no contiene un formato valido.");
                        }
                        HttpPostedFileBase imageEncoder = file;
                        var stream = imageEncoder.InputStream;
                        byte[] ImageByte;
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            ImageByte = ms.ToArray();
                        }
                        NotaCreditoImagen itemImagen = new NotaCreditoImagen();
                        itemImagen.UserName = User.Identity.Name;
                        itemImagen.ImagenBase64 = Convert.ToBase64String(ImageByte);
                        itemImagen.Tipo = tipo;
                        itemImagen.Extension = FileExtension;
                        itemImagen.FileType = mimeType;

                        lista.Add(itemImagen);
                        stream.Close();
                        stream.Dispose();
                    }
                }
                item.Imagenes = lista;
                Core.NotaCredito.NotaCreditoManager manager = new Core.NotaCredito.NotaCreditoManager();
                if (item.Imagenes.Count > 0)
                {
                    Add = manager.AddImagen(item);
                }
                return this.JsonResponse(Add);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult GetNotaCreditoAdmin1(NotaCreditoCriteria criteria)
        {
            
            try
            {
                criteria.ItemsPerPage = 10000;
                criteria.TipoUsuario = UserKind.Administrador;
                NotaCreditoManager manager = new NotaCreditoManager();
                var result = manager.FindPagedItems(criteria);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpGet]
        public JsonResult GetAlmacenPartidas()
        {
            try
            {
                AlmacenPartidaManager instance = new AlmacenPartidaManager();
                var result = instance.GetAlmacenPartidas();
                return this.JsonResponse(result);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message); ;
            }
        }


        public JsonResult AddNotaCreditoComentario(int Identifier, string Comentario, string Usuario, int Departamento)
        {
            try
            {
                NotaCreditoManager manager = new NotaCreditoManager();
                var result = manager.AddNotaCreditoComentario(Identifier, Usuario, Comentario, Departamento);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


    }
}