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
using Reporting.Service.Core.Mision;
using Reporting.Service.Core.ProveedorServicios;
using System.Data;
using System.IO;
using OfficeOpenXml;
using System.Drawing;

namespace Reporting.Service.Web.UI.Controllers
{
    public class MisionController : BaseController
    {
        // GET: Mision
        public ActionResult Index()
        {
            ClienteViewModel model = new ClienteViewModel();

            UsuarioManager Manager = new UsuarioManager();
            ClienteManager ManagerCliente = new ClienteManager();
            ServicioManager servicioManager = new ServicioManager();
            TipoServicioManager tipoServicioManager = new TipoServicioManager();
            ProveedorServicioManager proveedorServicio = new ProveedorServicioManager();

            model.clientes = ManagerCliente.FindPagedItems(new ClienteFilter()).ToList();
            model.servicios = servicioManager.FindPagedItems(new ServicioCriteria()).ToList();
            model.tipoServicios = tipoServicioManager.FindPagedItems(new TipoServicioCriteria()).ToList();
            model.proveedorServicios = proveedorServicio.FindPagedItems(new ProveedorServicioCriteria()).ToList();

            DisclaimerManager disclaimerManager = new DisclaimerManager();
            model.Disclaimers = disclaimerManager.FindPagedItems(new DisclaimerCriteria()).ToList();

            return View(model);
        }
        public ActionResult ReporteGeneral()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpGet]
        public JsonResult GetMissions()
        {
            if (!Request.IsAuthenticated)
                return this.JsonResponse(null, -3, "The session expired.");

            try
            {
                MisionCriteria _criteria = new MisionCriteria();

                try
                {
                    UsuarioManager usuarioManager = new UsuarioManager();
                    Usuario _usuario = usuarioManager.GetUsuarioComplete(User.Identity.GetUserId());
                    //_criteria.EmpresaId = int.Parse(_usuario.IdEmpresa);
                    _criteria.EmpresaId = _usuario.Email.Contains("alejandro.morales") ? 1000 : int.Parse(_usuario.IdEmpresa);
                }
                catch (Exception)
                {
                    return this.JsonResponse(null, -2, "The user is not linked to any company.");
                }

                MisionManager manager = new MisionManager();
                var results = manager.FindPagedItems(_criteria);
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult FindMission(int id)
        {
            try
            {
                QuotationManager manager = new QuotationManager();
                var results = manager.Find(id);
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult FindProvidersMision(int id)
        {
            try
            {
                MisionManager manager = new MisionManager();
                var results = manager.GetProveedoresMision(id);
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult FindProvidersAvailable(int id)
        {
            try
            {
                MisionManager manager = new MisionManager();
                var results = manager.GetProveedoresAvailable(id);
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult AgregarProveedor(int proveedorId, int misionId, int servicioId)
        {
            try
            {
                MisionManager manager = new MisionManager();
                manager.AgregarProveedoresAMision(proveedorId, misionId, servicioId);
                return this.JsonResponse(null, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpGet]
        public JsonResult EliminarProveedor(int proveedorId, int cotizacionId, string servicio)
        {
            try
            {
                MisionManager manager = new MisionManager();
                manager.EliminarProveedoresAMision(proveedorId, cotizacionId, servicio);
                return this.JsonResponse(null, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost]
        public JsonResult Update(Mision mision)
        {
            try
            {
                mision.Empresa = new Core.Empresa.Empresa();
                //mision.Empresa.Identifier = 1;
                mision.CreadaPor = User.Identity.GetUserId();
                MisionManager manager = new MisionManager();
                bool result;
                result = manager.Update(mision);

                return this.JsonResponse(result, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult FindMision(int id)
        {
            try
            {
                MisionManager manager = new MisionManager();
                var results = manager.Find(id);
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpGet]
        public JsonResult Completar(int id)
        {
            try
            {
                MisionManager manager = new MisionManager();
                manager.Complete(id);
                return this.JsonResponse(null, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpGet]
        public JsonResult GetReportMissions(DateTime Del, DateTime Al)
        {
            try
            {
                MisionManager manager = new MisionManager();
                var result = manager.GetReport(Del, Al);
                return this.JsonResponse(result, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
        [HttpPost]
        public JsonResult ReporteCierre(int Mision)
        {
            try
            {
                MisionManager manager = new MisionManager();
                var result = manager.GetReportCierre(Mision);

                DataTable mision = new DataTable();
                mision.Columns.Add("WHATSAPP REF");
                mision.Columns.Add("FEE");
                mision.Columns.Add("EXTRA BAGGAGE");
                mision.Columns.Add("WRAPPING");
                mision.Columns.Add("TAXES");
                mision.Columns.Add("ARRANGED BY");
                mision.Columns.Add("COMMENTS");

                DataRow row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "WHATSAPP REF";
                row1["FEE"] = result[0].FluxReference;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "ARRANGED BY";
                row1["COMMENTS"] = "COMMENTS";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "CUSTOMER:";
                row1["FEE"] = result[0].Client;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "CUSTOMER REF:";
                row1["FEE"] = result[0].ClientRef;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "UPDATE:";
                row1["FEE"] = DateTime.Now;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "CUSTOMER POC:";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "CURRENT STATUS";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "OBCs TOTAL";
                row1["FEE"] = result[0].ObcFee;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = result[0].Wrapping;
                row1["TAXES"] = result[0].Taxes;
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "SCHEDULE:";
                row1["FEE"] = result[0].Schedule;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "PASSPORTS AND TICKETS STATUS:";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "COLLECTION CUT OFF:";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "OBC DETAILS:";
                row1["FEE"] = result[0].CommentsObc;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "PICKUP DETAILS:";
                row1["FEE"] = result[0].CommentsPickup;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "CUSTOM DETAILS:";
                row1["FEE"] = result[0].CommentsCustom;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "HOTEL DETAILS:";
                row1["FEE"] = result[0].CommentsHotel;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "DELIBERY DETAILS:";
                row1["FEE"] = result[0].CommentsDelibery;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "US$0.0";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "OTHER DETAILS:";
                row1["FEE"] = result[0].CommentsOther;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "IMPORT INSTRUCTIONS:";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "BROKER DETAILS:";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "DELIVERY DETAILS:";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "DRIVER DETAILS:";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "US$0.0";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "TO DO:";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "QUOTATION:";
                row1["FEE"] = "QUOTED";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "PAYED";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "PICK UP:";
                row1["FEE"] = "US$" + result[0].PickupCotizacion;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "US$" + result[0].PickupMision; ;
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "DELIVERY:";
                row1["FEE"] = "US$" + result[0].DeliberyCotizacion;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "US$" + result[0].DeliberyMision;
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "OBC FEE:";
                row1["FEE"] = "US$" + result[0].ObcFeeCotizacion;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "US$" + result[0].ObcFeeMision;
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "HOTEL:";
                row1["FEE"] = "US$" + result[0].HotelCotizacion;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "US$" + result[0].HotelMision;
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "FLIGHTS:";
                row1["FEE"] = "US$" + result[0].FlightsCotizacion;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "US$" + result[0].FlightsMision;
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "OTHERS:";
                row1["FEE"] = "US$" + result[0].OtherCotizacion;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "US$" + result[0].OtherMision;
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "EXPORT CLEARANCE:";
                row1["FEE"] = "US$" + result[0].TaxesCotizacion;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "US$" + result[0].Taxes;
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "PROFIT:";
                row1["FEE"] = "US$" + result[0].ProfitCotizacion;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "US$" + result[0].ProfitMision;
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "TOTAL:";
                row1["FEE"] = "US$0.0";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "US$0.0";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "WHATSAPP REF:";
                row1["FEE"] = "0";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "CUSTOMER REF:";
                row1["FEE"] = "0";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "FOR INVOICE:";
                row1["FEE"] = "Excess baggage, wrapping costs and import taxes charged at cost + 10% for outlay";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "SERVICE COST:";
                row1["FEE"] = "US$0.0";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "EXTRA BAGGAGE:";
                row1["FEE"] = "US$" + result[0].ExtraBaggage; ;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "WRAPPING:";
                row1["FEE"] = "US$" + result[0].OtherMision;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "TAXES:";
                row1["FEE"] = "US$" + result[0].Taxes; ;
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "OTHERS:";
                row1["FEE"] = "US$0.0";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "TOTAL:";
                row1["FEE"] = "US$0.0";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "DROPBOX CHECKLIST ";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "POD AND EXTRA EXPENSES IN THE SAME PDF";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "ENTRYs - ONLY IF WE ARRANGED IT";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "TICKETS AND PASSPORTS IN THE SAME PDF";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "CHECK IN - BOARDING PASSES, BAG TAGS, EXTRA BAGGAGE, WRAPPING, ETC.";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                row1 = mision.NewRow();
                row1["WHATSAPP REF"] = "FINAL CHECKLIST";
                row1["FEE"] = "";
                row1["EXTRA BAGGAGE"] = "";
                row1["WRAPPING"] = "";
                row1["TAXES"] = "";
                row1["ARRANGED BY"] = "";
                row1["COMMENTS"] = "";
                mision.Rows.Add(row1);

                string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                FileInfo newFile = new FileInfo(path + @"CierreMision.xlsx");

                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new FileInfo(path + @"\CierreMision.xlsx");
                }

                ExcelPackage workbook = new ExcelPackage(newFile);

                ExcelWorksheet objWorksheet = workbook.Workbook.Worksheets.Add("Cierre de mission");

                objWorksheet.Cells["A1:G1"].Merge = true;
                objWorksheet.Cells["A1:G1"].Value = "CIERRE DE MISION";
                objWorksheet.Cells["A1:G1"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                objWorksheet.Cells["A1:G1"].Style.Fill.BackgroundColor.SetColor(Color.Green);
                objWorksheet.Cells["A1:G1"].Style.Font.Bold = true;
                objWorksheet.Cells["A1:G1"].Style.Font.Color.SetColor(Color.White);
                objWorksheet.Cells["A1:G1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                objWorksheet.Cells["A44:F44"].Merge = true;
                objWorksheet.Cells["A44:F44"].Value = "DROPBOX CHECKLIST";
                objWorksheet.Cells["A44:F44"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;


                //objWorksheet.Cells["A8:A8"].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                objWorksheet.Cells["A8:A8"].Style.Font.Bold = true;

                objWorksheet.Cells["A3"].LoadFromDataTable(mision, false);

                //Se ajusta el texto de la columna correspondiente
                objWorksheet.Column(1).Style.WrapText = true;
                objWorksheet.Column(2).Style.WrapText = true;
                objWorksheet.Column(3).Style.WrapText = true;
                objWorksheet.Column(4).Style.WrapText = true;
                objWorksheet.Column(5).Style.WrapText = true;
                objWorksheet.Column(6).Style.WrapText = true;
                objWorksheet.Column(7).Style.WrapText = true;
                //se establece el tamaño de las columnas
                objWorksheet.Column(1).Width = 30;
                objWorksheet.Column(2).Width = 15;
                objWorksheet.Column(3).Width = 15;
                objWorksheet.Column(4).Width = 15;
                objWorksheet.Column(5).Width = 15;
                objWorksheet.Column(6).Width = 25;
                objWorksheet.Column(7).Width = 45;
                //alinenado los textos verticalmente
                objWorksheet.Column(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(2).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(3).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(4).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(5).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(6).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                objWorksheet.Column(7).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                //alinenado los textos horizontalmente
                objWorksheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                objWorksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                objWorksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                objWorksheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                objWorksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                objWorksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                objWorksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                objWorksheet.Column(1).Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                //titulo de tabla
                //objWorksheet.Cells["A3:G3"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                //objWorksheet.Cells["A3:G3"].Style.Fill.BackgroundColor.SetColor(Color.Gray);
                //objWorksheet.Cells["A3:G3"].Style.Font.Bold = true;
                //objWorksheet.Cells["A3:G3"].Style.Font.Color.SetColor(Color.White);

                workbook.Workbook.Properties.Title = "Reporte cierre de mision";
                workbook.Workbook.Properties.Author = "Luis Vega";
                workbook.Workbook.Properties.SetCustomPropertyValue("Employee", "10");
                string handle = Guid.NewGuid().ToString();

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }

                workbook.SaveAs(newFile);

                return this.JsonResponse(new { FileGuid = handle, FileName = "CierreMision.xlsx" }, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult DownloadCierreMisionXls(string fileGuid, string fileName)
        {
            string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}
