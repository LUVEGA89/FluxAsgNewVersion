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
    public class CotizacionController : Controller
    {
        [NonAction]
        public JsonResult JsonResponse(object context = null, int code = 0, string message = "")
        {
            return this.Json(new
            {
                Context = context,
                Code = code,
                Message = message
            }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Cobranza/

        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            ClienteViewModel model = new ClienteViewModel();
            ClienteManager ManagerCliente = new ClienteManager();
            ServicioManager servicioManager = new ServicioManager();
            TipoServicioManager tipoServicioManager = new TipoServicioManager();

            model.clientes = ManagerCliente.FindPagedItems(new ClienteFilter()).ToList();
            model.servicios = servicioManager.FindPagedItems(new ServicioCriteria()).ToList();
            model.tipoServicios = tipoServicioManager.FindPagedItems(new TipoServicioCriteria()).ToList();

            DisclaimerManager disclaimerManager = new DisclaimerManager();
            model.Disclaimers = disclaimerManager.FindPagedItems(new DisclaimerCriteria()).ToList();

            return View(model);
        }

        public JsonResult GetQuotations()
        {
            try
            {
                QuotationCriteria _criteria = new QuotationCriteria();

                try
                {
                    UsuarioManager usuarioManager = new UsuarioManager();
                    Usuario _usuario = usuarioManager.GetUsuarioComplete(User.Identity.GetUserId());
                    _criteria.EmpresaId = _usuario.Email.Contains("alejandro.morales") ? 1000 : int.Parse(_usuario.IdEmpresa);
                }
                catch (Exception)
                {
                    return this.JsonResponse(null, -2, "The user is not linked to any company.");
                }

                QuotationManager manager = new QuotationManager();
                var results = manager.FindPagedItems(_criteria);
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult Airports(string criteria)
        {
            try
            {
                AirportManager manager = new AirportManager();
                var results = manager.GetAirportsAsync(new AirportCriteria() { Name = criteria });
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult Disclaimers(int? Servicio)
        {
            try
            {
                DisclaimerManager manager = new DisclaimerManager();
                var results = manager.FindPagedItems(new DisclaimerCriteria { Servicio = Servicio });
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        private Quotation CreateObjectQuote(FormCollection quote)
        {
            Quotation quotation = new Quotation();

            try
            {
                quotation.Client = quote["Client"]?.ToString();
                quotation.ClientRef = quote["ClientRef"]?.ToString();
                quotation.Origin = quote["Origin"]?.ToString();
                quotation.Destination = quote["Destination"]?.ToString();
                quotation.Service = int.Parse(quote["Service"]?.ToString());

                switch (quotation.Service)
                {
                    case 1:
                        DateTime fecha1 = DateTime.Parse(quote["ObcPickupDate"]?.ToString());
                        quotation.ObcPickupDate = fecha1;
                        DateTime fecha2 = DateTime.Parse(quote["ObcDisclaimerEta"]?.ToString());
                        quotation.ObcDisclaimerEta = fecha2;
                        quotation.ObcServiceType = int.Parse(quote["ObcServiceType"]?.ToString());
                        quotation.ObcDelibery = quote["ObcDelibery"]?.ToString();
                        quotation.ObcPickup = decimal.Parse(quote["ObcPickup"]?.ToString());
                        quotation.ObcHotel = quote["ObcHotel"]?.ToString();
                        quotation.ObcCustoms = quote["ObcCustoms"]?.ToString();
                        quotation.ObcOther = quote["ObcOther"]?.ToString();
                        quotation.ObcProfit = quote["ObcProfit"]?.ToString();
                        quotation.ObcSchedule = quote["ObcSchedule"]?.ToString();
                        quotation.ObcSellPrice = decimal.Parse(quote["ObcSellPrice"]?.ToString());

                        int totalConceptos = int.Parse(quote["TotalConceptos"].ToString());
                        quotation.QuotationDetails = new List<QuotationDetails>();
                        for (int i = 0; i < totalConceptos; i++)
                        {
                            quotation.QuotationDetails.Add(new QuotationDetails
                            {
                                Day = quote["Day" + i] == "" ? 0 : int.Parse(quote["Day" + i]?.ToString()),
                                Obc = quote["NumObc" + i] == "" ? 0 : int.Parse(quote["NumObc" + i].ToString()),
                                Flights = quote["Contenedor" + i] == "" ? 0.0m : decimal.Parse(quote["Contenedor" + i].ToString()),
                                ObcFee = quote["ObcFee" + i] == "" ? 0.0m : decimal.Parse(quote["ObcFee" + i].ToString())
                            });
                        }

                        break;
                    case 2:
                        quotation.CharterServiceType = int.Parse(quote["CharterServiceType"]?.ToString());
                        quotation.CharterBuy = quote["CharterBuy"]?.ToString();
                        quotation.CharterAircraft = quote["CharterAircraft"]?.ToString();
                        quotation.CharterPositioning = quote["CharterPositioning"]?.ToString();
                        quotation.CharterRute = quote["CharterRute"]?.ToString();
                        quotation.CharterLiveLeg = quote["CharterLiveLeg"]?.ToString();
                        quotation.CharterCosts = quote["CharterCosts"]?.ToString();

                        break;
                    case 3:
                        quotation.NfoServiceType = int.Parse(quote["NfoServiceType"]?.ToString());
                        quotation.NfoAirline = quote["NfoAirline"]?.ToString();
                        quotation.NfoBuy = quote["NfoBuy"]?.ToString();
                        quotation.NfoOption = quote["NfoOption"]?.ToString();
                        quotation.NfoRute = quote["NfoRute"]?.ToString();
                        quotation.NfoCosts = quote["NfoCosts"]?.ToString();

                        break;
                    case 4:
                        quotation.HostshotBuy = quote["HostshotBuy"]?.ToString();
                        quotation.HostshotOption = quote["HostshotOption"]?.ToString();
                        quotation.HostshotTransitTime = quote["HostshotTransitTime"]?.ToString();
                        quotation.HostshotCosts = quote["HostshotCosts"]?.ToString();

                        break;
                }
            }
            catch (Exception ex)
            {

            }

            return quotation;
        }

        private Quotation CreateObjectUpdateQuote(FormCollection quote)
        {
            Quotation quotation = new Quotation();

            try
            {
                quotation.Identifier = int.Parse(quote["HiddenIdCotizacionE"]?.ToString());
                quotation.Client = quote["ClientE"]?.ToString();
                quotation.ClientRef = quote["ClientRefE"]?.ToString();
                quotation.Origin = quote["OriginE"]?.ToString();
                quotation.Destination = quote["DestinationE"]?.ToString();
                quotation.Service = int.Parse(quote["ServiceE"]?.ToString());

                switch (quotation.Service)
                {
                    case 1:
                        DateTime fecha1 = DateTime.Parse(quote["ObcPickupDateE"]?.ToString());
                        quotation.ObcPickupDate = fecha1;
                        DateTime fecha2 = DateTime.Parse(quote["ObcDisclaimerEtaE"]?.ToString());
                        quotation.ObcDisclaimerEta = fecha2;
                        quotation.ObcServiceType = int.Parse(quote["ObcServiceTypeE"]?.ToString());
                        quotation.ObcDelibery = quote["ObcDeliberyE"]?.ToString();
                        quotation.ObcPickup = decimal.Parse(quote["ObcPickupE"]?.ToString());
                        quotation.ObcHotel = quote["ObcHotelE"]?.ToString();
                        quotation.ObcCustoms = quote["ObcCustomsE"]?.ToString();
                        quotation.ObcOther = quote["ObcOtherE"]?.ToString();
                        quotation.ObcProfit = quote["ObcProfitE"]?.ToString();
                        quotation.ObcSchedule = quote["ObcScheduleE"]?.ToString();
                        quotation.ObcSellPrice = decimal.Parse(quote["ObcSellPriceE"]?.ToString());

                        int totalConceptos = int.Parse(quote["TotalConceptosE"].ToString());
                        quotation.QuotationDetails = new List<QuotationDetails>();
                        for (int i = 0; i < totalConceptos; i++)
                        {
                            quotation.QuotationDetails.Add(new QuotationDetails
                            {
                                Day = quote["DayE" + i] == "" ? 0 : int.Parse(quote["DayE" + i]?.ToString()),
                                Obc = quote["NumObcE" + i] == "" ? 0 : int.Parse(quote["NumObcE" + i].ToString()),
                                Flights = quote["ContenedorE" + i] == "" ? 0.0m : decimal.Parse(quote["ContenedorE" + i].ToString()),
                                ObcFee = quote["ObcFeeE" + i] == "" ? 0.0m : decimal.Parse(quote["ObcFeeE" + i].ToString())
                            });
                        }

                        break;
                    case 2:
                        quotation.CharterServiceType = int.Parse(quote["CharterServiceTypeE"]?.ToString());
                        quotation.CharterBuy = quote["CharterBuyE"]?.ToString();
                        quotation.CharterAircraft = quote["CharterAircraftE"]?.ToString();
                        quotation.CharterPositioning = quote["CharterPositioningE"]?.ToString();
                        quotation.CharterRute = quote["CharterRuteE"]?.ToString();
                        quotation.CharterLiveLeg = quote["CharterLiveLegE"]?.ToString();
                        quotation.CharterCosts = quote["CharterCostsE"]?.ToString();

                        break;
                    case 3:
                        quotation.NfoServiceType = int.Parse(quote["NfoServiceTypeE"]?.ToString());
                        quotation.NfoAirline = quote["NfoAirlineE"]?.ToString();
                        quotation.NfoBuy = quote["NfoBuyE"]?.ToString();
                        quotation.NfoOption = quote["NfoOptionE"]?.ToString();
                        quotation.NfoRute = quote["NfoRuteE"]?.ToString();
                        quotation.NfoCosts = quote["NfoCostsE"]?.ToString();

                        break;
                    case 4:
                        quotation.HostshotBuy = quote["HostshotBuyE"]?.ToString();
                        quotation.HostshotOption = quote["HostshotOptionE"]?.ToString();
                        quotation.HostshotTransitTime = quote["HostshotTransitTimeE"]?.ToString();
                        quotation.HostshotCosts = quote["HostshotCostsE"]?.ToString();

                        break;
                }
            }
            catch (Exception ex)
            {

            }

            return quotation;
        }

        [HttpPost]
        public JsonResult SaveQuotation(FormCollection collection)
        {
            try
            {
                if (!Request.IsAuthenticated)
                    return this.JsonResponse(null, -3, "The session expired.");

                Quotation quotation = this.CreateObjectQuote(collection);
                quotation.CreadaPor = User.Identity.GetUserId();

                var userClaims = User.Identity as ClaimsIdentity;
                if (userClaims != null)
                {
                    var claims = userClaims.Claims;
                    // Ahora, claims contiene todos los claims del usuario autenticado.
                }

                try
                {
                    UsuarioManager usuarioManager = new UsuarioManager();
                    Usuario _usuario = usuarioManager.GetUsuarioComplete(User.Identity.GetUserId());
                    quotation.Empresa = new Reporting.Service.Core.Empresa.Empresa();
                    quotation.Empresa.Identifier = int.Parse(_usuario.IdEmpresa);
                }
                catch (Exception)
                {
                    return this.JsonResponse(null, -2, "The user is not linked to any company.");
                }

                QuotationManager manager = new QuotationManager();
                bool result;
                int idQuotationRegister = 0;
                if (quotation.IsUpdate)
                {
                    result = manager.Update(quotation);
                }
                else
                {
                    result = manager.Add(quotation);

                    //Solo seria para el servicio de OBC
                    // para cualquier otro servicio se regresaria 0 en el id de la cotizacion y en la validacion de la vista
                    //ya no mandaria actualizar el campo de schedule
                    if (result && quotation.Service == 1)
                    {
                        //Obtenemos el id generado para la cotización para que de esta manera se pueda actualizar el schedule
                        idQuotationRegister = quotation.Identifier;//manager.GetLastIdQuotation();

                    }
                }

                return this.JsonResponse(idQuotationRegister, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpPost]
        public JsonResult UpdateQuotation(FormCollection collection)
        {
            try
            {
                if (!Request.IsAuthenticated)
                    return this.JsonResponse(null, -3, "The session expired.");

                Quotation quotation = this.CreateObjectUpdateQuote(collection);
                quotation.CreadaPor = User.Identity.GetUserId();

                var userClaims = User.Identity as ClaimsIdentity;
                if (userClaims != null)
                {
                    var claims = userClaims.Claims;
                    // Ahora, claims contiene todos los claims del usuario autenticado.
                }

                try
                {
                    UsuarioManager usuarioManager = new UsuarioManager();
                    Usuario _usuario = usuarioManager.GetUsuarioComplete(User.Identity.GetUserId());
                    quotation.Empresa = new Reporting.Service.Core.Empresa.Empresa();
                    quotation.Empresa.Identifier = int.Parse(_usuario.IdEmpresa);
                }
                catch (Exception)
                {
                    return this.JsonResponse(null, -2, "The user is not linked to any company.");
                }

                QuotationManager manager = new QuotationManager();
                bool result;
                int idQuotationRegister = 0;

                result = manager.Update(quotation);

                idQuotationRegister = quotation.Identifier;//manager.GetLastIdQuotation();

                return this.JsonResponse(idQuotationRegister, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult Copy(int idCotizacion)
        {
            try
            {
                QuotationManager manager = new QuotationManager();
                var results = manager.Find(idCotizacion);
                return this.JsonResponse(results, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult Aprobar(int id)
        {
            try
            {
                QuotationManager manager = new QuotationManager();
                manager.Approve(id);
                return this.JsonResponse(null, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult Rechazar(int id)
        {
            try
            {
                QuotationManager manager = new QuotationManager();
                manager.Decline(id);
                return this.JsonResponse(null, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        [HttpGet]
        public JsonResult FindQuotation(int id)
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
    }
}
