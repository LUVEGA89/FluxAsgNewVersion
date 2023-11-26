using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OfficeOpenXml;
using Reporting.Service.Core.PDF;
using Reporting.Service.Web.UI.Models;

namespace Reporting.Service.Web.UI.Controllers
{
    public class PdfManagerController : Controller
    {
        private PdfManagerD _manager;
        private ApplicationUserManager _userManager;
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
        public PdfManagerController()
        {
            _manager = new PdfManagerD();
        }
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
        {
            var jsonResult = this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });

            return jsonResult;
        }
        // GET: PdfManager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tiendas()
        {
            ListasPreciosModel model = new ListasPreciosModel();

            PdfManagerD pdfManager = new PdfManagerD();

            model.ListasPrecios = pdfManager.FindPriceList(User.Identity.GetUserName());

            return View(model);
        }

        public JsonResult GetProductos()
        {
            try
            {
                var result = _manager.FindProductos();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult AddProducto(int Producto)
        {
            try
            {
                _manager.AddProducto(Producto, User.Identity.GetUserId());
                return this.JsonResponse("");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult DelProductoPDF(int Sequence)
        {
            try
            {
                _manager.DelProductoPlantilla(Sequence, User.Identity.GetUserId());
                return this.JsonResponse("");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult DelPlantillaPDF()
        {
            try
            {
                _manager.DelPlantilla(User.Identity.GetUserId());
                return this.JsonResponse("");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult AddLista(int Lista, int Tipo)
        {
            try
            {
                _manager.AddLista(Lista, Tipo, User.Identity.GetUserId());
                return this.JsonResponse("");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult UpdatePrecio(int Producto, decimal Precio)
        {
            try
            {
                _manager.UpdatePrecio(Producto, Precio, User.Identity.GetUserId());
                return this.JsonResponse("");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetProductosPlantilla()
        {
            try
            {
                var result = _manager.FindProductosPDF(User.Identity.GetUserId());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetListasPlantilla()
        {
            try
            {
                var result = _manager.FindListasPDF(User.Identity.GetUserId());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GeneraPDF(string Tipo)
        {
            try
            {
                string ruta = Server.MapPath("~/Reports/Productos.rpt");

                var result = _manager.FillPlantillaPDF(User.Identity.GetUserId());

                string FileName = string.Format("Lista-Productos-"+ Tipo + "-"+ Guid.NewGuid().ToString() + ".pdf");
                if (System.IO.File.Exists(Server.MapPath("~/Documentos/" + FileName)))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(Server.MapPath("~/Documentos/" + FileName));
                    Console.WriteLine("File deleted.");
                }
                ReportDocument report = new ReportDocument();
                report.FileName = Server.MapPath("~/Reports/Productos.rpt");
                report.Load(ruta);
                report.Database.Tables[0].SetDataSource(result);

                report.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Documentos/" + FileName));

                return this.JsonResponse(FileName);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
            
        }

       
        public ActionResult GeneraExcel(string Tipo)
        {
            try
            {
                string ruta = Server.MapPath("~/Reports/Productos.rpt");

                var result = _manager.FillPlantillaPDF(User.Identity.GetUserId());

                string FileName = string.Format("Lista-Productos-" + Tipo + "-" + Guid.NewGuid().ToString() + ".xls");
                if (System.IO.File.Exists(Server.MapPath("~/Documentos/" + FileName)))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(Server.MapPath("~/Documentos/" + FileName));
                    Console.WriteLine("File deleted.");
                }
                ReportDocument report = new ReportDocument();
                report.FileName = Server.MapPath("~/Reports/Productos.rpt");
                report.Load(ruta);
                report.Database.Tables[0].SetDataSource(result);

                report.ExportToDisk(ExportFormatType.Excel, Server.MapPath("~/Documentos/" + FileName));

                return this.JsonResponse(FileName);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

    }
}