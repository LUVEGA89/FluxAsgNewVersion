using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using OfficeOpenXml;
using Reporting.Service.Core.Auditoria;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;

namespace Reporting.Service.Web.UI.Controllers
{
    public class AuditoriaController : Controller
    {
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
        ApplicationDbContext context;
        public AuditoriaController()
        {
            context = new ApplicationDbContext();
        }
        [NonAction]
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
        {
            var jsonResult = this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });

            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        // GET: Auditoria
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpGet]
        public ActionResult Captura(int Tienda, int Tipo)
        {
            CapturaModel model = new CapturaModel();
            model.Tienda = Tienda;
            model.Tipo = Tipo;
            return View(model);
        }
        [HttpGet]
        public ActionResult finalizados(int Tienda, string Del = "", string Al = "", int Tipo = 0)
        {
            AuditoriaManager manager = new AuditoriaManager();

            CapturaModel model = new CapturaModel();
            model.Tienda = Tienda;
            model.Auditorias = manager.GetFinalizados(Tienda, Del, Al, Tipo);
            model.Fechas = model.Auditorias.Select(s => s.RegistradoEl).Distinct().ToList();
            return View(model);
        }

        public JsonResult ObtenerTodos(string Del, string Al)
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.GetAllFinalizados(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public ActionResult Administracion()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }
        public ActionResult Reporte()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public JsonResult AddTipoAuditoria(string Nombre, string Descripcion)
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.AddTipoAuditoria(Nombre, Descripcion, User.Identity.GetUserId());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult EditarTipoAuditoria(int Sequence, string Nombre, string Descripcion)
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.EditTipoAuditoria(Sequence, Nombre, Descripcion);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult EditarSegmento(int Sequence, string Nombre, string Descripcion)
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.EditSegmento(Sequence, Nombre, Descripcion);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult EliminarTipoAuditoria(int Sequence)
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.DeleteTipoAuditoria(Sequence);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult AddSegmento(int Tipo, string Nombre, string Descripcion, int Orden = 1)
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.AddSegmento(Tipo, Nombre, Descripcion, Orden);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult AddRubro(string Segmento, string Nombre, string Descripcion, string Puntuacion, int Requerido, string Nota)
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.AddRubro(Segmento, Nombre, Descripcion, Puntuacion, Requerido, Nota);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult EditRubro(string sequence, string Nombre, string Descripcion, string Puntuacion, int Requerido, string Nota)
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.EditRubro(sequence, Nombre, Descripcion, Puntuacion, Requerido, Nota);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetAuditorias()
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.GetTipoAuditoria();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult RubroCaptura(int Tienda, int Tipo)
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.GetRubroTipoTienda(Tipo, Tienda);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult AgregarComentario(int Tienda, int Tipo, string mensaje, DateTime fecha)
        {
            try
            {
                bool Auditoria = false;
                AuditoriaManager manager = new AuditoriaManager();
                Auditoria = manager.AddComentarioAuditoria(Tipo, Tienda, mensaje, fecha);

                return this.JsonResponse(Auditoria);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult Finalizar(int Tienda, int Tipo)
        {
            try
            {
                int Auditoria = 0;
                AuditoriaManager manager = new AuditoriaManager();
                Auditoria = manager.UpdateAuditoria(Tipo, Tienda);

                if (Auditoria != 0)
                {
                    List<string> Copias = new List<string>();
                    var ConfigAddress = ConfigurationManager.AppSettings["Email.CCO.Auditoria"];
                    String[] Address = ConfigAddress.Split(char.Parse(";"));
                    foreach (var item in Address)
                        Copias.Add(item);

                    var user = User.Identity;
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var Email = UserManager.GetEmail(user.GetUserId());

                    string ruta = Server.MapPath("~/Reports/Auditoria.rpt");

                    var result = manager.GetDetalleAuditoria(Auditoria);
                    var Comentarios = manager.GetComentariosAuditoria(Auditoria);

                    string tienda = result.Rows[0]["Nombre"].ToString();
                    string NombreAuditoria = result.Rows[0]["Tipo"].ToString();
                    string FileName = string.Format("Auditoria [" + NombreAuditoria + "] " + tienda + " #" + Auditoria.ToString() + ".pdf");

                    ReportDocument report = new ReportDocument();
                    report.FileName = Server.MapPath("~/Reports/Auditoria.rpt");
                    report.Load(ruta);
                    report.Database.Tables[0].SetDataSource(result);
                    report.Database.Tables[1].SetDataSource(Comentarios);

                    report.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Documentos/" + FileName + ""));

                    Services.Email.Service EmailSer = new Services.Email.Service();
                    var Enviado = EmailSer.SendEmail(Email, Email, "Se ha Finalizado Auditoria de la Sucursal " + tienda + ".",
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
                                    <td style='text-align: left;'>
                                        <p style='text-align: center;'>
                                            <span style='font-size: 12pt;'>
                                                <strong>
                                                    <span style='color: #ff0000;'>
                                                        En horabuena se ha concluido exitosamente la auditoría.<br />
                                                    </span>
                                                </strong>
                                            </span>
                                        </p>
                                        <p style='text-align: justify;'>
                                            <span style='font-size: 12pt; font-family: helvetica;'>
                                                El detalle de la auditoria podra ser consultado en linea.<br /><br />
                                                Nota: Adjunto podra encontrar el resumen de la auditoría.
                                            </span>
                                        </p>
                                        <br /><br /><br /><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style='font-size: 12pt; font-family: helvetica;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/footer.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>", Copias, FileName, Server.MapPath("~/Documentos/" + FileName + ""));

                }


                return this.JsonResponse("");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetTiendaAuditoria(int Tienda = 0)
        {
            try
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var Email = UserManager.GetEmail(user.GetUserId());

                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.GetTiendasAuditoria(Email, Tienda);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetTiendasSIAT()
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.GetTiendasSIAT(GetCurrentEmailUser());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        private string GetCurrentEmailUser()
        {
            var user = User.Identity;
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var Email = UserManager.GetEmail(user.GetUserId());

            return Email;
        }
        public JsonResult DetalleAuditoria2(int Sequence)
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.GetDetalleAuditoriaView(Sequence);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GuardarRubro(FormCollection collection)
        {
            bool Added = false;
            try
            {
                var Comprobante = "Evidencia-" + Guid.NewGuid().ToString();
                //var files = Request.Files;
                int aux = 1;


                int Tienda = int.Parse(collection["Tienda"].ToString());
                int Tipo = int.Parse(collection["Tipo"].ToString());
                int Segmento = int.Parse(collection["Segmento"].ToString());
                int Rubro = int.Parse(collection["Rubro"].ToString());

                int Aplica = int.Parse(collection["Califica"].ToString());
                string Observaciones = collection["Observacion"].ToString();
                string Latitud = collection["Latitud"].ToString();
                string Longitud = collection["Longitud"].ToString();
                string Exactitud = collection["Exactitud"].ToString();


                AuditoriaManager manager = new AuditoriaManager();
                Added = manager.AddDetalleAuditoria(
                    Tienda,
                    Tipo,
                    Segmento,
                    Rubro,
                    Aplica,
                    Comprobante,
                    Observaciones,
                    User.Identity.GetUserId(),
                    Latitud,
                    Longitud,
                    Exactitud);

                for (int i = 1; i < 10; i++)
                {
                    var file = Request.Files["Evidencia-" + i.ToString()];
                    if (SaveImageServer(file, Comprobante + "-" + (aux).ToString() + ".png", Tienda, Tipo, Segmento, Rubro))
                        aux++;
                    else
                        break;
                }

                return this.JsonResponse(Added);

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public bool SaveImageServer(HttpPostedFileBase file, string Name, int Tienda, int Tipo, int Segmento, int Rubro)
        {

            if (file != null && file.ContentLength > 0)
            {
                var stream = file.InputStream;
                //var fileName = Name;
                //var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Imagenes"), fileName);
                //var fileStream = File(file.InputStream, file.ContentType);
                Image img = System.Drawing.Image.FromStream(stream);
                //img.Save(path, System.Drawing.Imaging.ImageFormat.Png);

                byte[] ImageByte;
                using (var ms = new MemoryStream())
                {
                    EncoderParameters eps = new EncoderParameters(1);
                    eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 50L);
                    string mimetype = GetMimeType(file.ContentType);
                    ImageCodecInfo ici = GetEncoderInfo(mimetype);

                    img.Save(ms, ici, eps);
                    ImageByte = ms.ToArray();
                }

                AuditoriaManager manager = new AuditoriaManager();
                manager.AddImagen(ImageByte, Name, Tienda, Tipo, Segmento, Rubro, Convert.ToBase64String(ImageByte));

                return true;
            }
            return false;

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
        public ActionResult EliminarSegmento(int Sequence)
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                manager.EliminarSegmento(Sequence);

                return this.JsonResponse("El registro fue eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public ActionResult EliminarRubro(int Sequence)
        {
            try
            {
                AuditoriaManager manager = new AuditoriaManager();
                manager.EliminarRubro(Sequence);

                return this.JsonResponse("El registro fue eliminado correctamente.");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public ActionResult DetalleAuditoria(int Sequence)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Auditoria.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Auditoria.xls");
            }

            AuditoriaManager manager = new AuditoriaManager();
            var result = manager.GetDetalleAuditoria(Sequence);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Resultado auditoria";
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
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Auditoria.xls" }
            };

        }
        public ActionResult DetalleAuditoriasFechas(DateTime Del, DateTime Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Reportes-Auditorias.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Reportes-Auditorias.xls");
            }

            AuditoriaManager manager = new AuditoriaManager();
            var result = manager.GetAuditoriasFinalizadasReporte(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Resultado auditorias";
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
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Reportes-Auditorias.xls" }
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
                return new EmptyResult();
            }
        }

        [HttpPost]
        public JsonResult EnviaAuditoria(int Sequence)
        {
            try
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var Email = UserManager.GetEmail(user.GetUserId());
                List<string> Copias = new List<string>();
                var ConfigAddress = ConfigurationManager.AppSettings["Email.CCO.Auditoria"];
                String[] Address = ConfigAddress.Split(char.Parse(";"));

                foreach (var item in Address)
                    Copias.Add(item);

                string ruta = Server.MapPath("~/Reports/Auditoria.rpt");
                AuditoriaManager manager = new AuditoriaManager();
                var result = manager.GetDetalleAuditoria(Sequence);
                var Comentarios = manager.GetComentariosAuditoria(Sequence);
                string tienda = result.Rows[0]["Nombre"].ToString();
                string Auditoria = result.Rows[0]["Tipo"].ToString();
                string FileName = string.Format("Auditoria [" + Auditoria + "] " + tienda + " #" + Sequence.ToString() + ".pdf");

                ReportDocument report = new ReportDocument();
                report.FileName = Server.MapPath("~/Reports/Auditoria.rpt");
                report.Load(ruta);
                report.Database.Tables[0].SetDataSource(result);
                report.Database.Tables[1].SetDataSource(Comentarios);

                report.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Documentos/" + FileName + ""));



                Services.Email.Service EmailSer = new Services.Email.Service();
                var Enviado = EmailSer.SendEmail(Email, Email, "Se ha Finalizado Auditoria de la Sucursal " + tienda + ".", @"<table width='608'>
                            <tbody>
                                <tr>
                                    <td>
                                        <span style='font-size: 12pt;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/encabezado.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='text-align: left;'>
                                        <p style='text-align: center;'>
                                            <span style='font-size: 12pt;'>
                                                <strong>
                                                    <span style='color: #ff0000;'>
                                                        Ha descargado con exito el pdf de la auditoria.<br />
                                                    </span>
                                                </strong>
                                            </span>
                                        </p>
                                        <p style='text-align: justify;'>
                                            <span style='font-size: 12pt; font-family: helvetica;'>
                                                El detalle de la auditoria tambien podra ser consultado en linea.<br /><br />
                                                Nota: Adjunto podra encontrar el resumen de la auditoría.
                                            </span>
                                        </p>
                                        <br /><br /><br /><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <span style='font-size: 12pt; font-family: helvetica;'>
                                            <img style='display: block; margin-left: auto; margin-right: auto;' src='http://www.fussionweb.com/Recursos/footer.png' alt='' width='600' height='117' />
                                        </span>
                                    </td>
                                </tr>
                            </tbody>
                        </table>", Copias, FileName, Server.MapPath("~/Documentos/" + FileName + ""));

                if (Enviado)
                    return this.JsonResponse("El correo se envió correctamente");
                else
                    return this.JsonResponse(null, -1, "Ocurrio un problema al enviar el correo");

            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}