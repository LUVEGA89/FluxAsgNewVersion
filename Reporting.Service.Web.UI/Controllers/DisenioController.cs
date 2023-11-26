using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using Reporting.Service.Core.Disenio;
using Reporting.Service.Core.Trafico.Contenedor;
using Reporting.Service.Core.Trafico.Contenedor.ContenedorAnexo;
using Reporting.Service.Core.Trafico.Contenedor.ContenedorEnvio;
using Reporting.Service.Core.Trafico.Contenedor.Envio;
using Reporting.Service.Core.Trafico.Contenedor.Naviera;
using Reporting.Service.Core.Trafico.Contenedor.Seguimiento;
using Reporting.Service.Core.Trafico.Contenedor.SeguimientoComentario;
using Reporting.Service.Core.Trafico.Contenedor.StatusContenedor;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class DisenioController : JsonController
    {
        // GET: Reporte
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult Descontinuados()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult SkuDescontinuados()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public JsonResult GetArticulosNuevos(DateTime Del, DateTime Al)
        {
            try
            {
                DisenioManager manager = new DisenioManager();
                var result = manager.GetArticulosNuevos(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetArticulosDescontinuados()
        {
            try
            {
                DisenioManager manager = new DisenioManager();
                var result = manager.GetArticulosDescontinuados();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetArticulosDescontinuadosAltaSap(DateTime Del, DateTime Al)
        {
            try
            {
                DisenioManager manager = new DisenioManager();
                var result = manager.GetArticulosDescontinuadosAltaSap(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}