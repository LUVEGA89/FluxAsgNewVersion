using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Reporting.Service.Core.Roles;
using Reporting.Service.Web.UI.Controllers;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class RolesController : BaseController
    {
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Account/Register
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
        public async Task<ActionResult> Register(RolesViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                if (!roleManager.RoleExists(model.Nombre))
                {
                    var role = new IdentityRole
                    {
                        Name = model.Nombre
                    };
                    roleManager.Create(role);
                }

                return RedirectToAction("Index", "Roles");
            }
            return View(model);
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
    }
}