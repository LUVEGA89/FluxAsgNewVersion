using Reporting.Service.Core.MenuByUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Reporting.Service.Web.UI.Controllers;
using Reporting.Service.Core.MenuByUser.Vistas;
using Reporting.Service.Core.MenuByUser.Modulo;
using Microsoft.AspNet.Identity;

namespace Reporting.Service.Web.UI.Controllers
{
    public class MenuByUserController : Controller
    {
        ModuloCatalog moduloCatalog = null;
        VistaCatalog vistaCatalog = null;

        public MenuByUserController()
        {
            moduloCatalog = new ModuloCatalog();
            vistaCatalog = new VistaCatalog();
        }

        // GET: MenuByUser        
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            Reporting.Service.Web.UI.Models.MenuByUser model = new Models.MenuByUser();
            model.modulos = moduloCatalog.FindPagedItems(new ModuloCriteria() { Tipo = ModuloKind.Admin }).ToList();

            model.usuarios = moduloCatalog.GetUserAccess();
            model.vistas = vistaCatalog.GetVistas();
            return View(model);
        }
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "Error")
        {
            return this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });
        }
        public JsonResult VistaPrincipal()
        {
            var modulos = moduloCatalog.FindPagedItems(new ModuloCriteria() { Tipo = ModuloKind.Admin }).ToList();
            return JsonResponse(modulos, 200, "Success");
        }
        //[Authorize(Roles ="Administrador")]
        public JsonResult GetModulos()
        {
            var modulos = moduloCatalog.GetModulos();
            return JsonResponse(modulos, 200, "Success");
        }
        public JsonResult GetVistas()
        {
            var vistas = vistaCatalog.GetVistas();
            return JsonResponse(vistas, 200, "Success");
        }
        public JsonResult GetVistasUsuarios()
        {
            var vistas = vistaCatalog.GetVistasUsuarios();
            return JsonResponse(vistas, 200, "Success");
        }
        //[Authorize(Roles = "Administrador")]
        public JsonResult AddModulo(string Icon, string Nombre, int? Padre = null)
        {
            try
            {
                Modulo modulo = new Modulo()
                {
                    Nombre = Nombre,
                    Icon = Icon,
                    Padre = Padre
                };
                bool result = moduloCatalog.Add(modulo);
                if (result)
                {
                    return JsonResponse("Agregado con exito", 200, "Success");
                }
                else
                {
                    return JsonResponse();
                }
            }
            catch (Exception ex)
            {
                return JsonResponse(ex.Message, 0, ex.StackTrace);
            }
        }

        public JsonResult AddVista(Vista vista)
        {
            try
            {
                bool result = vistaCatalog.Add(vista);
                if (result)
                {
                    return JsonResponse("Agregado con exito", 200, "Success");
                }
                else
                {
                    return JsonResponse("Error al agregar vista");
                }
            }
            catch (Exception ex)
            {
                return JsonResponse(ex.Message, 0, ex.StackTrace);
            }
        }
        public JsonResult UpdateVista(Vista vista)
        {
            try
            {
                bool result = vistaCatalog.Update(vista);
                if (result)
                {
                    return JsonResponse("Agregado con exito", 200, "Success");
                }
                else
                {
                    return JsonResponse("Error al agregar vista");
                }
            }
            catch (Exception ex)
            {
                return JsonResponse(ex.Message, 0, ex.StackTrace);
            }
        }
        public JsonResult AddVistaUser(List<Vista> vistas, List<UserAccess> users)
        {
            bool Sucess = false;
            try
            {
                UserAccess userAccess = new UserAccess();
                userAccess.IdUser = (users[0].IdUser).Replace("Identificador-", "");
                userAccess.Email = users[0].Email;
                Sucess = moduloCatalog.AddVistaUser(vistas, userAccess);
                if (Sucess)
                {
                    return JsonResponse("Vista agregado a usuario con exito", 200, "Success");
                }
                else
                {
                    return JsonResponse("Error al agregar vista a usuario");
                }
            }
            catch (Exception ex)
            {
                return JsonResponse(ex.Message, 0, ex.StackTrace);
            }

        }
        public JsonResult ControlVistaUsuario(Vista vista)
        {
            try
            {
                Vista vis = new Vista();
                vis.IdUsuario = vista.IdUsuario.Replace("Identificador-", "");
                vis.Identifier = vista.Identifier;
                vis.Activo = vista.Activo;
                bool result = moduloCatalog.ControlVistaUser(vis);
                if (result)
                {
                    return JsonResponse("Agregado con exito", 200, "Success");
                }
                else
                {
                    return JsonResponse("Error al agregar vista");
                }
            }
            catch (Exception ex)
            {
                return JsonResponse(ex.Message, 0, ex.StackTrace);
            }
        }

        public JsonResult GetValidaCorreo(string correo = null)
        {
            try
            {
                Core.Common.CommonManager manager = new Core.Common.CommonManager();
                var x = manager.GetUsuario(correo);
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return JsonResponse(ex.Message, 0, ex.StackTrace);
            }
        }

        // Traer los vistas disponibles para el usuario actual
        public JsonResult GetPermisosVistaUsuario(string correo = null)
        {
            try
            {
                Core.Common.CommonManager manager = new Core.Common.CommonManager();
                var x = manager.GetUsuario(correo);

                Core.MenuByUser.Modulo.ModuloCatalog moduloManager = new ModuloCatalog();
                var view = moduloManager.FindPagedItems(new ModuloCriteria() { Tipo = ModuloKind.User, UuidUser = x.Identifier }).ToList();
                return this.JsonResponse(view);
            }
            catch (Exception ex)
            {
                return JsonResponse(ex.Message, 0, ex.StackTrace);
            }
        }

        public JsonResult GetPermisosUsuario(string correo = null)
        {
            try
            {
                Core.Common.CommonManager manager = new Core.Common.CommonManager();
                var x = manager.GetUsuario(correo);

                Core.MenuByUser.Modulo.ModuloCatalog moduloManager = new ModuloCatalog();                
                var view = moduloManager.FindPagedItems(new ModuloCriteria() { Tipo = ModuloKind.Admin }).ToList();

                var x1 = moduloManager.Find(1);
                return this.JsonResponse(view);
            }
            catch (Exception ex)
            {
                return JsonResponse(ex.Message, 0, ex.StackTrace);
            }
        }


        public JsonResult AddPermisosUsuario(List<UserAccess> arrayViewUser, List<string> arrayUser)
        {
            try
            {
                if (moduloCatalog == null)
                {
                    moduloCatalog = new ModuloCatalog();
                }
                var x = moduloCatalog.AddPermisosUsuario(arrayViewUser, arrayUser);
                return this.JsonResponse(x);
            }
            catch (Exception ex)
            {
                return JsonResponse(ex.Message, 0, ex.StackTrace);
            }
        }


    }
}