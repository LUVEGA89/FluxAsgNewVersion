using System;
using System.Collections.Generic;
using Reporting.Service.Core.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/
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

        public JsonResult GetUserRolByEmail(string Email)
        {
            try
            {
                RolManager manager = new RolManager();
                var result = manager.CoreGetUserRolByEmail(Email);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }
    }
}
