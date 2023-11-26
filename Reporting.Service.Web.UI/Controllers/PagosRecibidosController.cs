using Reporting.Service.Core.PagosRecibidos;
using System;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class PagosRecibidosController : JsonController
    {
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public JsonResult DetallePagos(DateTime Del, DateTime Al, int Tipo)
        {
            try
            {
                PagosManager manager = new PagosManager();
                var result = manager.GetDetalleVentas(Del, Al, Tipo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult AcumuladoVencidas(DateTime Del, DateTime Al, int Tipo)
        {
            try
            {
                PagosManager manager = new PagosManager();
                decimal result = manager.GetAcumuladoVencidas(Del, Al, Tipo);
                return this.JsonResponse(result, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}
