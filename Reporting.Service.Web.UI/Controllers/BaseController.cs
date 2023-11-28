using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

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
        }
    }
}