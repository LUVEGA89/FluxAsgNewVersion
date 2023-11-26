using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.Buzon.Area;
using Reporting.Service.Core.Common;
using Reporting.Service.Core.FacturasManager;
using Reporting.Service.Core.Requerimiento;
using Reporting.Service.Core.Requerimiento.Archivo;
using Reporting.Service.Core.Requerimiento.Concepto;
using Reporting.Service.Core.Requerimiento.Solicitud;
using Reporting.Service.Core.Requerimiento.TipoReq;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class RequerimientoController : JsonController
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext context;

        public RequerimientoController()
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

        // GET: Requerimiento
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

            return View();
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

            return View();
        }

        public ActionResult Reporte()
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

            return View();
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

        public ActionResult Configuracion()
        {
            return View();
        }

        #region Requerimientos
        public JsonResult AddRequerimiento(string Identifier, string descripcion, string idArea)
        {
            try
            {
                TipoRequerimientoManager manager = new TipoRequerimientoManager();
                bool respuesta = false;
                if (!string.IsNullOrEmpty(Identifier))
                {
                    respuesta = manager.Update(new TipoRequerimiento()
                    {
                        Identifier = int.Parse(Identifier),
                        Descripcion = descripcion,
                        Area = new Area()
                        {
                            Identifier = int.Parse(idArea)
                        }
                    });
                }
                else
                {
                    respuesta = manager.Add(new TipoRequerimiento()
                    {
                        Descripcion = descripcion,
                        Area = new Area
                        {
                            Identifier = int.Parse(idArea)
                        }
                    });
                }

                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetRequerimientos()
        {
            try
            {
                TipoRequerimientoManager manager = new TipoRequerimientoManager();
                var x = manager.GetRequerimientos(new TipoRequerimientoCriteria());
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetRequerimientosByArea(string idArea)
        {
            try
            {
                TipoRequerimientoManager manager = new TipoRequerimientoManager();
                var x = manager.GetRequerimientos(new TipoRequerimientoCriteria() { Area = int.Parse(idArea) });
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetRequerimiento(string Identifier)
        {
            try
            {
                TipoRequerimientoManager manager = new TipoRequerimientoManager();
                var x = manager.Find(int.Parse(Identifier));
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult EliminarRequerimiento(int Identifier)
        {
            try
            {
                TipoRequerimientoManager manager = new TipoRequerimientoManager();
                var x = manager.Delete(Identifier);
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        #endregion

        #region Conceptos
        public JsonResult AddConcepto(string Identifier, string descripcion, string requerimiento)
        {
            try
            {
                ConceptoManager manager = new ConceptoManager();
                bool respuesta = false;
                if (!string.IsNullOrEmpty(Identifier))
                {
                    respuesta = manager.Update(new Concepto()
                    {
                        Identifier = int.Parse(Identifier),
                        Descripcion = descripcion,
                        TipoRequerimiento = new TipoRequerimiento()
                        {
                            Identifier = int.Parse(requerimiento)
                        }
                    });
                }
                else
                {
                    respuesta = manager.Add(new Concepto()
                    {
                        Descripcion = descripcion,
                        TipoRequerimiento = new TipoRequerimiento()
                        {
                            Identifier = int.Parse(requerimiento)
                        }
                    });
                }

                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetConceptos()
        {
            try
            {
                ConceptoManager manager = new ConceptoManager();
                var x = manager.GetRequerimientos(new ConceptoCritetria());
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetConcepto(string Identifier)
        {
            try
            {
                ConceptoManager manager = new ConceptoManager();
                var x = manager.Find(int.Parse(Identifier));
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetConceptoByRequerimiento(string idRequerimiento)
        {
            try
            {
                ConceptoManager manager = new ConceptoManager();
                var x = manager.GetRequerimientos(new ConceptoCritetria()
                {
                    TipoRequerimiento = int.Parse(idRequerimiento)
                });
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult EliminarConcepto(int Identifier)
        {
            try
            {
                ConceptoManager manager = new ConceptoManager();
                var x = manager.Delete(Identifier);
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        #endregion

        #region Areas Responsable
        public JsonResult GetAreaResponsable(string Identifier)
        {
            try
            {
                AreaManager manager = new AreaManager();
                var x = manager.Find(int.Parse(Identifier));
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        #endregion


        #region Solicitud
        public JsonResult AddSolicitudRequerimiento(string comentario, string idArea, string idRequerimiento, string idConcepto, DateTime fechaRequerida, string registradoPor)
        {
            try
            {
                SolicitudManager manager = new SolicitudManager();
                var Item = new Solicitud()
                {
                    Comentarios = comentario,
                    Area = new Area() { Identifier = int.Parse(idArea) },
                    Requerimiento = new TipoRequerimiento() { Identifier = int.Parse(idRequerimiento) },
                    Concepto = new Concepto() { Identifier = int.Parse(idConcepto) },
                    FechaRequerida = fechaRequerida,
                    RegistradorPor = registradoPor
                };
                var result = manager.Add(Item);

                if (Item.Identifier != 0)
                {
                    EnviarCorreoSolicitante(Item.Identifier);
                    EnviarCorreoResponsableArea(Item.Identifier);
                }

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ActualizarSolicitudRequerimiento(string identifier, DateTime fechaCompromiso, string estatus, string email)
        {
            try
            {
                SolicitudManager manager = new SolicitudManager();
                var Item = new Solicitud()
                {
                    Identifier = int.Parse(identifier),
                    FechaCompromiso = fechaCompromiso,
                    Estatus = (EstatusKind)int.Parse(estatus)
                };
                var result = manager.Update(Item);

                if (Item.Identifier != 0)
                {
                    EnviarCorreoSolicitante(Item.Identifier);
                    EnviarCorreoResponsableArea(Item.Identifier);
                }

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetSolicitudesByUsuario(string usuario)
        {
            bool isRoleAdministrador = false;
            try
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                foreach (var item in s)
                {
                    if(item == "Administrador" || item == "Dirección")
                    {
                        isRoleAdministrador = true;
                        break;
                    }
                }
                SolicitudManager manager = new SolicitudManager();
                List<Solicitud> x = null;

                if (isRoleAdministrador)
                {
                    x = manager.GetSolicitudes(new SolicitudCriteria()
                    {
                        RegistradoPor = null,
                        ResponsableArea = null
                    });
                }
                else
                {
                    x = manager.GetSolicitudes(new SolicitudCriteria()
                    {
                        RegistradoPor = usuario,
                        ResponsableArea = usuario
                    });
                }
                
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetSolicitud(string identifier)
        {
            try
            {
                SolicitudManager manager = new SolicitudManager();
                var x = manager.Find(int.Parse(identifier));
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetSolicitudArchivo(string identifier)
        {
            try
            {
                ArchivoManager manager = new ArchivoManager();
                var x = manager.GetArchivosByFolio(int.Parse(identifier));
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        public JsonResult AddSolicitudArchivo(FormCollection collection)
        {
            bool Add = false;
            try
            {
                int FolioGeneral = int.Parse(collection["Identifier-requerimiento-solicitud"].ToString());
                string Email = collection["Identifier-requerimiento-email"].ToString();

                var lista = new List<Core.Requerimiento.Archivo.Archivo>();
                foreach (var key in this.Request.Files.AllKeys)
                {
                    var file = this.Request.Files[key];
                    if (file != null && file.ContentLength > 0)
                    {
                        Reporting.Service.Core.Requerimiento.Archivo.EvidenciaKind tipo;
                        string FileExtension = Path.GetExtension(file.FileName);
                        string mimeType = MimeMapping.GetMimeMapping(file.FileName);
                        switch (mimeType)
                        {
                            case "image/png":
                            case "image/jpeg":
                            case "image/x-icon":
                            case "image/gif":
                            case "image/bmp":
                                tipo = Core.Requerimiento.Archivo.EvidenciaKind.Imagen;
                                break;
                            case "application/pdf":
                                tipo = Core.Requerimiento.Archivo.EvidenciaKind.PDF;
                                break;
                            default:
                                tipo = Core.Requerimiento.Archivo.EvidenciaKind.Desconocido;
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
                        Archivo itemArchivo = new Archivo();
                        itemArchivo.FolioGeneral = FolioGeneral;
                        itemArchivo.UserName = Email;
                        itemArchivo.ArchivoBase64 = Convert.ToBase64String(ImageByte);
                        itemArchivo.Tipo = tipo;
                        itemArchivo.Extension = FileExtension;
                        itemArchivo.FileType = mimeType;

                        lista.Add(itemArchivo);
                        stream.Close();
                        stream.Dispose();
                    }
                }

                ArchivoManager manager = new ArchivoManager();

                if (lista.Count > 0)
                {
                    foreach (var itemArchivoRow in lista)
                    {
                        Add = manager.AddArchivo(itemArchivoRow);
                    }

                }
                return this.JsonResponse(Add);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        private bool EnviarCorreoSolicitante(int Identifier)
        {
            bool bandera = false;
            SolicitudManager manager = new SolicitudManager();

            var item = manager.Find(Identifier);

            string htmlcode = ArmarCuerpoCorreoSolicitante(item);

            var ListaCorreo = new Correo()
            {
                Para = item.RegistradorPor,
                //CC = "daniel.mateos95@hotmail.com"
            };


            EnviarEmail(htmlcode, ListaCorreo, item.Identifier);
            return bandera;
        }

        public bool EnviarCorreoResponsableArea(int Identifier)
        {
            bool bandera = false;
            SolicitudManager manager = new SolicitudManager();

            var item = manager.Find(Identifier);

            string htmlcode = ArmarCuerpoCorreoResponsable(item);

            var ListaCorreo = new Correo()
            {
                Para = string.IsNullOrEmpty(item.Area.Email) ? WebConfigurationManager.AppSettings["Email.Sistemas"].ToString() : item.Area.Email,
                //CC = "daniel.mateos95@hotmail.com"
            };


            EnviarEmail(htmlcode, ListaCorreo, item.Identifier);

            return bandera;
        }





        public string ArmarCuerpoCorreoSolicitante(Solicitud context)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<div>");
            builder.AppendLine("<h4>Estimado usuario.</h4>");
            builder.AppendLine("<h4></h4>");
            builder.AppendLine($"<h4>Asunto: Aviso de Generación de Solicitud de Requerimiento con Folio: {context.Identifier}</h4>");
            builder.AppendLine("<h4>Detalle de la solicitud:</h4>");
            builder.AppendLine($"<h4>Area dirigida:: { context.Area.Nombre.ToUpper()}</h4>");
            builder.AppendLine($"<h4>Responsable del area: {context.Area.Email}</h4>");
            builder.AppendLine($"<h4>Requerimiento: {context.Requerimiento.Descripcion.ToUpper()}</h4>");
            builder.AppendLine($"<h4>Clasificación: {context.Concepto.Descripcion.ToUpper()}</h4>");
            builder.AppendLine($"<h4>Fecha de solicitud requerida: {context.FechaRequerida}</h4>");

            if (context.Estatus != EstatusKind.Creado)
            {
                builder.AppendLine($"<h4>Estatus: {context.EstatusDescripcion.ToUpper()}</h4>");

                if (context.FechaCompromiso != null)
                {
                    builder.AppendLine($"<h4>Fecha de Compromiso: {context.FechaCompromiso}</h4>");
                }
                builder.AppendLine($"<h4>Se ha actualizado el estatus y fecha de compromiso de la solicitud</h4>");
            }

            builder.AppendLine("<br/>");
            builder.AppendLine($"<h4>Comentarios: {context.Comentarios}</h4>");
            builder.AppendLine("</div>");

            return builder.ToString();
        }

        public string ArmarCuerpoCorreoResponsable(Solicitud context)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<div>");
            builder.AppendLine("<h4>Estimado usuario.</h4>");
            builder.AppendLine("<h4></h4>");
            builder.AppendLine($"<h4>Asunto: Aviso de Generación de Solicitud de Requerimiento con Folio: {context.Identifier}</h4>");
            builder.AppendLine("<h4>Detalle de la solicitud:</h4>");
            builder.AppendLine($"<h4>Solicitado por: { context.RegistradorPor}</h4>");
            builder.AppendLine($"<h4>Area dirigida:: { context.Area.Nombre.ToUpper()}</h4>");
            builder.AppendLine($"<h4>Requerimiento: {context.Requerimiento.Descripcion.ToUpper()}</h4>");
            builder.AppendLine($"<h4>Clasificación: {context.Concepto.Descripcion.ToUpper()}</h4>");
            builder.AppendLine($"<h4>Fecha de solicitud requerida: {context.FechaRequerida}</h4>");


            if (context.Estatus != EstatusKind.Creado)
            {
                builder.AppendLine($"<h4>Estatus: {context.EstatusDescripcion.ToUpper()}</h4>");

                if (context.FechaCompromiso != null)
                {
                    builder.AppendLine($"<h4>Fecha de Compromiso: {context.FechaCompromiso}</h4>");
                }
            }

            builder.AppendLine("<br/>");
            builder.AppendLine($"<h4>Comentarios: {context.Comentarios}</h4>");
            builder.AppendLine("</div>");

            return builder.ToString();
        }

        //Envia correo electrónico junto con las facturas
        public bool EnviarEmail(string HtmlCode, Correo correo, int Folio)
        {
            string asunto = "Solicitud de Requerimiento con Folio " + Folio;// + " PRUEBAS DESARROLLO";
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Email.User"]);
                mail.To.Add(new MailAddress(correo.Para));
                if (correo.CC != null)
                {
                    string[] Address = correo.CC.Split(char.Parse(";"));
                    foreach (var value in Address)
                    {
                        mail.To.Add(new MailAddress(value));
                    }
                }
                //mail.To.Add(new MailAddress(WebConfigurationManager.AppSettings["Email.Sistemas"].ToString()));
                mail.Subject = asunto;
                mail.IsBodyHtml = true;
                mail.Body = HtmlCode;
                mail.Priority = MailPriority.High;

                using (SmtpClient cliente = new SmtpClient())
                {
                    cliente.Host = WebConfigurationManager.AppSettings["EmailFactura.Server"];
                    cliente.Port = int.Parse(WebConfigurationManager.AppSettings["EmailFactura.Port"]);
                    cliente.EnableSsl = false;
                    cliente.UseDefaultCredentials = true;
                    cliente.Credentials = new System.Net.NetworkCredential(
                        WebConfigurationManager.AppSettings["EmailFactura.User"],
                        WebConfigurationManager.AppSettings["EmailFactura.Password"]
                        );
                    cliente.Send(mail);

                }
                mail.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        #endregion 


        public JsonResult GetEstatus()
        {
            try
            {
                SolicitudManager manager = new SolicitudManager();
                var x = manager.GetEstatus();
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        public JsonResult GetSolicitudReporte(string idArea, DateTime Del, DateTime Al)
        {
            try
            {
                SolicitudManager manager = new SolicitudManager();
                var x = manager.GetSolicitudesReporte(new SolicitudCriteria()
                {
                    Area = int.Parse(idArea),
                    Del = Del,
                    Al = Al
                });
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

    }
}