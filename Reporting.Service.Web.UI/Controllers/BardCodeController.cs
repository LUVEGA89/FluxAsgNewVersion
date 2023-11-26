using Reporting.Service.Core.BardCode;
using Reporting.Service.Core.EmpresasSap;
using Reporting.Service.Core.Proveedores;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class BardCodeController : JsonController
    {
        // GET: Proveedor
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult BardCode()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public JsonResult CreateBarCode(int OrdenCompra)
        {
            try
            {
                BardCodeManager manager = new BardCodeManager();
                var result = manager.Find(OrdenCompra);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}