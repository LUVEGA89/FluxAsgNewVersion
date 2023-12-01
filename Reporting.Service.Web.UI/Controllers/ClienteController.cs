using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.Cliente;
using Reporting.Service.Core.Pais;
using System.Linq;
using System;
using Reporting.Service.Core.Empresa;
using Reporting.Service.Web.UI.Models;
using Reporting.Service.Core.Usuarios;
using Microsoft.Ajax.Utilities;
using Reporting.Service.Web.UI.Controllers;
using Reporting.Service.Web.UI;

namespace Reporting.Service.Web.UI.Controllers
{
    [Authorize]
    public class ClienteController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ClienteController()
        {
        }

        public ClienteController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpGet]
        public JsonResult ObtenerPaises()
        {
            try
            {
                PaisManager manager = new PaisManager();
                var results = manager.FindPagedItems(new PaisCriteria() { });
                results = results.OrderBy(x => x.Nombre).ToArray();
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Insert(Cliente param)
        {
            try
            {
                if (param == null)
                {
                    throw new Exception("Clase cliente null");
                }
                if (param.DatosBancarios.TitularCuenta == null)
                {
                    param.DatosBancarios.TitularCuenta = String.Empty;
                }

                if (param.DatosBancarios.CuentaBanco == null)
                {
                    param.DatosBancarios.CuentaBanco = String.Empty;
                }

                if (param.DatosBancarios.ClabeInterbancaria == null)
                {
                    param.DatosBancarios.ClabeInterbancaria = String.Empty;
                }

                if (param.DatosBancarios.DireccionCuentaBancaria == null)
                {
                    param.DatosBancarios.DireccionCuentaBancaria = String.Empty;
                }

                ClienteManager manager = new ClienteManager();
                var result = manager.Add(param);
                if (result)
                {
                    return this.JsonResponse();
                }
                return this.JsonResponse();
            }
            catch (Exception e)
            {
                return JsonResponse(null, -1, "error" + e.Message);
            }
        }

        [HttpGet]
        public JsonResult ObtenerClientes()
        {
            try
            {
                ClienteManager manager = new ClienteManager();
                var results = manager.FindPagedItems(new ClienteFilter() { });
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult FindClient(string id)
        {
            try
            {
                ClienteManager manager = new ClienteManager();
                var results = manager.Find(int.Parse(id));
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Update(Cliente param)
        {
            try
            {
                ClienteManager manager = new ClienteManager();

                if (param.DatosBancarios.TitularCuenta == null)
                {
                    param.DatosBancarios.TitularCuenta = String.Empty;
                }

                if (param.DatosBancarios.CuentaBanco == null)
                {
                    param.DatosBancarios.CuentaBanco = String.Empty;
                }

                if (param.DatosBancarios.ClabeInterbancaria == null)
                {
                    param.DatosBancarios.ClabeInterbancaria = String.Empty;
                }

                if (param.DatosBancarios.DireccionCuentaBancaria == null)
                {
                    param.DatosBancarios.DireccionCuentaBancaria = String.Empty;
                }

                var result = manager.Update(param);
                if (result)
                {

                }
                return this.JsonResponse("Update completed");
            }
            catch (Exception e)
            {
                return JsonResponse("", -1, "error" + e.Message);
            }
        }
    }
}