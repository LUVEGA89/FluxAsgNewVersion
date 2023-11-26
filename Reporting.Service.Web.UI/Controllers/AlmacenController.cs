using Reporting.Service.Core.Almacen.StatusPedidos;
using Reporting.Service.Core.Tarimas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class AlmacenController : JsonController
    {
        // GET: Tarimas
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult Reporte()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Buscar(StatusPedidosCriteria criteria)
        {
            try
            {
                StatusPedidosManager manager = new StatusPedidosManager();
                
                var result = manager.GetReporte(criteria);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

    }
}