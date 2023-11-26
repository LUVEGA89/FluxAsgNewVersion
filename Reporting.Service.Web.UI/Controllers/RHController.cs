using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class RHController : Controller
    {
        // GET: RH
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Requisicion()
        {
            return View();
        }
    }
}