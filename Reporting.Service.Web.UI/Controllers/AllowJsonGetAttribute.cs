using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Models
{
    public class AllowJsonGetAttribute: ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var jsonResult = filterContext.Result as JsonResult;

            if (jsonResult == null)
                throw new ArgumentException("Action does not return a JsonResult, attribute AllowJsonGet is not allowed");
    

            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            base.OnResultExecuting(filterContext);
        }
    }
}