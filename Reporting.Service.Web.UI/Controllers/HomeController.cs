using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Reporting.Service.Web.UI.Models;
using Microsoft.AspNet.Identity.EntityFramework;

using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.Common;
using Reporting.Service.Core.MenuByUser;
using Reporting.Service.Core.MenuByUser.Modulo;
using Reporting.Service.Core.MenuByUser.Menu;

namespace Reporting.Service.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext context;

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
        public HomeController()
        {
            context = new ApplicationDbContext();
        }
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Administrador")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        public IList<Rol> GetRoles()
        {
            List<Rol> roles = new List<Rol>();
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                foreach (string Item in s)
                {
                    switch (Item.ToString())
                    {
                        case "Administrador":
                            roles.Add(Rol.Administrador);
                            break;
                        case "Compras":
                            roles.Add(Rol.Compras);
                            break;
                        case "Tráfico":
                            roles.Add(Rol.Trafico);
                            break;
                        case "Ventas":
                            roles.Add(Rol.Ventas);
                            break;
                        case "Credito":
                            roles.Add(Rol.Credito);
                            break;
                        case "Dirección":
                            roles.Add(Rol.Direccion);
                            break;
                        case "Tiendas":
                            roles.Add(Rol.Tiendas);
                            break;
                        case "Finanzas":
                            roles.Add(Rol.Finanzas);
                            break;
                        case "Ecommerce":
                            roles.Add(Rol.Ecommerce);
                            break;
                        case "Business":
                            roles.Add(Rol.Business);
                            break;
                        case "Asistente":
                            roles.Add(Rol.Asistente);
                            break;
                        case "Precios":
                            roles.Add(Rol.Precios);
                            break;
                        case "Regional":
                            roles.Add(Rol.Regional);
                            break;
                        case "Inventarios":
                            roles.Add(Rol.Inventarios);
                            break;
                        case "Papeleria":
                            roles.Add(Rol.Papeleria);
                            break;
                        case "AdministracionPapeleria":
                            roles.Add(Rol.AdministracionPapeleria);
                            break;
                        case "Conciliacion":
                            roles.Add(Rol.Conciliacion);
                            break;
                        case "PedidosTienda":
                            roles.Add(Rol.PedidosTienda);
                            break;
                        case "Almacén":
                            roles.Add(Rol.Almacen);
                            break;
                        case "Gerencia":
                            roles.Add(Rol.Gerencia);
                            break;
                        case "Retail":
                            roles.Add(Rol.Retail);
                            break;
                        case "Supervisor":
                            roles.Add(Rol.Supervisor);
                            break;
                        case "Recursos Humanos":
                            roles.Add(Rol.RecursosHumanos);
                            break;
                        case "RetailAdmin":
                            roles.Add(Rol.RetailAdmin);
                            break;
                        case "Capacitacion":
                            roles.Add(Rol.Capacitacion);
                            break;
                        case "Aperturas":
                            roles.Add(Rol.Aperturas);
                            break;
                        case "AlmacenAdmin":
                            roles.Add(Rol.AlmacenAdmin);
                            break;
                        case "AsistenteDireccion":
                            roles.Add(Rol.AsistenteDireccion);
                            break;
                    }
                }
            }
            return roles;
        }
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View();
        }
        public ActionResult Lockout()
        {
            return View();
        }
        public ActionResult About()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult Contact()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            ViewBag.Message = "Your contact page.";
            return View();
        }
        public ActionResult Header()
        {
            //UserModel model = new UserModel();
            return PartialView();
        }
        public ActionResult Sidebar()
        {
            CommonManager manager = new CommonManager();
            var result = manager.GetDetalleUsuario(User.Identity.GetUserId());
            UserModel model = new UserModel();
            //model.IsAdmin = isAdminUser();
            model.Roles = GetRoles();
            model.Area = result.Area;
            model.Nombre = result.Usuario;
            model.CodigoEmpleado = result.CodigoEmpleado;
            //model.Menu = GetMenuByUser();
            return PartialView("Sidebar", model);
        }

        // Aplica de Menu Dinamico
        public Menu GetMenuByUser()
        {
            ModuloCatalog catalog = new ModuloCatalog();
            Menu menu = new Menu();
            string user = (User.Identity).GetUserId();

            var x = catalog.FindPagedItems(new ModuloCriteria()
            {
                UuidUser = user,
                Tipo = ModuloKind.User
            });
            menu.modulos = x.ToList();

            return menu;
        }

        public ActionResult LoginPartial()
        {
            CommonManager manager = new CommonManager();
            var result = manager.GetDetalleUsuario(User.Identity.GetUserId());
            UserModel model = new UserModel();
            //model.IsAdmin = isAdminUser();
            model.Roles = GetRoles();
            model.Area = result.Area;
            model.Nombre = result.Usuario;
            model.CodigoEmpleado = result.CodigoEmpleado;
            return PartialView("_LoginPartial", model);
        }
    }
}