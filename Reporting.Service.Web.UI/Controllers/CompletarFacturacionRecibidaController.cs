using Newtonsoft.Json;
using Reporting.Service.Core.CompletarFacturacionRecibida;
using Reporting.Service.Core.CompletarFacturacionRecibida.CuentasGastos;
using Reporting.Service.Core.CompletarFacturacionRecibida.Factory;
using Reporting.Service.Core.CompletarFacturacionRecibida.SucursalesSAP;
using Reporting.Service.Core.FacturacionRecibida;
using Reporting.Service.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Xml;

namespace Reporting.Service.Web.UI.Controllers
{
    public class CompletarFacturacionRecibidaController : JsonController
    {
        // GET: CompletarFacturacionRecibida
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            EmisoresModel model = new EmisoresModel();

            FacturacionRecibidaCatalog Emisores = new FacturacionRecibidaCatalog(FacturacionKind.Recibida);
            SucursalesSAPManager sucursalesSAP = new SucursalesSAPManager();

            model.Emisores = Emisores.FindEmisores(FacturacionKind.Recibida);

            model.SucursalesSAP = sucursalesSAP.FindPagedItems(new SucursalesSAPCriteria());

            return View(model);
        }

        //public JsonResult ObtenerInfoXml(long Sequence)
        //{
        //    try
        //    {
        //        CompletarFacturacionRecibidaManager manager = new CompletarFacturacionRecibidaManager();
        //        dynamic result = new System.Dynamic.ExpandoObject();

        //        var XmlDetalle = manager.GetXmlNotSerializer(Sequence);

        //        if (XmlDetalle.CfdiVersion == "3.3")
        //        {
        //            result = manager.GetXmlv33(XmlDetalle.Xml);
        //        }
        //        if (XmlDetalle.CfdiVersion == "4.0")
        //        {
        //            Core.Sat.v40.SatVersion40.Comprobante oComprobante = manager.GetXmlv40(XmlDetalle.Xml);
        //            oComprobante = manager.CargarImpuestosRetenciones(oComprobante);

        //            result = oComprobante as dynamic;
        //        }
        //        return this.JsonResponse(result, 200, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.JsonResponse(null, -1, ex.Message);
        //    }
        //}
        public JsonResult EnviarASAP(FormCollection collection)
        {
            try
            {
                if (this.Request.Files.Count < 2)
                    throw new Exception("Es necesario por lo menos agregar 2 anexos (.xml y .pdf)");

                CompletarFacturacionRecibidaManager manager = new CompletarFacturacionRecibidaManager();

                FacturaCompra facturaCompra = JsonConvert.DeserializeObject<FacturaCompra>(collection["FacturaCompra"]);

                facturaCompra.Proveedor = manager.ValidateProveedor(facturaCompra.Rfc, facturaCompra.Empresa);

                var _factory = new Factory();
                string Error = null;

                switch (facturaCompra.Empresa)
                {
                    case "GMA020313G59":
                        facturaCompra.TipoDBNomina = FactoryBaseKind.Massriv2007;
                        break;
                    case "SME1006298L7":
                        facturaCompra.TipoDBNomina = FactoryBaseKind.Steuben2018;
                        break;
                    case "PKO090209IV6":
                        facturaCompra.TipoDBNomina = FactoryBaseKind.ParKoiwa2009;
                        break;
                    case "AME1710272J7":
                        facturaCompra.TipoDBNomina = FactoryBaseKind.ANMIL2019;
                        break;
                    case "KAD040518GX1":
                        facturaCompra.TipoDBNomina = FactoryBaseKind.Koiwa;
                        break;
                    case "OME160128QX2":
                        facturaCompra.TipoDBNomina = FactoryBaseKind.OKKU_Operaciones;
                        break;
                    case "PTA180314945":
                        facturaCompra.TipoDBNomina = FactoryBaseKind.PRARE;
                        break;
                    default:
                        throw new Exception($"No se ha implementado el proceso para la empresa {facturaCompra.Empresa}");
                }

                switch (facturaCompra.TipoDBNomina)
                {
                    case FactoryBaseKind.ParKoiwa2009:
                        _factory.DbActual = new Core.CompletarFacturacionRecibida.Factory.Tipo.ParKoiwa2009(facturaCompra.Empresa);
                        Error = _factory.AgregarFactura(facturaCompra, this.Request.Files).Error;
                        break;
                    case FactoryBaseKind.Steuben2018:
                        _factory.DbActual = new Core.CompletarFacturacionRecibida.Factory.Tipo.Steuben2018(facturaCompra.Empresa);
                        Error = _factory.AgregarFactura(facturaCompra, this.Request.Files).Error;
                        break;

                    default:
                        throw new Exception($"No se ha implementado el proceso para la empresa {facturaCompra.Empresa}.");
                }

                if (Error != null)
                    throw new Exception($"Ha ocurrido el siguiente error: {Error}.");

                return this.JsonResponse(facturaCompra, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult ObtenerCuentas(string Empresa)
        {
            try
            {
                CuentasGastosManager manager = new CuentasGastosManager(Empresa);
                var result = manager.FindPagedItems(new CuentasGastosCriteria { Empresa = Empresa });
                return this.JsonResponse(result, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }

        public JsonResult Obtener(DateTime Del, DateTime Al, string RfcEmisor)
        {
            try
            {
                CompletarFacturacionRecibidaManager manager = new CompletarFacturacionRecibidaManager(RfcEmisor);
                CompletarFacturacionRecibida[] result = manager.FindPagedItems(new CompletarFacturacionRecibidaCriteria()
                {
                    Del = Del,
                    Al = Al,
                    RfcReceptor = RfcEmisor,
                    ItemsPerPage = 100000
                });
                return this.JsonResponse(result, 200, "OK");
            }
            catch (Exception ex)
            {
                return this.JsonResponse(null, -1, ex.Message);
            }
        }
    }
}