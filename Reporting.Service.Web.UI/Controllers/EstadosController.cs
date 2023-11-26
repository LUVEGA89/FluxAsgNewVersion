using Reporting.Service.Core.Estados;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using Reporting.Service.Web.UI.Models;
using System.Runtime.InteropServices;

namespace Reporting.Service.Web.UI.Controllers
{
    public class EstadosController : Controller
    {
        [NonAction]
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
        {
            var serialize = new System.Web.Script.Serialization.JavaScriptSerializer();
            serialize.MaxJsonLength = 500000000;
            var result = this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });
            result.MaxJsonLength = 500000000;
            return result;
            //return this.Json(new
            //{
            //    Context = context,
            //    Code = code,
            //    Message = message
            //});
        }
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        public ActionResult Relacion()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        public ActionResult Conciliacion()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        //STEUBEN BANCOMER
        public JsonResult GetEstadoBancomer()
        {
            try
            {
                BancomerManager manager = new BancomerManager();
                var result = manager.CoreGetEstadoBancomer();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ActualizarEstadoDeCuentaSteubenBancomer(string Sequence, string Sucursal, string FecDiaVenta, string TipoPago, string Comentario)
        {
            try
            {
                BancomerManager manager = new BancomerManager();
                var result = manager.CoreUpdateDataSteubenBancomer(Sequence, Sucursal, FecDiaVenta, TipoPago, Comentario);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetEstadoBancomerBySequence(string Sequence)
        {
            try
            {
                BancomerManager manager = new BancomerManager();
                var result = manager.CoreGetEstadoBancomerBySequence(Sequence);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //STEUBEN BANORTE
        public JsonResult GetEstadoBanorte()
        {
            try
            {
                BanorteManager manager = new BanorteManager();
                var result = manager.CoreGetEstadoBanorte();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ActualizarEstadoDeCuentaSteubenBanorte(string Sequence, string Sucursal, string FecDiaVenta, string TipoPago, string Comentario)
        {
            try
            {
                BanorteManager manager = new BanorteManager();
                var result = manager.CoreUpdateDataSteubenBanorte(Sequence, Sucursal, FecDiaVenta, TipoPago, Comentario);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //STEUBEN INBURSA
        public JsonResult GetEstadoInbursa()
        {
            try
            {
                InbursaManager manager = new InbursaManager();
                var result = manager.CoreGetEstadoInbursa();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ActualizarEstadoDeCuentaInbursa(string Sequence, string Sucursal, string FecDiaVenta, string TipoPago, string Comentario)
        {
            try
            {
                InbursaManager manager = new InbursaManager();
                var result = manager.CoreUpdateDataInbursa(Sequence, Sucursal, FecDiaVenta, TipoPago, Comentario);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //STEUBEN OKKU BANORTE
        public JsonResult GetEstadoOkkuBanorte()
        {
            try
            {
                BanorteOkkuManager manager = new BanorteOkkuManager();
                var result = manager.CoreGetEstadoOkkuBanorte();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ActualizarEstadoDeCuentaOkkuBanorte(string Sequence, string Sucursal, string FecDiaVenta, string TipoPago, string Comentario)
        {
            try
            {
                BanorteOkkuManager manager = new BanorteOkkuManager();
                var result = manager.CoreUpdateDataOkkuBanorte(Sequence, Sucursal, FecDiaVenta, TipoPago, Comentario);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //EXPORTAR A EXCEL
        public ActionResult GetReportExcelSteubenBancomer(string Del, string Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Relacion-Bancomer.xls");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Relacion-Bancomer.xls");
            }
            BancomerManager manager = new BancomerManager();
            var result = manager.CoreGetReportExcelSteubenBancomer(Del, Al);
            ExcelPackage workbook = new ExcelPackage(newFile);
            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);
            workbook.Workbook.Properties.Title = "Relacion-Bancomer";
            workbook.Workbook.Properties.Author = "Eli Álvarez";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "2435");
            string handle = Guid.NewGuid().ToString();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Relacion-Steuben-Bancomer.xls" }
            };
        }
        public ActionResult GetReportExcelSteubenBanorte(string Del, string Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Relacion-Steuben-Banorte.xls");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Relacion-Steuben-Banorte.xls");
            }
            BanorteManager manager = new BanorteManager();
            var result = manager.CoreGetReportExcelSteubenBanorte(Del, Al);
            ExcelPackage workbook = new ExcelPackage(newFile);
            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);
            workbook.Workbook.Properties.Title = "Relacion-Steuben-Banorte";
            workbook.Workbook.Properties.Author = "Eli Álvarez";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "2435");
            string handle = Guid.NewGuid().ToString();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Relacion-Steuben-Banorte.xls" }
            };
        }

        public ActionResult GetReportExcelOkkuBanorte(string Del, string Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Relacion-Okku-Bancomer.xls");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Relacion-Okku-Bancomer.xls");
            }
            BanorteOkkuManager manager = new BanorteOkkuManager();
            // 
            var result = manager.CoreGetReportExcelOkkuBanorte(Del, Al);
            ExcelPackage workbook = new ExcelPackage(newFile);
            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);
            workbook.Workbook.Properties.Title = "Relacion-Okku-Banorte";
            workbook.Workbook.Properties.Author = "Eli Álvarez";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "2435");
            string handle = Guid.NewGuid().ToString();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Relacion-Okku-Banorte.xls" }
            };
        }


        public ActionResult GetReportExcelSteubenInbursa(string Del, string Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Relacion-Steuben-Inbursa.xls");
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Relacion-Steuben-Inbursa.xls");
            }
            InbursaManager manager = new InbursaManager();
            var result = manager.CoreGetReportExcelSteubenInbursa(Del, Al);
            ExcelPackage workbook = new ExcelPackage(newFile);
            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);
            workbook.Workbook.Properties.Title = "Relacion-Steuben-Inbursa";
            workbook.Workbook.Properties.Author = "Eli Álvarez";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "2435");
            string handle = Guid.NewGuid().ToString();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Relacion-Steuben-Inbursa.xls" }
            };
        }
        public ActionResult Download(string fileGuid, string fileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }
        //CONFIRMACION BANCOMER

        public JsonResult ConfirmaCuentaBancomer()
        {
            try
            {
                BancosManager manager = new BancosManager();
                var result = manager.CoreUpdateConfirmBancomer();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult RevertirConfirmaCuentaBancomer()
        {
            try
            {
                BancosManager manager = new BancosManager();
                var result = manager.CoreUpdateRevertConfirmBancomer();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }




        public JsonResult ConfirmaCuentaSteubenBanorte()
        {
            try
            {
                BancosManager manager = new BancosManager();
                var result = manager.CoreUpdateConfirmSteubenBanorte();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult RevertirConfirmaSteubenBanorte()
        {
            try
            {
                BancosManager manager = new BancosManager();
                var result = manager.CoreUpdateRevertConfirmSteubenBanorte();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ConfirmaCuentaInbursa()
        {
            try
            {
                BancosManager manager = new BancosManager();
                var result = manager.CoreUpdateConfirmSteubenInbursa();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult RevertirConfirmaCuentaInbursa()
        {
            try
            {
                BancosManager manager = new BancosManager();
                var result = manager.CoreUpdateRevertConfirmSteubenInbursa();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ConfirmaCuentaOkkuBanorte()
        {
            try
            {
                BancosManager manager = new BancosManager();
                var result = manager.CoreUpdateConfirmOkkuBanorte();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult RevertirConfirmaOkkuBanorte()
        {
            try
            {
                BancosManager manager = new BancosManager();
                var result = manager.CoreUpdateRevertConfirmOkkuBanorte();
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
       
        //CORTE GLOBAL
        public JsonResult GetRelacionCorteGlobalTiendas(DateTime FechaIni, DateTime FechaFin)
        {
            try
            {
                BancosManager manager = new BancosManager();
                var result = manager.CoreGetRelacionCorteGlobalTiendas(FechaIni, FechaFin);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetCompocisionCorteGlobal(DateTime Fecha, string Sucursal)
        {
            try
            {
                BancosManager manager = new BancosManager();
                var result = manager.CoreGetCompocisionCorteGlobal(Fecha, Sucursal);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

    }
}
