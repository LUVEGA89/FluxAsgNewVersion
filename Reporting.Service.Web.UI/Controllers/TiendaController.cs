using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OfficeOpenXml;
using Reporting.Service.Core.Common;
using Reporting.Service.Core.Tiendas;
using Reporting.Service.Core.Autorización;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class TiendaController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext context;

        private TiendasManager manager = new TiendasManager();

        public TiendaController()
        {
            context = new ApplicationDbContext();
        }
        [NonAction]
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
        {
            return this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });
        }
        // GET: Tienda
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Boletin()
        {
            return View();
        }
        public ActionResult PedidoTienda()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult Autorizacion()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult Seguimiento()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public JsonResult DetalleTiendas()
        {
            try
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var Email = UserManager.GetEmail(user.GetUserId());

                TiendasManager manager = new TiendasManager();
                var result = manager.GetTiendasRegional(Email);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult Tiendas()
        {
            try
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var Email = UserManager.GetEmail(user.GetUserId());

                TiendasManager manager = new TiendasManager();
                var result = manager.GetTiendas(Email);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult SuscritosBoletin()
        {
            try
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var Email = UserManager.GetEmail(user.GetUserId());

                TiendasManager manager = new TiendasManager();
                var result = manager.GetBoletin(Email);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public ActionResult ExcelBoletin()
        {
            var user = User.Identity;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var Email = UserManager.GetEmail(user.GetUserId());

            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            FileInfo newFile = new FileInfo(path + @"Boletin.xls");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(path + @"\Boletin.xls");
            }
            TiendasManager manager = new TiendasManager();
            var result = manager.GetBoletinExcel(Email);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Boletin";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Boletin.xls" }
            };

        }
        [HttpGet]
        public ActionResult Download(string fileGuid, string fileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }
        // Agregado Armando Pedidos Tiendas
        public JsonResult ListaTiendas()
        {
            try
            {
                TiendasManager manager = new TiendasManager();
                var result = manager.GetListaTiendas();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        // 
        public JsonResult AgregarArticulosPedidos(string CardCode, string SKU, int Cantidad)
        {
            try
            {
                TiendasManager manager = new TiendasManager();
                var result = manager.CoreInsertPedidosTiendas(CardCode, SKU, Cantidad);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult BorrarArticulosPedidos(string CardCode)
        {
            try
            {
                TiendasManager manager = new TiendasManager();
                var result = manager.CoreBorrarPedidosTiendas(CardCode);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult ejecutaSap(string CardCode, string Referencia, string Comentario, int Auth,int Tipo=0)
        {
            if (!Request.IsAuthenticated)
                RedirectToAction("Login", "Account");
            try
            {
                TiendasManager manager = new TiendasManager();
                string folio = manager.CoreInsertarPedido(CardCode, Referencia, Comentario,User.Identity.GetUserName(), Auth);
                if(folio!="X" && !folio.Contains("Error"))//Proceso autorización
                {
                    CardCode = obtenerNombreTienda(CardCode);
                    if (Tipo == 1)//Autorización
                    {
                        enviarCorreo("Pedido Autorizado", "El pedido de la tienda " + CardCode + " ha sido autorizado\nFolios de SAP: " + folio);
                    }
                    else//Pedido
                    {
                        enviarCorreo("Pedido Realizado", "El pedido de la tienda " + CardCode + " ha sido realizado\nFolios de SAP: " + folio);
                    }
                    
                }
                return this.JsonResponse(folio);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public string obtenerNombreTienda(string cardcode)
        {
            string nombre = "";

            TiendasManager manager = new TiendasManager();
            nombre = manager.obtenerNombreTienda(cardcode);

            return nombre;
        }

        public bool enviarCorreo(String Subject, String Body)
        {
            //Enviar correo con los folios
            MailMessage correo = new MailMessage();
            //correo.To.Add(new MailAddress("moises_rodriguez@fussionweb.com"));
            //correo.To.Add(new MailAddress("jose_rodriguez@fussionweb.com"));
            correo.To.Add(new MailAddress("cesar_cruz@fussionweb.com"));
            correo.Bcc.Add(new MailAddress("francisco_martinez@fussionweb.com"));
            var from = @System.Configuration.ConfigurationManager.AppSettings["EmailSistemasFussion"];
            correo.From = new MailAddress(from);
            correo.Subject = Subject;
            correo.Body = Body;
            correo.Priority = MailPriority.High;
            SmtpClient cliente = new SmtpClient();
            cliente.Host = "mail.fussionweb.com";
            cliente.Port = 587;
            cliente.EnableSsl = false;
            cliente.UseDefaultCredentials = true;
            cliente.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");

            cliente.Send(correo);
            return true;
        }

        public JsonResult validarPresupuesto(String CardCode)
        {
            try
            {
                TiendasManager manager = new TiendasManager();
                var respuesta = manager.validarPresupuesto(CardCode);
                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult getPedidosTiendasNoAuth(string CardCode)
        {
            try
            {
                TiendasManager manager = new TiendasManager();
                var respuesta = manager.getPedidosTiendasNoAuth(CardCode);
                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult findPedidosTiendas(string CardCode, int Auth)
        {
            try
            {
                TiendasManager manager = new TiendasManager();
                var respuesta = manager.findPedidosTiendas(CardCode, Auth);
                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult SendAuthorization(string ToEmail, string FromEmail, string SubjectEmail, string BodyEmail, int RegistrosID, string Cliente, decimal precioTotal, string Referencia, string Comentarios)
        {

            MailMessage correo = new MailMessage();
            correo.To.Add(new MailAddress(ToEmail));
            correo.From = new MailAddress(FromEmail);
            correo.Subject = SubjectEmail;
            correo.IsBodyHtml = true;
            correo.Body = "<html><body>" + BodyEmail + "</body></html>";
            correo.Priority = MailPriority.High;
            SmtpClient cliente = new SmtpClient();
            cliente.Host = "mail.fussionweb.com";
            cliente.Port = 587;
            cliente.EnableSsl = false;
            cliente.UseDefaultCredentials = true;
            cliente.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");

            cliente.Send(correo);
            try
            {
                AutorizacionManager manager = new AutorizacionManager();
                Autorizacion nuevo = new Autorizacion();
                nuevo.Cliente = Cliente;
                nuevo.Importe = precioTotal;
                nuevo.referencia = Referencia;
                nuevo.comentarios = Comentarios;
                nuevo.status = 0;
                nuevo.PedidosTiendasWEB = RegistrosID;
                var result = manager.Add(nuevo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetAuthorizations()
        {
            try
            {
                AutorizacionManager manager = new AutorizacionManager();

                var respuesta = manager.FindPagedItems(new AutorizacionCriteria());
                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult findAuthorization(int id)
        {
            try
            {
                AutorizacionManager manager = new AutorizacionManager();
                var respuesta = manager.Find(id);
                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult UpdateAuthorization(int id, string Cliente, decimal Importe, string fecha, string referencia, string comentarios, int status, int registros)
        {
            try
            {
                AutorizacionManager manager = new AutorizacionManager();
                Autorizacion buscado = new Autorizacion();
                buscado.Identifier = id;
                buscado.Cliente = Cliente;
                buscado.Importe = Importe;
                buscado.fecha = fecha;
                buscado.referencia = referencia;
                buscado.comentarios = comentarios;
                buscado.status = status;
                buscado.PedidosTiendasWEB = registros;
                var respuesta = manager.Update(buscado);
                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult PresupuestoDisponible(string CardCode)
        {
            try
            {
                TiendasManager manager = new TiendasManager();
                var respuesta = manager.PresupuestoDisponible(CardCode);
                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        //Total pedido
        public JsonResult TotalPedido(string CardCode)
        {
            try
            {
                TiendasManager manager = new TiendasManager();
                var respuesta = manager.TotalPedido(CardCode);
                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult obtenerTienda()
        {
            try
            {
                TiendasManager manager = new TiendasManager();
                var respuesta = manager.obtenerTienda(User.Identity.GetUserName());
                return this.JsonResponse(respuesta);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        
        public JsonResult ObtenerPedidos(DateTime Inicio, DateTime Fin, int Folio, string Cliente)
        {
            try
            {
                TiendasManager manager = new TiendasManager();
                var result = manager.ObtenerPedidos(Inicio, Fin, Folio, Cliente, User.Identity.GetUserName());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult ComprobarArchivo(int Factura, string Fecha)
        {
            string[] fe = Fecha.Split('/');
            int mes = int.Parse(fe[0]);
            int dia = int.Parse(fe[1]);
            int anio = int.Parse(fe[2]);

            try
            {
                string _base = "\\\\192.168.2.194\\QR\\MASSRIV\\";
                if (new DateTime(anio, mes, dia) > new DateTime(2023, 03, 31))
                {
                    _base = _base + "I ";
                }
                
                string resp = "";
                string fullpath = _base + Factura + ".PDF";
                fullpath = Path.GetFullPath(fullpath);
                if (System.IO.File.Exists(fullpath))
                    resp = "Descargar";
                else
                {
                    fullpath = _base + Factura + ".XML";
                    if (System.IO.File.Exists(fullpath))
                        resp = "Proceso";
                    else
                        resp = "Error";
                }
                return this.JsonResponse(resp);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, $"parametros mes: {mes}, dia: {dia}, año: {anio}, detalle: {ex.Message}");
            }
        }

        public FileResult Descargar(int Factura, string Fecha)
        {
            string[] fe = Fecha.Split('/');
            int mes = int.Parse(fe[0]);
            int dia = int.Parse(fe[1]);
            int anio = int.Parse(fe[2]);
            
            string _base = "\\\\192.168.2.194\\QR\\MASSRIV\\";
            if (new DateTime(anio, mes, dia) > new DateTime(2023, 03, 31))
            {
                _base = _base + "I ";
            }
            
            var fullpath = _base + Factura + ".PDF";
            fullpath = Path.GetFullPath(fullpath);
            var respuesta = File(fullpath, "application/force-download", Path.GetFileName(fullpath));
            return respuesta;
        }

        public JsonResult DetallesFactura(int ruta, int Factura)
        {
            try
            {
                var response = manager.GetDetailsInvoice(ruta, Factura);

                return this.JsonResponse(response);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}
