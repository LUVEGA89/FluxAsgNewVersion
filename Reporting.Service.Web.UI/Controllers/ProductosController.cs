using Microsoft.AspNet.Identity;
using NPOI.XWPF.UserModel;
using OfficeOpenXml;
using Reporting.Service.Core.Productos;
using Reporting.Service.Core.SAP;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class ProductosController : Controller
    {
        private SAPManager _manager = new SAPManager();

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

        public ActionResult Reporte()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
        public JsonResult ListaProductos(DateTime Del, DateTime Al)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetArticulos8020(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult DecrementoArticulos(DateTime Del, DateTime Al)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetDecrementoArticulos(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult DecrementoCliente(DateTime Del, DateTime Al)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetDecrementoCliente(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult PiezasPorSku(DateTime Del, DateTime Al)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetPiezasPorSku(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult PiezasPorCliente(DateTime Del, DateTime Al)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetPiezasPorCliente(Del, Al);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public ActionResult ExcelDecrementoArticulos(DateTime Del, DateTime Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Detalle Decremento-Articulos.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Detalle Decremento-Articulos.xls");
            }

            ProductoManager manager = new ProductoManager();
            var result = manager.GetDecrementoArticulosExcel(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Reporte - Decremento articulos";
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
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Detalle Decremento-Articulos.xls" }
            };

        }
        public ActionResult ExcelProductos(DateTime Del, DateTime Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Detalle Articulos-8020.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Detalle Articulos-8020.xls");
            }

            ProductoManager manager = new ProductoManager();
            var result = manager.GetArticulos8020Excel(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Reporte - Articulos 8020";
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
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Detalle Articulos-8020.xls" }
            };

        }
        public ActionResult ExcelDecrementoCliente(DateTime Del, DateTime Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Detalle Decremento-Clientes.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Detalle Decremento-Clientes.xls");
            }

            ProductoManager manager = new ProductoManager();
            var result = manager.GetDecrementoClienteExcel(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Reporte - Articulos 8020";
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
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Detalle Decremento-Clientes.xls" }
            };

        }
        public ActionResult ExcelPiezasPorSku(DateTime Del, DateTime Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Piezas-Sku.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Piezas-Sku.xls");
            }

            ProductoManager manager = new ProductoManager();
            var result = manager.GetPiezasPorSkuExcel(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Reporte - Piezas por Sku";
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
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Piezas-Sku.xls" }
            };

        }
        public ActionResult ExcelPiezasPorCliente(DateTime Del, DateTime Al)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            FileInfo newFile = new FileInfo(path + @"Piezas-Clientes.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(path + @"\Piezas-Clientes.xls");
            }

            ProductoManager manager = new ProductoManager();
            var result = manager.GetPiezasPorClienteExcel(Del, Al);

            ExcelPackage workbook = new ExcelPackage(newFile);

            ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
            objWorksheet.Cells["A1"].LoadFromDataTable(result, true);

            workbook.Workbook.Properties.Title = "Reporte - Piezas por cliente";
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
            return new JsonResult()
            {
                Data = new { FileGuid = handle, FileName = "Piezas-Clientes.xls" }
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
        //AQUI EMPIEZA LOS PRODUCTOS DE CHINA      
        public ActionResult skuchina()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ProductoManager productoManager = new ProductoManager();

            SkuChinaModel Model = new SkuChinaModel()
            {
                Familias = productoManager.GetFamiliasSAP()/*,
                Categorias = productoManager.GetCategoriasSAP(),
                Clasificaciones = productoManager.GetClasificacionesSAP(),
                Tipos = productoManager.GetTipossSAP()*/
            };

            return View(Model);
        }
        public JsonResult ListaProductosChina(string Sku, string Familia, string Categoria, string Clasificacion, string Tipo)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetProductosChina(Sku, Familia, Categoria, Clasificacion, Tipo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult PreciosFacturaChina(string Sku)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetPreciosFacturaChina(Sku);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult PreciosCartaFacturaChina(string Sku)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetPreciosCartaFacturaChina(Sku);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult DetalleProductosChina(string Sku)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetDetalleProductosChina(Sku);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult ListasPreciosChina(string Sku)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetListasPreciosChina(Sku);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult OrdenesCompra(string Sku)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetOrdenesCompra(Sku);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult Envios(string Sku)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetEnvios(Sku);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetCategorias(string Familia)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetCategoriasSAP(Familia);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GetClasificaciones(string Categoria)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetClasificacionesSAP(Categoria);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public JsonResult GetTipos(string Categoria, string Clasificaciones)
        {
            try
            {
                ProductoManager manager = new ProductoManager();
                var result = manager.GetTiposSAP(Categoria, Clasificaciones);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GuardarComentario(string Sku, string Comentario)
        {
            try
            {
                
                ProductoManager manager = new ProductoManager();
                var result = manager.PutComment(Sku, Comentario, User.Identity.GetUserId());
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult Comentarios(string Sku)
        {
            try
            {

                ProductoManager manager = new ProductoManager();
                var result = manager.Comentarios(Sku);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult GETAnexos(string Sku)
        {
            try
            {
                var result = _manager.GetAnexos(Sku);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        //AQUI TERMINA LOS PRODUCTOS DE CHINA 

        #region Reporte detalle de productos llegada       

        public JsonResult DetalleProductosLlegadas(byte EsProduccion)
        {
            try
            {
                ProductoManager manager = new ProductoManager(System.Configuration.ConfigurationManager.AppSettings["ConexionProximasLlegadas"].ToString());
                var result = manager.GetProductosDetalleLlegadas(EsProduccion);
                var resultSec = result;
                if (result != null && result.Count>0)
                {
                    resultSec = result.OrderBy(x => x.Contenedor).ToList();
                }                
                return this.JsonResponse(resultSec);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message + ex.StackTrace);
            }
        }
        #endregion
    }
}
