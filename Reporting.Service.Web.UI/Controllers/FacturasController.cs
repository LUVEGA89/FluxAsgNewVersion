using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Reporting.Service.Core.FacturasManager;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class FacturasController : JsonController
    {
        // GET: Facturas
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        //Manejo de busqueda de facturas por fechas y codigo de clientes
        public JsonResult BuscarFacturasXCliente(string CodeCliente, DateTime Del, DateTime Al)
        {
            try
            {
                IList<Reporting.Service.Core.FacturasManager.FacturaManager> result = null;
                FacturaCatalog manager = new FacturaCatalog();
                result = manager.FindFacturasXCliente(CodeCliente, Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + "Trace: " + ex.StackTrace);
            }
        }
        //Manejo de busqueda de facturas por fechas y codigo de clientes
        public JsonResult BuscarFacturas(DateTime Del, DateTime Al)
        {
            try
            {
                IList<Reporting.Service.Core.FacturasManager.FacturaManager> result = null;
                FacturaCatalog manager = new FacturaCatalog();
                result = manager.FindFacturas(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + "Trace: " + ex.StackTrace);
            }
        }
        //Manejo de busqueda de facturas por fechas y codigo de clientes
        public JsonResult BuscarFacturasXNumero(string DocNum)
        {
            try
            {
                IList<Reporting.Service.Core.FacturasManager.FacturaManager> result = null;
                FacturaCatalog manager = new FacturaCatalog();
                result = manager.FindFacturasXNum(DocNum);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + "Trace: " + ex.StackTrace);
            }
        }
        //Busqueda de facturas XML y guardado en Memory
        public ActionResult DescargarFacturas(List<FacturaManager> Facturas)
        {
            try
            {
                var di = @"" + WebConfigurationManager.AppSettings["Directorio.Facturas"];
                DirectoryInfo directory = new DirectoryInfo(di);
                List<FileInfo> FacturaPDF = new List<FileInfo>();
                List<FileInfo> FacturaXML = new List<FileInfo>();
                int countFacturas = Facturas.Count;
                for (int i = 0; i < countFacturas; i++)
                {
                    FacturaPDF.Add(directory.GetFiles("I " + Facturas[i].DocNum + ".pdf")[0]);
                    FacturaXML.Add(directory.GetFiles("I " + Facturas[i].DocNum + ".xml")[0]);
                    if (FacturaPDF[i].ToString() == "")
                    {
                        return JsonResponse("Espere 10 minutos y vuelva a intentar");
                    }
                }
                int countFacturasFind = FacturaPDF.Count;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        for (int i = 0; i < countFacturasFind; i++)
                        {
                            ziparchive.CreateEntryFromFile(FacturaPDF[i].FullName, FacturaPDF[i].Name);
                            ziparchive.CreateEntryFromFile(FacturaXML[i].FullName, FacturaXML[i].Name);
                        }
                    }
                    string handle = Guid.NewGuid().ToString();
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    TempData[handle] = memoryStream.ToArray();
                    return new JsonResult()
                    {
                        Data = new { FileGuid = handle, FileName = "Facturas"+DateTime.Today.ToString()+".zip" }
                    };
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + "Trace: " + ex.StackTrace);
            }
        }
        [HttpGet]//Descarga de lo guardado en la memoria temporal
        public ActionResult DownloadFactura(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/zip", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }
        //Envia correo electrónico junto con las facturas
        public JsonResult EnviarEmail(List<FacturaManager> Facturas, List<Reporting.Service.Core.FacturasManager.Correo> correo)
        {
            string asunto = "Envio de " + Facturas.Count + " facturas";
            string html_code = "<span>"+ correo[0].Comentario +"</span>";
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["EmailFactura.User"]);
                mail.To.Add(new MailAddress(correo[0].Para));
                if (correo[0].CC != null)
                {
                    string[] Address = correo[0].CC.Split(char.Parse(";"));
                    foreach (var value in Address)
                    {
                        mail.To.Add(new MailAddress(value));
                    }
                }
                mail.To.Add(new MailAddress(WebConfigurationManager.AppSettings["Email.Sistemas"].ToString()));
                mail.Subject = asunto;
                mail.IsBodyHtml = true;
                mail.Body = html_code;
                mail.Priority = MailPriority.High;
                //********** Trae la factura pdf **********
                var di = @"" + WebConfigurationManager.AppSettings["Directorio.Facturas"];

                String[] rutaFile;
                String[] rutaFileXML;
                foreach (var item in Facturas)
                {
                    rutaFile = Directory.GetFiles(di, "I " + item.DocNum + ".pdf");
                    rutaFileXML = Directory.GetFiles(di, "I " + item.DocNum + ".xml");
                    if (rutaFile[0].ToString() == "" || rutaFileXML[0].ToString()=="")
                    {
                        return JsonResponse("Espere 10 minutos y vuelva a intentar");
                    }
                    else
                    {
                        mail.Attachments.Add(new Attachment(rutaFile[0].ToString()));
                        mail.Attachments.Add(new Attachment(rutaFileXML[0].ToString()));
                    }
                }
                SmtpClient cliente = new SmtpClient();
                cliente.Host = WebConfigurationManager.AppSettings["EmailFactura.Server"];
                cliente.Port = int.Parse(WebConfigurationManager.AppSettings["EmailFactura.Port"]);
                cliente.EnableSsl = false;
                cliente.UseDefaultCredentials = true;
                cliente.Credentials = new System.Net.NetworkCredential(
                    WebConfigurationManager.AppSettings["EmailFactura.User"],
                    WebConfigurationManager.AppSettings["EmailFactura.Password"]
                    );
                cliente.Send(mail);
                mail.Dispose();
                cliente.Dispose();

                return JsonResponse("Enviado");
            }
            catch (Exception ex)
            {
                return JsonResponse(ex.Message + " Trace: "+ ex.StackTrace);
            }
        }
        //busca si existe la factura, sino la crea
        public Stream Factura(int Key, int Id)
        {
            var di = @"" + WebConfigurationManager.AppSettings["Directorio.Facturas"];
            DirectoryInfo directory = new DirectoryInfo(di);
            FileInfo[] FacturaPDF = directory.GetFiles("" + Id + ".pdf");
            if (FacturaPDF.Length == 0) 
            {
                ReportDocument CRFactura = new ReportDocument(); 
                string path = @"" + WebConfigurationManager.AppSettings["Directorio.Facturas.rpt"];
                CRFactura.Load(path, OpenReportMethod.OpenReportByTempCopy);
                CRFactura.SetDatabaseLogon("sa", "Passw0rd", "192.168.2.143", "Massriv2007");
                CRFactura.SetParameterValue("DocKey@", Key);
                CRFactura.ExportToDisk(ExportFormatType.PortableDocFormat, @"" + WebConfigurationManager.AppSettings["Directorio.Facturas"] + Id + ".pdf");
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = CRFactura.ExportToStream(ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return stream;
            }
            else
            {
                Stream stream = FacturaPDF[0].OpenRead();
                return stream;
            }
        }

    }
}