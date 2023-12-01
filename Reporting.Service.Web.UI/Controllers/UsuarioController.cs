using Reporting.Service.Core.Empresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Reporting.Service.Web.UI.Models;
using Reporting.Service.Core.Cliente;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Reporting.Service.Core.Roles;
using Reporting.Service.Core.Usuarios;

namespace Reporting.Service.Web.UI.Controllers
{
    public class UsuarioController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UsuarioController()
        {
        }

        public UsuarioController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        public JsonResult GetUsers()
        {
            try
            {
                UsuarioManager manager = new UsuarioManager();
                var result = manager.FindPagedItems(new UsuarioFilter());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetAllUsers()
        {
            try
            {
                UsuarioManager manager = new UsuarioManager();
                var result = manager.GetAllUsers();
                return this.JsonResponse(result, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult FindUser(string id)
        {
            try
            {
                UsuarioManager manager = new UsuarioManager();
                var results = manager.Find(id);
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult GetUser(string id)
        {
            try
            {
                UsuarioManager manager = new UsuarioManager();
                var results = manager.GetUsuarioComplete(id);
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
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

        [HttpGet]
        public JsonResult ObtenerRoles()
        {
            try
            {
                RolesManager manager = new RolesManager();
                var results = manager.RolesAll();
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Insert(Usuario param)
        {
            try
            {
                UsuarioManager manager = new UsuarioManager();
                //var results = manager.InsertUser(param);
                return this.JsonResponse("", 200, "OK");
            }
            catch (Exception e)
            {
                return JsonResponse(null, -1, "error" + e.Message);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Usuario model)
        {
            try
            {
                var roleName = Request.QueryString["roleName"];
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, roleName);
                    if (result1.Succeeded)
                    {
                        //Inserta relación entre Empresa y Usuario
                        UsuarioManager manager = new UsuarioManager();
                        var results = manager.AddUsuarioAndUsuarioEmpresa(user.Id, "", "", "", model.Empresa);

                    }

                }

                return this.JsonResponse(result, 200, "OK");

            }
            catch (Exception e)
            {
                return JsonResponse(null, -1, "error" + e.Message);
            }

        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult Update(Usuario param)
        {
            try
            {
                //Actualiza Rol
                var user = UserManager.FindById(param.Identifier);
                var oldRoleId = user.Roles.SingleOrDefault().RoleId;
                var oldRoleName = UserManager.GetRoles(user.Id).FirstOrDefault();
                if (oldRoleId != param.IdRol)
                {
                    UserManager.RemoveFromRole(user.Id, oldRoleName);
                    UserManager.AddToRole(user.Id, param.Rol);
                }

                UsuarioManager manager = new UsuarioManager();
                var result = manager.UpdateUserComplete(param);

                return this.JsonResponse(result, 200, "OK");
            }
            catch (Exception e)
            {
                return JsonResponse("", -1, "error" + e.Message);
            }
        }

    }
}