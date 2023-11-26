using Reporting.Service.Core.Tarimas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class TarimasController : JsonController
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
        public JsonResult Buscar(TarimaCriteria criteria)
        {
            try
            {
                TarimasCatalog catalog = new TarimasCatalog();
                ResumenTarimas resumenTarimas = new ResumenTarimas();
                resumenTarimas = catalog.GetTotales(criteria);

                resumenTarimas.Tarimas = catalog.FindPagedItems(criteria);

                //Obtenemos la suma del DocTotal
                //resumenTarimas.MontoTotalPedidos = resumenTarimas.Tarimas.Sum(item => item.DocTotal);

                return this.JsonResponse(resumenTarimas);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult SincronizarSIE()
        {
            try
            {
                TarimasCatalog catalog = new TarimasCatalog();
                int result = catalog.SyncSIE();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult SincronizarSAP(TarimaCriteria criteria)
        {
            try
            {
                if (criteria.Completados != null)
                    criteria.Completados = criteria.Completados.TrimEnd(',');
                if (criteria.Cancelados != null)
                    criteria.Cancelados = criteria.Cancelados.TrimEnd(',');

                TarimasCatalog catalog = new TarimasCatalog();
                string result = catalog.SyncSAP(criteria);
                if (result == "ok")
                {
                    return this.JsonResponse("Los datos se procesaron correctamente.", 200, "OK");
                }
                else
                {
                    return this.JsonResponse(result, -1, result);
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}