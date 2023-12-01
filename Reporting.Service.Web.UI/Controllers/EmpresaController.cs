using Reporting.Service.Core.Empresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Reporting.Service.Web.UI.Models;
using Reporting.Service.Core.Cliente;

namespace Reporting.Service.Web.UI.Controllers
{
    public class EmpresaController : BaseController
    {
        // GET: Empresa
        public ActionResult Index()
        {
            return View();
        }

        //        
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(EmpresaRegisterViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Empresa item = new Empresa();
                    item.Nombre = model.Nombre;
                    item.RazonSocial = model.RazonSocial;
                    item.Entrega = model.Entrega;
                    EmpresaManager manager = new EmpresaManager();
                    var result = manager.Add(item);
                    if (result)
                    {
                        return RedirectToAction("Index", "Empresa");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Empresa");
            }
            return View(model);            
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Insert(Empresa param)
        {
            try
            {
                if (param == null)
                {
                    throw new Exception("Clase Empresa null");
                }
                EmpresaManager manager = new EmpresaManager();
                var result = manager.Add(param);
                if (result)
                {

                }
                return this.JsonResponse();
            }
            catch (Exception e)
            {
                return JsonResponse(null, -1, "error" + e.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Update(Empresa param)
        {
            try
            {
                EmpresaManager manager = new EmpresaManager();               
                var result = manager.Update(param);
                if (result)
                {

                }
                return this.JsonResponse("Save completed");
            }
            catch (Exception e)
            {
                return JsonResponse("",-1, "error" + e.Message);
            }
        }



        [HttpGet]
        public JsonResult Edit(int id)
        {
            EmpresaUpdateViewModel item = new EmpresaUpdateViewModel();
            EmpresaManager manager = new EmpresaManager();
            var result = manager.Find(id);
            if (result != null)
            {
                item.Identifier = result.Identifier;
                item.Nombre = result.Nombre;
                item.RazonSocial = result.RazonSocial;
                item.Entrega = result.Entrega;
            }


            return this.JsonResponse(item);
        }

        [HttpGet]
        public JsonResult Eliminar(int id)
        {
            try
            {
                EmpresaManager manager = new EmpresaManager();
                var result = manager.Delete(id);
                return this.JsonResponse();
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null,-1,ex.Message);
            }
        }

        [HttpGet]
        public JsonResult ObtenerEmpresas()
        {
            try
            {
                EmpresaManager manager = new EmpresaManager();
                var results = manager.EmpresasAll();
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }



    }
}