
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Reporting.Service.Core.Inventarios;

namespace Reporting.Service.Web.UI.Controllers
{
    public class InventariosController : Controller
    {
        [NonAction]
        public JsonResult JsonResponse(object context = null, int code = 200, string message = "Success")
        {
            return this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });
        }
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        public JsonResult ObtenerFacturas(string FecIni,string FecFin)
        {
            try
            {
                FacturasManager manager = new FacturasManager();

                var result = manager.CoreSearch(FecIni,FecFin);

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

    }
}
