using Microsoft.AspNet.Identity;
using Reporting.Service.Core.MetasCobranza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class MetasCobranzaController : JsonController
    {
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public JsonResult GuardarMetas(Meta meta)
        {
            try
            {
                string resultado = string.Empty;
                MetasCobranzaManager manager = new MetasCobranzaManager();
                meta.RegistradoPor = User.Identity.GetUserId();
                resultado = manager.AddMeta(meta);
                return this.JsonResponse(resultado);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ActualizarMetas(Meta meta)
        {
            try
            {
                string resultado = string.Empty;
                MetasCobranzaManager manager = new MetasCobranzaManager();
                meta.ActualizadoPor = User.Identity.GetUserId();
                resultado = manager.UpdateMeta(meta);
                return this.JsonResponse(resultado);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ObtenerMetas(int IdCanal)
        {
            try
            {
                MetasCobranzaManager manager = new MetasCobranzaManager();
                var result = manager.GetMetas(IdCanal);

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
                MetasCobranzaManager manager = new MetasCobranzaManager();
                var result = manager.GetAcumuladoVencidas(Del, Al, Tipo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }


        public JsonResult SincronizarPagos()
        {
            try
            {
                MetasCobranzaManager manager = new MetasCobranzaManager();
                var result = manager.SincronizarPagos();
                return this.JsonResponse(null, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ObtenerDetallesMetas(DateTime Del, DateTime Al, int IdCanal)
        //public JsonResult ObtenerDetallesMetas()
        {
            try
            {
                MetasCobranzaManager manager = new MetasCobranzaManager();
                var result = manager.GetFacturaDetalles(Del, Al, IdCanal);
                //                var result = manager.GetFacturaDetalles();

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}
