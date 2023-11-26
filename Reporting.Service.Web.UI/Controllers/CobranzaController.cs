using Reporting.Service.Web.UI.Models;
using Reporting.Service.Core.CreditoCobranza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class CobranzaController : Controller
    {
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
        public JsonResult ConsultaCobranza()
        {
            try
            {
                CobranzaManager manager = new CobranzaManager();
                manager.AddConsulta();
                return this.JsonResponse();
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult UltimosRegistros()
        {
            try
            {
                CobranzaManager manager = new CobranzaManager();
               var result = manager.GetUltimasConsultas();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult DetalleCobranza()
        {
            try
            {
                CobranzaManager manager = new CobranzaManager();
                var result = manager.GetDetalleCobranza();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult DetalleCobranzaDesglose()
        {
            try
            {
                CobranzaManager manager = new CobranzaManager();
                CobranzaModel model = new CobranzaModel();
                model.DetalleCondicionesPago = manager.GetCobranzaCondicionesPago();
                model.Contado = manager.GetCobranzaCanalesContado();
                model.Credito = manager.GetCobranzaCanalesCredito();
                model.PorCobrar = manager.GetCobranzaCanalesCuentasPorCobrar();
                model.Historial = manager.GetHistorialCartera();
                model.TopClientes = manager.GetTopClientes();
                return this.JsonResponse(model);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ConsultaEliminar(int Sequence)
        {
            try
            {
                CobranzaManager manager = new CobranzaManager();
                manager.EliminarRegitrosCobranza(Sequence);
                return this.JsonResponse();
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //
        // GET: /Cobranza/

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

    }
}
