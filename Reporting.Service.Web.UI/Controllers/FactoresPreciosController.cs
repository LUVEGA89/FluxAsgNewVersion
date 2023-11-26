using Reporting.Service.Core.FactoresPrecios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class FactoresPreciosController : JsonController
    {
        // GET: FactoresPrecios
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }
        public JsonResult GetTipoPrecioArticulos()
        {
            try
            {
                TipoPrecioArticuloManager manager = new TipoPrecioArticuloManager();
                var result = manager.FindPagedItems(new TipoPrecioCanalCriteria());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult AddTipoPrecio(string tipo)
        {
            try
            {
                TipoPrecioArticuloManager manager = new TipoPrecioArticuloManager();
                TipoPrecioArticulo item = new TipoPrecioArticulo();
                item.Descripcion = tipo;
                    
                var result = manager.Add(item);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult GetTipoPrecioCanales()
        {
            try
            {
                TipoPrecioCanalManager manager = new TipoPrecioCanalManager();
                var result = manager.FindPagedItems(new TipoPrecioFactorCirteria());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult GetCanalesDisponibles(int IDTipoPrecioArt)
        {
            try
            {
                TipoPrecioCanalManager manager = new TipoPrecioCanalManager();
                var result = manager.FindDisponible(IDTipoPrecioArt);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult AddTipoCanal(string IDTipoPrecioCanal,string Descripcion)
        {
            try
            {
                TipoPrecioCanalManager manager = new TipoPrecioCanalManager();
                TipoPrecioCanal item = new TipoPrecioCanal();
                item.Identifier = IDTipoPrecioCanal;
                item.Descripcion = Descripcion;
                var result = manager.Add(item);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult GetFactores()
        {
            try
            {
                TipoPrecioFactorManager manager = new TipoPrecioFactorManager();
                var result = manager.FindPagedItems(new TipoPrecioFactorCriteria());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult UpdateFactor(int IdFactor,decimal factor)
        {
            try
            {
                TipoPrecioFactorManager manager = new TipoPrecioFactorManager();
                TipoPrecioFactor item = new TipoPrecioFactor();
                item.Identifier = IdFactor;
                item.Factor = factor;

                var result = manager.Update(item);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult FindDuplicates(int IDTipoPrecioArt, string IDTipoPrecioCanal)
        {
            try
            {
                TipoPrecioFactorManager manager = new TipoPrecioFactorManager();
                var result = manager.FindPagedItems(new TipoPrecioFactorCriteria { IdTipoPrecioArt = IDTipoPrecioArt, IdTipoPrecioCanal = IDTipoPrecioCanal });            
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
        public JsonResult AddFactor(int IDTipoPrecioArt,string IDTipoPrecioCanal ,decimal factor)
        {
            try
            {
                TipoPrecioFactorManager manager = new TipoPrecioFactorManager();
                TipoPrecioFactor item = new TipoPrecioFactor();
                item.TipoPrecioArticulo = new TipoPrecioArticulo();
                item.TipoPrecioCanal = new TipoPrecioCanal();
                item.TipoPrecioArticulo.Identifier = IDTipoPrecioArt;
                item.TipoPrecioCanal.Identifier = IDTipoPrecioCanal;
                item.Factor = factor;
                var result = manager.Add(item);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
    }
}