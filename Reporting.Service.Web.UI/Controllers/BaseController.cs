using Reporting.Service.Web.UI.Models;
using Reporting.Service.Core.CreditoCobranza;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Reporting.Service.Core.Quotation;
using Reporting.Service.Core.Usuarios;
using Reporting.Service.Core.Cliente;
using Reporting.Service.Core.Servicio;
using Reporting.Service.Core.TipoServicio;
using Reporting.Service.Core.Disclaimers;
using Microsoft.AspNet.Identity;
using Resporting.Service.Core.Airport;
using System.Security.Claims;
using System.Globalization;
using NPOI.Util;

namespace Reporting.Service.Web.UI.Controllers
{
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
