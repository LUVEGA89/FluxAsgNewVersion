using OfficeOpenXml;
using Reporting.Service.Core.Consejo;
using Reporting.Service.Core.EvaluacionVendedor;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class EvaluacionController : Controller
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
        //
        // GET: /Evaluacion/

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        public ActionResult Retail()
        {
            return View();
        }
        public ActionResult Consejo()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult CostoVenta()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult DetalleCanal(DateTime Del, DateTime Al, int Tipo = 0)
        {
            try
            {
                ConsejoManager manager = new ConsejoManager();
                var result = manager.GetDetalleCanal(Del, Al, Tipo);

                decimal TotalItems = 0;
                decimal TotalAñoActual = 0;
                decimal TotalAñoAnterior = 0;

                if (result != null)
                {
                    TotalItems = result.Count;
                    TotalAñoActual = result.Sum(rs => rs.MontoAñoAct);
                    TotalAñoAnterior = result.Sum(rs => rs.MontoAñoAnt);
                }
                foreach(var item in result)
                {
                    //Porcentaje de participacion
                    item.ActualPorcentPart = TotalAñoActual == 0 ? 0 : (item.MontoAñoAct * 100) / TotalAñoActual;
                    item.AnteriorPorcentPart = TotalAñoAnterior == 0 ? 0 : (item.MontoAñoAnt * 100) / TotalAñoAnterior;
                    //Porcentaje de utilidad
                    item.ActualPorcentUtilidad = (item.MontoAñoAct == 0) ? 0 : (item.UtilidadAñoAct * 100) / item.MontoAñoAct; 
                    item.AnteriorPorcentUtilidad = (item.MontoAñoAnt == 0)? 0 : (item.UtilidadAñoAnt * 100) / item.MontoAñoAnt;
                    //porcentaje de crecimiento
                    item.PorcentajeCrecimiento = (item.MontoAñoAnt == 0) ? 0 : ((item.MontoAñoAct / item.MontoAñoAnt) - 1) * 100;
                }

                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult BuscarVendedor(string Vendedor)
        {
            try
            {
                EvaluacionManager manager = new EvaluacionManager();
                var result = manager.GetVendedor(Vendedor);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult DetalleAgente(int Vendedor)
        {
            try
            {
                EvaluacionManager manager = new EvaluacionManager();
                var result = manager.GetDetalleAgente(Vendedor);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult VentasEstados(int Vendedor)
        {
            try
            {
                EvaluacionManager manager = new EvaluacionManager();
                var result = manager.GetTopVentasEstado(Vendedor);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult ClientesEstadosPorcentaje(int Vendedor)
        {
            try
            {
                EvaluacionManager manager = new EvaluacionManager();
                decimal TotalItems = 0;
                decimal TotalCantidad = 0;
                var result = manager.GetPorcentajeClientesEstado(Vendedor);
                if(result != null && result.Count > 0)
                {
                    TotalItems = result.Count;
                    TotalCantidad = result.Sum(rs => rs.Cantidad);
                    foreach (var item in result)
                    {
                        item.Porcentaje = (item.Cantidad * 100) / TotalCantidad;
                    }
                    return this.JsonResponse(result);
                }
                else
                {
                    return this.JsonResponse(null, -1, "No se encontraron dato del vendedor");
                }
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult VentasClientes(int Vendedor)
        {
            try
            {
                EvaluacionManager manager = new EvaluacionManager();
                var result = manager.GetTopVentasClientes(Vendedor);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult FacturadoCartaFactura(DateTime Del, DateTime Al)
        {
            try
            {
                ConsejoManager manager = new ConsejoManager();
                var result = manager.GetCartafacturaFacturado(Del, Al);
                decimal TotalItems = 0;
                decimal TotalAñoActual = 0;
                decimal TotalAñoAnterior = 0;

                if (result != null)
                {
                    TotalItems = result.Count;
                    TotalAñoActual = result.Sum(rs => rs.MontoAñoAct);
                    TotalAñoAnterior = result.Sum(rs => rs.MontoAñoAnt);
                }
                foreach (var item in result)
                {
                    //Porcentaje de participacion
                    item.ActualPorcentPart = TotalAñoActual == 0 ? 0 : (item.MontoAñoAct * 100) / TotalAñoActual;
                    item.AnteriorPorcentPart = TotalAñoAnterior == 0 ? 0 : (item.MontoAñoAnt * 100) / TotalAñoAnterior;
                    //Porcentaje de utilidad
                    item.ActualPorcentUtilidad = (item.MontoAñoAct == 0) ? 0 : (item.UtilidadAñoAct * 100) / item.MontoAñoAct;
                    item.AnteriorPorcentUtilidad = (item.MontoAñoAnt == 0) ? 0 : (item.UtilidadAñoAnt * 100) / item.MontoAñoAnt;
                    //porcentaje de crecimiento
                    item.PorcentajeCrecimiento = (item.MontoAñoAnt == 0) ? 0 : ((item.MontoAñoAct / item.MontoAñoAnt) - 1) * 100;
                }
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GetDetalleEstados(DateTime Del, DateTime Al, int Tipo)
        {
            try
            {
                ConsejoManager manager = new ConsejoManager();
                var result = manager.GetDetallePorEstado(Del, Al, Tipo);
                decimal TotalItems = 0;
                decimal TotalAñoActual = 0;
                decimal TotalAñoAnterior = 0;

                if (result != null)
                {
                    TotalItems = result.Count;
                    TotalAñoActual = result.Sum(rs => rs.MontoAñoAct);
                    TotalAñoAnterior = result.Sum(rs => rs.MontoAñoAnt);
                }
                foreach (var item in result)
                {
                    //Porcentaje de participacion
                    item.ActualPorcentPart = TotalAñoActual == 0 ? 0 : (item.MontoAñoAct * 100) / TotalAñoActual;
                    item.AnteriorPorcentPart = TotalAñoAnterior == 0 ? 0 : (item.MontoAñoAnt * 100) / TotalAñoAnterior;
                    //Porcentaje de utilidad
                    item.ActualPorcentUtilidad = (item.MontoAñoAct == 0) ? 0 : (item.UtilidadAñoAct * 100) / item.MontoAñoAct;
                    item.AnteriorPorcentUtilidad = (item.MontoAñoAnt == 0) ? 0 : (item.UtilidadAñoAnt * 100) / item.MontoAñoAnt;
                    //porcentaje de crecimiento
                    item.PorcentajeCrecimiento = (item.MontoAñoAnt == 0) ? 0 : ((item.MontoAñoAct / item.MontoAñoAnt) - 1) * 100;
                }
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public ActionResult ExcelDetalleEstados(DateTime Del, DateTime Al, int Tipo)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            FileInfo newFile = new FileInfo(path + @"Reporte Por estado.xls");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(path + @"\Reporte Por estado.xls");
            }
            ConsejoManager manager = new ConsejoManager();
            var result = manager.GetDetallePorEstado(Del, Al, Tipo);
            decimal TotalItems = 0;
            decimal TotalAñoActual = 0;
            decimal TotalAñoAnterior = 0;

            if (result != null)
            {
                TotalItems = result.Count;
                TotalAñoActual = result.Sum(rs => rs.MontoAñoAct);
                TotalAñoAnterior = result.Sum(rs => rs.MontoAñoAnt);
            }
            foreach (var item in result)
            {
                //Porcentaje de participacion
                item.ActualPorcentPart = TotalAñoActual == 0 ? 0 : (item.MontoAñoAct * 100) / TotalAñoActual;
                item.AnteriorPorcentPart = TotalAñoAnterior == 0 ? 0 : (item.MontoAñoAnt * 100) / TotalAñoAnterior;
                //Porcentaje de utilidad
                item.ActualPorcentUtilidad = (item.MontoAñoAct == 0) ? 0 : (item.UtilidadAñoAct * 100) / item.MontoAñoAct;
                item.AnteriorPorcentUtilidad = (item.MontoAñoAnt == 0) ? 0 : (item.UtilidadAñoAnt * 100) / item.MontoAñoAnt;
                //porcentaje de crecimiento
                item.PorcentajeCrecimiento = (item.MontoAñoAnt == 0) ? 0 : ((item.MontoAñoAct / item.MontoAñoAnt) - 1) * 100;
            }
            var datat = CreateDataTable(result);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(datat, true);

            workbook.Workbook.Properties.Title = "Trafico";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Reporte Por estado.xls" }
            };

        }

        public ActionResult ExcelDetalleBYEstados(DateTime Del, DateTime Al, int Tipo, string Estado)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            FileInfo newFile = new FileInfo(path + @"Reporte Por estado-Cliente.xls");

            if (newFile.Exists)
            {
                newFile.Delete();  // ensures we create a new workbook
                newFile = new FileInfo(path + @"\Reporte Por estado-Cliente.xls");
            }
            ConsejoManager manager = new ConsejoManager();
            var result = manager.GetDetalleByEstado(Del, Al, Tipo, Estado);
            decimal TotalItems = 0;
            decimal TotalAñoActual = 0;
            decimal TotalAñoAnterior = 0;

            if (result != null)
            {
                TotalItems = result.Count;
                TotalAñoActual = result.Sum(rs => rs.MontoAñoAct);
                TotalAñoAnterior = result.Sum(rs => rs.MontoAñoAnt);
            }
            foreach (var item in result)
            {
                //Porcentaje de participacion
                item.ActualPorcentPart = (item.MontoAñoAct * 100) / (TotalAñoActual == 0 ? 1 : TotalAñoActual);
                item.AnteriorPorcentPart = (item.MontoAñoAnt * 100) /  (TotalAñoAnterior == 0 ? 1 : TotalAñoAnterior);
                //Porcentaje de utilidad
                item.ActualPorcentUtilidad = (item.MontoAñoAct == 0) ? 0 : (item.UtilidadAñoAct * 100) / item.MontoAñoAct;
                item.AnteriorPorcentUtilidad = (item.MontoAñoAnt == 0) ? 0 : (item.UtilidadAñoAnt * 100) / item.MontoAñoAnt;
                //porcentaje de crecimiento
                item.PorcentajeCrecimiento = (item.MontoAñoAnt == 0) ? 0 : ((item.MontoAñoAct / item.MontoAñoAnt) - 1) * 100;
            }
            var datat = CreateDataTable(result);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(datat, true);

            workbook.Workbook.Properties.Title = "Trafico";
            workbook.Workbook.Properties.Author = "Ricardo Alonso";
            workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1828");

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }
            workbook.SaveAs(newFile);
            // Note we are returning a filename as well as the handle
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Reporte Por estado-Cliente.xls" }
            };

        }

        [HttpGet]
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
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }
        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GetDetalleCategoria(DateTime Del, DateTime Al)
        {
            try
            {
                ConsejoManager manager = new ConsejoManager();
                var result = manager.GetDetallePorCategoria(Del, Al);
                decimal TotalItems = 0;
                decimal TotalAñoActual = 0;
                decimal TotalAñoAnterior = 0;

                if (result != null)
                {
                    TotalItems = result.Count;
                    TotalAñoActual = result.Sum(rs => rs.MontoAñoAct);
                    TotalAñoAnterior = result.Sum(rs => rs.MontoAñoAnt);
                }
                foreach (var item in result)
                {
                    //Porcentaje de participacion
                    item.ActualPorcentPart = TotalAñoActual == 0 ? 0 : (item.MontoAñoAct * 100) / TotalAñoActual;
                    item.AnteriorPorcentPart = TotalAñoAnterior == 0 ? 0 : (item.MontoAñoAnt * 100) / TotalAñoAnterior;
                    //Porcentaje de utilidad
                    item.ActualPorcentUtilidad = (item.MontoAñoAct == 0) ? 0 : (item.UtilidadAñoAct * 100) / item.MontoAñoAct;
                    item.AnteriorPorcentUtilidad = (item.MontoAñoAnt == 0) ? 0 : (item.UtilidadAñoAnt * 100) / item.MontoAñoAnt;
                    //porcentaje de crecimiento
                    item.PorcentajeCrecimiento = (item.MontoAñoAnt == 0) ? 0 : ((item.MontoAñoAct / item.MontoAñoAnt) - 1) * 100;
                }
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult GetTotalesRetail(DateTime Del, DateTime Al)
        {
            try
            {
                Core.Retail.RetailCatalog manager = new Core.Retail.RetailCatalog();
                var result = manager.GetDatosRetail(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetCostoVenta(DateTime Del, DateTime Al)
        {
            try
            {
                EvaluacionManager manager = new EvaluacionManager();
                var result = manager.getCostoVenta(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult getArticulos(string cliente, DateTime Del, DateTime Al)
        {
            try
            {
                EvaluacionManager manager = new EvaluacionManager();
                var result = manager.getArticulos(cliente, Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult getTiendasEstado(string Estado, DateTime Del, DateTime Al,int Tipo)
        {
            try
            {
                EvaluacionManager manager = new EvaluacionManager();
                var result = manager.getTiendasEstado(Estado, Del, Al,Tipo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}
