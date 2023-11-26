using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class JsonController : Controller
    {
        [NonAction]
        public JsonResult JsonResponse(object context = null, int code = 200, string message = "Success")
        {            
            var result = this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });
            result.MaxJsonLength = 500000000;
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result;

            //return this.Json(new
            //{
            //    Context = context,
            //    Code = code,
            //    Message = message
            //});
        }
    }
}