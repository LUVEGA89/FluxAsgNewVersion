using Reporting.Service.Core.Garantias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class GarantiasController : JsonController
    {
        // GET: Garantias
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
            {
                throw new Exception("No estas autenticado!!!");
            }

            return View();
        }

        //prGetDetalleProducto_llegadas
        public ActionResult Reporte()
        {
            if (!Request.IsAuthenticated)
            {
                throw new Exception("No estas autenticado!!!");
            }

            return View();
        }


        public JsonResult ReporteGarantiasSku()
        {
            if (!Request.IsAuthenticated)
            {
                throw new Exception("No estas autenticado!!!");
            }

            try
            {
                GarantiaManager manager = new GarantiaManager();
                var results = manager.GetReporte();
                return this.JsonResponse(results);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}