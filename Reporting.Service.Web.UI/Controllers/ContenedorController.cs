using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using Reporting.Service.Core.Trafico.Contenedor;
using Reporting.Service.Core.Trafico.Contenedor.ContenedorAnexo;
using Reporting.Service.Core.Trafico.Contenedor.ContenedorEnvio;
using Reporting.Service.Core.Trafico.Contenedor.Envio;
using Reporting.Service.Core.Trafico.Contenedor.Naviera;
using Reporting.Service.Core.Trafico.Contenedor.Seguimiento;
using Reporting.Service.Core.Trafico.Contenedor.SeguimientoComentario;
using Reporting.Service.Core.Trafico.Contenedor.StatusContenedor;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Reporting.Service.Web.UI.Controllers
{
    public class ContenedorController : Controller
    {
        ApplicationDbContext context;
        public ContenedorController()
        {
            context = new ApplicationDbContext();
        }
        [NonAction]
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
        {
            var result = this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            });
            result.MaxJsonLength = 500000000;
            return result;
            
        }
        // GET: Contenedor
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult Registrar()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ContenedorModel model = new ContenedorModel();

            NavieraManager naviManager = new NavieraManager();
            model.navieras = naviManager.FindPagedItems(new NavieraCriteria()).ToList();

            StatusContenedorManager statManager = new StatusContenedorManager();
            model.status = statManager.FindPagedItems(new StatusContenedorCriteria()).ToList();

            return View(model);
        }

        public ActionResult Envio()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ContenedorModelCheckBox model = new ContenedorModelCheckBox();

            ContenedorManager conteManager = new ContenedorManager();
            model.contenedores = conteManager.GetListContenedores(new ContenedorCriteria { statusIni=1, statusFin=2 }).ToList();

            return View(model);
        }

        public ActionResult Seguimiento()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ContenedorModel model = new ContenedorModel();

            StatusContenedorManager statManager = new StatusContenedorManager();
            model.status = statManager.FindPagedItems(new StatusContenedorCriteria()).ToList();

            return View(model);
        }

        public ActionResult ReporteSKUS()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public JsonResult AddContenedor(String Nombre, DateTime Fecha, string Naviera, int Status, decimal TipoCambio)
        {
            try
            {
                ContenedorManager manager = new ContenedorManager();
                Contenedor nuevo = new Contenedor();
                nuevo.nomContenedor = Nombre;
                nuevo.fechaCrea = Fecha;
                nuevo.naviera = new Naviera() { Identifier = Naviera };
                nuevo.statusContenedor_id = Status;
                nuevo.usuario = User.Identity.GetUserName();
                nuevo.tipoCambio = TipoCambio;
                var result = manager.Add(nuevo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult NombreContenedorRepetido(String Nombre)
        {
            try
            {
                ContenedorManager manager = new ContenedorManager();
                var contenedor = manager.FindPagedItems(new ContenedorCriteria { nomCompleto = Nombre });
                var result = true;
                if (contenedor.Length == 0)
                    result = false;
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult GetEnviosDisponibles()
        {
            try
            {
                EnvioManager envManager = new EnvioManager();
                DateTime inicio = DateTime.Now.AddMonths(-6);
                DateTime Fin = DateTime.Now;
                var result = envManager.FindPagedItems(new EnvioCriteria { Inicio = inicio, Fin = Fin });
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult getEnviosContenedor(int idContenedor)
        {
            try
            {
                ContenedorEnvioManager manager = new ContenedorEnvioManager();
                var result = manager.FindPagedItems(new ContenedorEnvioCriteria() { idContenedor = idContenedor, estadoCriteria = 1 });
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult DocNumEnContenedor(int DocNum)
        {
            try
            {
                ContenedorEnvioManager manager = new ContenedorEnvioManager();
                var contenedor = manager.FindPagedItems(new ContenedorEnvioCriteria { DocNum = DocNum });
                var idEnvio = 0;
                if (contenedor.Length > 0)
                    idEnvio = contenedor[0].Identifier;
                return this.JsonResponse(idEnvio);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult AgregarContenedorEnvio(int idContenedor, int DocNum)
        {
            try
            {
                ContenedorEnvioManager manager = new ContenedorEnvioManager();
                ContenedorEnvio nuevo = new ContenedorEnvio();
                nuevo.Contenedor = new Contenedor() { Identifier = idContenedor };
                nuevo.Envio = new Envio() { Identifier = DocNum };
                nuevo.usuario = User.Identity.GetUserName();
                var result = manager.Add(nuevo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult ActualizarContenedorEnvio(int idEnvio, int Estado, int idContenedor = 0)
        {
            try
            {
                ContenedorEnvioManager manager = new ContenedorEnvioManager();
                ContenedorEnvio nuevo = new ContenedorEnvio();
                nuevo.Identifier = idEnvio;
                nuevo.estado = Estado;
                nuevo.Contenedor = new Contenedor() { Identifier = idContenedor };
                var result = manager.Update(nuevo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult getEnvioPedidos(int DocNum)
        {
            try
            {
                EnvioManager manager = new EnvioManager();
                var result = manager.GetArtículosVentas(DocNum);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult getContenedor(DateTime FecInicio , DateTime FecFin, string NombreCont, int Status=0, int Seguimiento=0)
        {
            try
            {
                ContenedorManager manager = new ContenedorManager();
               var result = manager.FindPagedItems(new ContenedorCriteria() { nomParcial=NombreCont, fecIni=FecInicio, fecFin=FecFin,status=Status,seguimiento_id=Seguimiento});
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult UpdateContenedorStatus(int idContenedor, int Estado)
        {
            try
            {
                ContenedorManager manager = new ContenedorManager();
                var result = manager.Update(new Contenedor() { Identifier = idContenedor, statusContenedor_id = Estado });
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult UpdateContenedorCambio(int idContenedor, decimal TipoCambio)
        {
            try
            {
                ContenedorManager manager = new ContenedorManager();
                var result = manager.Update(new Contenedor() { Identifier = idContenedor, tipoCambio = TipoCambio });
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult getSeguimiento(int idSeguimiento)
        {
            try
            {
                SeguimientoManager manager = new SeguimientoManager();
                var result = manager.Find(idSeguimiento);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult getSeguimientoFecha(DateTime Inicio, DateTime Fin)
        {
            try
            {
                SeguimientoManager manager = new SeguimientoManager();
                var result = manager.FindPagedItems(new SeguimientoCriteria() { fecIni = Inicio, fecFin = Fin});
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult getSeguimientos(DateTime fecPuerini, DateTime fecPuerFin)
        {
            try
            {
                SeguimientoManager manager = new SeguimientoManager();
                var result = manager.FindPagedItems(new SeguimientoCriteria() { fecIni = fecPuerini, fecFin = fecPuerFin});
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult UpdateSeguimiento(int idSeguimiento, DateTime Embarcada, DateTime LlegadaPuerto, DateTime SalidaPuerto, DateTime LlegadaPantaco, DateTime SalidaPantaco, DateTime LibTransito, DateTime LibDespacho)
        {
            try
            {
                SeguimientoManager manager = new SeguimientoManager();
                var result = manager.Update(new Seguimiento() {
                    Identifier = idSeguimiento,
                    embarcada = Embarcada,
                    llegadaPuerto = LlegadaPuerto,
                    salidaPuerto = SalidaPuerto,
                    llegadaPantaco = LlegadaPantaco,
                    salidaPantaco = SalidaPantaco,
                    libTransito = LibTransito,
                    libDespacho = LibDespacho,
                    usuario = User.Identity.GetUserName()});
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult getComentarios(int idSeguimiento, int parentID)
        {
            try
            {
                SeguimientoComentarioManager manager = new SeguimientoComentarioManager();
                var result = manager.FindPagedItems(new SeguimientoComentarioCriteria() { seguimientoId=idSeguimiento, parentID = parentID});
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult getAnexosEnvio(int DocNum, string SKU, int idContenedor)
        {
            try
            {
                if (idContenedor == 0)//Si no es busqueda de anexos de contenedor
                {
                    EnvioManager manager = new EnvioManager();
                    var result = manager.getAnexos(DocNum, SKU);
                    return this.JsonResponse(result);
                }
                else
                {
                    ContenedorAnexoManager manager = new ContenedorAnexoManager();
                    var result = manager.FindPagedItems(new ContenedorAnexoCriteria { contenedorID = idContenedor });
                    return this.JsonResponse(result);
                }
                
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult AgregarAnexo(FormCollection collection)
        {
            ContenedorAnexoManager manager = new ContenedorAnexoManager();
            ContenedorManager contManager = new ContenedorManager();
            //Por cada archivo se agrega la fecha al nombre, para evitar conflictos en la carpeta
            var finFiles = this.Request.Files.Count;
            for (int i = 0; i < finFiles; i++)
            {
                var file = this.Request.Files[i];//Obtenemos el archivo
                ContenedorAnexo nuevo = new ContenedorAnexo();
                var ruta = "\\\\serwebgrupomass\\DocumentosWWW\\Anexos Contenedores";
                nuevo.contenedor_id = int.Parse(collection["idCont"].ToString());
                var contenedor = contManager.Find(nuevo.contenedor_id);
                nuevo.path = ruta;
                ruta += "\\" + contenedor.nomContenedor;
                nuevo.subPath = contenedor.nomContenedor;
                if (!Directory.Exists(Path.GetFullPath(ruta)))//Si no existe creamos la carpeta
                    Directory.CreateDirectory(Path.GetFullPath(ruta));
                nuevo.archivo = Path.GetFileNameWithoutExtension(file.FileName);
                nuevo.ext = Path.GetExtension(file.FileName).Substring(1);
                nuevo.usuario = User.Identity.GetUserName();
                var filename = nuevo.archivo + "." + nuevo.ext;
                var path = Path.Combine(Path.GetFullPath(ruta), filename);
                if (ComprobarExistenciaArchivo(Path.Combine(ruta, filename)))
                {
                    try
                    {
                        file.SaveAs(path);
                        manager.Update(nuevo);
                    }
                    catch (Exception ex)
                    {
                        return this.JsonResponse(null, -1, ex.Message + "\nEl archivo: " + filename + "no se guardo, intentelo más tarde");
                    }
                }
                else
                {
                    try
                    {
                        file.SaveAs(path);
                        manager.Add(nuevo);
                    }
                    catch (Exception ex)
                    {
                        return this.JsonResponse(null, -1, ex.Message + "\nEl archivo: " + filename + "no se guardo, intentelo más tarde");
                    }
                }
                
            }
            return this.JsonResponse(true);

        }

        public bool ComprobarExistenciaArchivo(string fullpath)
        {
            bool resp = false;
            fullpath = Path.GetFullPath(fullpath);
            if (System.IO.File.Exists(fullpath))
                resp = true;
            return resp;
        }

        public JsonResult AgregarComentario(int idSeguimiento, string Mensaje, int parentID)
        {
            try
            {
                SeguimientoComentarioManager manager = new SeguimientoComentarioManager();
                var result = manager.Add(new SeguimientoComentario() { seguimiento_id=idSeguimiento, coment=Mensaje, parentID = parentID, usuario = User.Identity.GetUserName()});
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult findContenedorStatus(int idStatus)
        {
            try
            {
                StatusContenedorManager manager = new StatusContenedorManager();
                var result = manager.Find(idStatus);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult getSKU(string SKU)
        {
            try
            {
                EnvioManager manager = new EnvioManager();
                var result = manager.GetDetalleArticulo(SKU);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public JsonResult getReporteSKUS(int Tipo)
        {
            try
            {
                EnvioManager manager = new EnvioManager();
                var result = manager.ReporteSKUS(Tipo);
                return this.JsonResponse(result);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }

        }

        public FileResult Descargar(string fullpath)
        {
            fullpath = Path.GetFullPath(fullpath);
            var respuesta = File(fullpath, "application/force-download", Path.GetFileName(fullpath));
            return respuesta;
        }

        //Comprueba el archivo con la ruta completa y un tipo para saber si es archivo de contenedor o los de SAP
        public JsonResult ComprobarArchivo(string fullpath)
        {
            try
            {
                bool resp = false;
                fullpath = Path.GetFullPath(fullpath);
                if (System.IO.File.Exists(fullpath))
                    resp = true;
                return this.JsonResponse(resp);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        //Comprueba el archivo que se va a subir
        public JsonResult ComprobarArchivoContenedor(int idContenedor, string filename)
        {
            try
            {
                string resp = "";
                ContenedorManager contManager = new ContenedorManager();
                var ruta = "\\\\serwebgrupomass\\DocumentosWWW\\Anexos Contenedores";
                var contenedor = contManager.Find(idContenedor);
                ruta += "\\" + contenedor.nomContenedor;
                var fullpath = ruta + "\\" + filename;
                fullpath = Path.GetFullPath(fullpath);
                if (System.IO.File.Exists(fullpath))
                    resp = filename;
                return this.JsonResponse(resp);
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        public ActionResult PostReportPartial(DateTime Del, DateTime Al, int Estado, int Tipo)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

            FileInfo newFile = new FileInfo(path + @"DetalleTrafico.xls");

            if (newFile.Exists)
            {
                newFile.Delete();
            }

            ContenedorManager manager = new ContenedorManager();
            var result = manager.GetDetallesExcel(Del, Al, Estado, Tipo);
            if (result.Rows.Count > 0)
            {
                ExcelPackage workbook = new ExcelPackage(newFile);

                ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Detalle");
                objWorksheet.Cells["B2"].Value = Tipo == 2 ? "Impuestos a pagar" : "Modulación 48 a 72 hrs";
                objWorksheet.Cells["B3"].Value = Tipo == 2 ? "" : "Gondola / Carga ferroc 48 a 72 hrs";
                objWorksheet.Cells["B4"].Value = Tipo == 2 ? "" : "Llegada a Pantaco";

                objWorksheet.Cells["E2"].Value = String.Format("{0:dd/MM/yyyy}", DateTime.Now);

                objWorksheet.Cells["B6"].LoadFromDataTable(result, true).AutoFitColumns();

                // Do something to populate your workbook
                workbook.Workbook.Properties.Title = "Reporte de trafico";
                workbook.Workbook.Properties.Author = "Moises Rodriguez";
                workbook.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "2726");

                // Generate a new unique identifier against which the file can be stored
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
                    Data = new { FileGuid = handle, FileName = "DetalleTrafico.xls", Context = true }
                };
            }
            else
            {
                return new JsonResult()
                {
                    Data = new { Context = false }
                };
            }

        }
        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
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
    }
}