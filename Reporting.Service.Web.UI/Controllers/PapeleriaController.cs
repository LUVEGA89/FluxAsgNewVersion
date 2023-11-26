using Reporting.Service.Core.Common;
using Reporting.Service.Core.Papeleria;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class PapeleriaController : Controller
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
        public ActionResult Entrega()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult Reporte()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult CancelarPedidos()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }


        public JsonResult ObtenerInformacionUsuario(string Email)
        {
            try
            {
                CommonManager manager = new CommonManager();
                var result = manager.GetInfoUserByUserName(Email);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }




        public JsonResult BuscarPapeleria(string ItemName, int Tipo)
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                var result = manager.CoreSearchStationery(ItemName, Tipo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult InsertarPedidoPapeleria(string ItemCode, string Comentario, int Cantidad, string Area, int FolioPapeleria)
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                var result = manager.CoreInsertOrderStationery(ItemCode, Comentario, Cantidad, Area, FolioPapeleria);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult InsertarFolio(string Departamento, string UsuarioFolio)
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                var result = manager.CoreInsertFolioPedido(Departamento, UsuarioFolio);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult PedidosDelUsuario(string tipo)
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                var result = manager.CoreOrdersStationery(tipo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult PedidosDelUsuarioByEmail(string UsuarioFolio)
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                var result = manager.CoreOrdersStationeryByUser(UsuarioFolio);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ApruebaOrdenPapeleria(string Sequence, string CantidadAprobada, int Foliox)
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                manager.CoreAprovetOneOrderStationery(Sequence, CantidadAprobada, Foliox);
                return this.JsonResponse();
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ObtenerStockCantidad(string Sequence)
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                var result = manager.ObtenerStockCantidad(Sequence);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult RechazaOrdenPapeleria(string Sequence, int Foliox, string ComentarioSistema)
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                manager.CoreRejectOneOrderStationery(Sequence, Foliox, ComentarioSistema);
                return this.JsonResponse();
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult PedidosDelUsuarioParaSap(string Tipo)
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                manager.CoreInsertOrderStationerySap(Tipo);
                return this.JsonResponse(true);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult comprobarPedidos(string Tipo)
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                var result = manager.comprobarPedidos(Tipo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ReportePapeleriaUno(string FecIni, string FecFin, string Tipo, string centroCosto = "")
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                var result = manager.CoreReportePapeleriaUno(FecIni, FecFin, centroCosto,Tipo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ReportePapeleriaDos(string FecIni, string FecFin, string Tipo)
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                var result = manager.CoreReportePapeleriaDos(FecIni, FecFin, Tipo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ReportePapeleriaTres(string FecIni, string FecFin)
        {
            try
            {
                PapeleriaManager manager = new PapeleriaManager();
                var result = manager.CoreReportePapeleriaTres(FecIni, FecFin);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

    }
}
