using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Reporting.Service.Core.Buzon.Area;
using Reporting.Service.Core.Buzon.Categoria;
using Reporting.Service.Core.Buzon;
using WikiCore;
using WikiCore.Data;
using Sive.Core;
using System.Net.Mail;
using System.Web.Configuration;

namespace Reporting.Service.Web.UI.Controllers
{
    public class BuzonController : JsonController
    {

        public BuzonController()
        {

        }

        // GET: Buzon
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reporte()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }

        public JsonResult GetCategoria()
        {
            try
            {
                CategoriaManager manager = new CategoriaManager();
                var x = manager.FindPagedItems(new CategoriaCriteria());
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetAreas()
        {
            try
            {
                AreaManager manager = new AreaManager();
                var x = manager.FindPagedItems(new AreaCriteria());
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetSucursales()
        {
            try
            {
                StoreCatalog manager = new StoreCatalog();
                var x = manager.FindPagedItems(new StoreCriteria());                
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult AddBuzon(Buzon item)
        {
            try
            {
                if (item.Categoria.Identifier == 0)
                {
                    item.Categoria = null;
                }
                if (item.Area.Identifier == 0)
                {
                    item.Area = null;
                }
                if (string.IsNullOrWhiteSpace(item.Nombre))
                {
                    item.Nombre = ("Anonimo");
                }
                BuzonManager manager = new BuzonManager();
                var x = manager.Add(item);
                SendCorreoBuzon(item);
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        private void SendCorreoBuzon(Buzon item)
        {
            string buzon = string.Empty;
            switch (item.Tipo)
            {
                case BuzonKind.Queja:
                    buzon = "Queja";
                    break;
                case BuzonKind.Sugerencia:
                    buzon = "Sugerencia";
                    break;
            }
            CategoriaManager manager = new CategoriaManager();
            var x = manager.Find(item.Categoria.Identifier);

            AreaManager areaManager = new AreaManager();
            var area = areaManager.Find(item.Area.Identifier);

            if (string.IsNullOrWhiteSpace(item.Nombre))
            {
                item.Nombre = "Anonimo";
            }
            if (string.IsNullOrWhiteSpace(item.Sucursal))
            {
                item.Sucursal = "No definido";
            }
            string html_code = string.Empty;
            string asunto = "Buzon de Sugerencias";
            html_code += "<div>";
            html_code += "<h4 style='font-family: arial, sans-serif;'>Comunicado.</h4>";            
            html_code += "<h5 style='font-family: arial, sans-serif;'>Se ha generado una nueva buzón de " + buzon + " </h5>";
            html_code += "<h5 style='font-family: arial, sans-serif;'>Creado por: " + item.Nombre + "</h5>";
            html_code += "<h5 style='font-family: arial, sans-serif;'>Categoria: " + x.Nombre + " </h5>";
            html_code += "<h5 style='font-family: arial, sans-serif;'>Area: " + area.Nombre + "</h5>";
            html_code += "<h5 style='font-family: arial, sans-serif;'>Comentario: " + item.Sugerencia + "</h5>";
            html_code += "<h5 style='font-family: arial, sans-serif;'>Sucursal: " + item.Sucursal + "</h5>";
            html_code += "<h5 style='font-family: arial, sans-serif;'>Regitrado el: " + DateTime.Now + "</h5>";
            html_code += "<br/>";
            html_code += "</div>";
            try
            {
                MailMessage correo = new MailMessage();
                correo.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Email.User"]);
                var ConfigAddress2 = System.Configuration.ConfigurationManager.AppSettings["Email.Buzon.RH"];
                string[] Address2 = ConfigAddress2.Split(char.Parse(";"));                
                foreach (var value in Address2)
                {
                    correo.To.Add(new MailAddress(value));
                }                
                if(correo.To.Count == 0)
                {
                    return;
                }
                correo.Subject = asunto;
                correo.IsBodyHtml = true;
                correo.Body = html_code;
                correo.Priority = MailPriority.High;
                SmtpClient cliente = new SmtpClient();
                cliente.Host = WebConfigurationManager.AppSettings["Email.Server"].ToString();
                cliente.Port = int.Parse(WebConfigurationManager.AppSettings["Email.Port"]);
                cliente.EnableSsl = false;
                cliente.UseDefaultCredentials = true;
                cliente.Credentials = new System.Net.NetworkCredential("911@fussionweb.com", ")helpfussion1");
                cliente.Send(correo);
                correo.Dispose();
                cliente.Dispose();
            }
            catch (Exception ex)
            {
                return;
            }
        }


        public JsonResult GetReporteBuzon(BuzonCriteria criteria)
        {
            try
            {
                BuzonManager manager = new BuzonManager();
                var x = manager.FindPagedItems(criteria);
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}