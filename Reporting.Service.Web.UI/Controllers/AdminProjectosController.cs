using OfficeOpenXml;
using Reporting.Service.Web.UI.Models;
using Reporting.Service.Core.Venta;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Reporting.Service.Core.Common;
using Reporting.Service.Core.AdminProyectos;
using System.Data;

namespace Reporting.Service.Web.UI.Controllers
{
    public class AdminProjectosController : Controller
    {

        [NonAction]
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
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

        public JsonResult ApGetProyecto()
        {
            try
            {
                AdminProyectosManager manager = new AdminProyectosManager();
                var result = manager.CoreApGetProyecto();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

    }
}
