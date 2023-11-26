using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SAPbobsCOM;

namespace Reporting.Service.Core.CompletarFacturacionRecibida.Factory.Tipo
{
    public class ParKoiwa2009 : ICreadorDB
    {
        CuentasGastos.CuentasGastosManager manager;
        public ParKoiwa2009(string Empresa)
        {
            manager = new CuentasGastos.CuentasGastosManager(Empresa);
        }

        Company oCompany = null;
        FactoryBaseKind factoryBaseKind;
        //CuentasGastos.CuentasGastosManager manager = new CuentasGastos.CuentasGastosManager(_empresa);

        public string GetCodigoImpuesto(string CodigoSAT, decimal TasaoCuota, decimal LimiteInferior = 0.0m, decimal LimiteSuperior = 0.0m) => manager.GetCodigoImpuesto(CodigoSAT, TasaoCuota, LimiteInferior, LimiteSuperior);

        public FactoryBaseKind AsignarTipo()
        {
            factoryBaseKind = FactoryBaseKind.ParKoiwa2009;

            return factoryBaseKind;
        }

        public Company CreateOcompany()
        {
            oCompany = new Company
            {
                DbServerType = BoDataServerTypes.dst_MSSQL2012,
                Server = ConfigurationManager.AppSettings["DbServer.ParKoiwa2009"],
                language = BoSuppLangs.ln_Spanish,
                UseTrusted = false,
                DbUserName = ConfigurationManager.AppSettings["DbUser.ParKoiwa2009"],
                DbPassword = ConfigurationManager.AppSettings["DbPassword.ParKoiwa2009"],
                LicenseServer = ConfigurationManager.AppSettings["LicenseServer.ParKoiwa2009"],
                CompanyDB = ConfigurationManager.AppSettings["DbCompany.ParKoiwa2009"],
                UserName = ConfigurationManager.AppSettings["SapUser.ParKoiwa2009"],
                Password = ConfigurationManager.AppSettings["SapPassword.ParKoiwa2009"]
            };

            return oCompany;
        }

        public FacturaCompra ValidaConceptos(FacturaCompra factura)
        {
            try
            {
                if (factura.Hijos != null)
                {
                    if (factura.Hijos.Count > 0)//Si hay hijos se validan y se elimina el concepto padre
                    {
                        List<int> PadresAEliminar = new List<int>();
                        List<Concepto> ConceptosNuevos = new List<Concepto>();

                        foreach (var concepto in factura.Conceptos)
                        {
                            if (concepto.Retenciones != null)
                            {
                                foreach (var item in concepto.Retenciones)
                                {
                                    factura.RetencionesOriginales.Add(item);
                                }
                            }

                            //Se revisa si el padre tiene hijos
                            Hijo[] hijosPorPadre = factura.Hijos.Where(h => h.Padre == concepto.Sequence).ToArray();

                            if (hijosPorPadre != null)
                            {
                                PadresAEliminar.Add((int)concepto.Sequence);

                                //Agregar los traslados y las retencciones al hijo dependiendo del importe del hijo
                                //y de la Tasa o Cuota, igual validar si se van a manejar dolares
                                foreach (var hijo in hijosPorPadre)
                                {
                                    Concepto newconcepto = new Concepto();

                                    newconcepto.Contenedor = hijo.Contenedor;
                                    newconcepto.Cuenta = hijo.Cuenta;
                                    newconcepto.Descripcion = hijo.Descripcion;
                                    newconcepto.Importe = hijo.Importe;

                                    if (concepto.Retenciones != null)
                                    {
                                        if (concepto.Retenciones.Count() > 0)
                                        {
                                            if (concepto.Retenciones[0] != null)
                                            {
                                                newconcepto.Retenciones = new Retenciones[concepto.Retenciones.Count()];

                                                for (int i = 0; concepto.Retenciones.Count() > i; i++)
                                                {
                                                    //para el traslado con tasa o cuota 0 no se hace nada
                                                    if (concepto.Retenciones[i].TasaOCuota == 0)
                                                    {
                                                        Retenciones newRetencion = new Retenciones();

                                                        newRetencion.Base = hijo.Importe;
                                                        newRetencion.Importe = 0.00m;
                                                        newRetencion.Impuesto = concepto.Retenciones[i].Impuesto;
                                                        newRetencion.TasaOCuota = concepto.Retenciones[i].TasaOCuota;
                                                        newRetencion.TipoFactor = concepto.Retenciones[i].TipoFactor;

                                                        newconcepto.Retenciones[i] = newRetencion;
                                                    }
                                                    else
                                                    {
                                                        Retenciones newRetencion = new Retenciones();

                                                        newRetencion.Base = hijo.Importe;
                                                        newRetencion.Importe = Math.Round(hijo.Importe * concepto.Retenciones[i].TasaOCuota, 2);
                                                        newRetencion.Impuesto = concepto.Retenciones[i].Impuesto;
                                                        newRetencion.TasaOCuota = concepto.Retenciones[i].TasaOCuota;
                                                        newRetencion.TipoFactor = concepto.Retenciones[i].TipoFactor;

                                                        newconcepto.Retenciones[i] = newRetencion;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (concepto.Traslados.Count() > 0)
                                    {
                                        if (concepto.Traslados[0] != null)
                                        {
                                            newconcepto.Traslados = new Traslados[concepto.Traslados.Count()];

                                            for (int i = 0; concepto.Traslados.Count() > i; i++)
                                            {
                                                //para el traslado con tasa o cuota 0 no se hace nada
                                                if (concepto.Traslados[i].TasaOCuota == 0)
                                                {
                                                    Traslados newTraslado = new Traslados();

                                                    newTraslado.Base = hijo.Importe;
                                                    newTraslado.Importe = 0.00m;
                                                    newTraslado.Impuesto = concepto.Traslados[i].Impuesto;
                                                    newTraslado.TasaOCuota = concepto.Traslados[i].TasaOCuota;
                                                    newTraslado.TipoFactor = concepto.Traslados[i].TipoFactor;

                                                    newconcepto.Traslados[i] = newTraslado;
                                                }
                                                else
                                                {
                                                    Traslados newTraslado = new Traslados();

                                                    newTraslado.Base = hijo.Importe;
                                                    newTraslado.Importe = Math.Round(hijo.Importe * concepto.Traslados[i].TasaOCuota, 2);
                                                    newTraslado.Impuesto = concepto.Traslados[i].Impuesto;
                                                    newTraslado.TasaOCuota = concepto.Traslados[i].TasaOCuota;
                                                    newTraslado.TipoFactor = concepto.Traslados[i].TipoFactor;

                                                    newconcepto.Traslados[i] = newTraslado;
                                                }
                                            }
                                        }
                                    }

                                    ConceptosNuevos.Add(newconcepto);
                                }
                            }
                        }

                        //Se elimina el Concepto padre 
                        foreach (var item in PadresAEliminar)
                        {
                            factura.Conceptos.RemoveAll(x => x.Sequence == item);
                        }

                        foreach (var item in ConceptosNuevos)
                        {
                            factura.Conceptos.Add(item);
                        }
                    }
                    else//si no hay hijos se valida si la moneda el USD y se hace la transformación
                    {
                        //Se pasan las retenciones para poder meterlas 
                        foreach (var concepto in factura.Conceptos)
                        {
                            if (concepto.Retenciones != null)
                            {
                                foreach (var retencion in concepto.Retenciones)
                                {
                                    factura.RetencionesOriginales.Add(retencion);
                                }
                            }
                        }

                    }
                }
                else//si no hay hijos se valida si la moneda el USD y se hace la transformación
                {
                    //Se pasan las retenciones para poder meterlas 
                    foreach (var concepto in factura.Conceptos)
                    {
                        foreach (var retencion in concepto.Retenciones)
                        {
                            factura.RetencionesOriginales.Add(retencion);
                        }
                    }

                }

                return factura;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public Documents FillPurchaseInvoice(Documents oPurchaseInvoice, FacturaCompra factura, ICreadorDB DbActual, string Referencia)
        {
            //Agregamos el UUID a los campos de usuario
            oPurchaseInvoice.UserFields.Fields.Item("U_UUID").Value = factura.UUID;

            if (factoryBaseKind == FactoryBaseKind.ParKoiwa2009)
            {
                oPurchaseInvoice.UserFields.Fields.Item("U_Contenedor").Value = factura.Conceptos[0].Contenedor ?? "";
                oPurchaseInvoice.UserFields.Fields.Item("U_Proforma").Value = Referencia;
            }

            if (factoryBaseKind == FactoryBaseKind.Steuben2018)
            {
                oPurchaseInvoice.BPL_IDAssignedToInvoice = factura.IdSucursal;
            }
            int BaseLine = 0;
            //int BaseLineRetenciones = 0;
            foreach (var concepto in factura.Conceptos)
            {
                int BaseLineRetenciones = 0;
                string[] cuenta = concepto.Cuenta.Split('/');

                oPurchaseInvoice.Lines.Currency = factura.MonedaOriginal == "USD" ? "USD" : factura.Moneda;

                oPurchaseInvoice.Lines.BaseLine = BaseLine;

                //if (factoryBaseKind == FactoryBaseKind.ParKoiwa2009)
                //{
                oPurchaseInvoice.Lines.AccountCode = cuenta[0];//AcctCode
                                                               //}
                                                               //else
                                                               //{
                                                               //    oPurchaseInvoice.Lines.AccountCode = cuenta[2];//ActId
                                                               //}


                if (concepto.Traslados != null)
                {
                    if (concepto.Traslados.Count() > 0)
                    {
                        oPurchaseInvoice.Lines.TaxCode = DbActual.GetCodigoImpuesto("002", concepto.Traslados[0].TasaOCuota) ?? throw new Exception($"No se encontró el impuesto para 002 y la tasa {concepto.Traslados[0].TasaOCuota}"); //"W3";
                        oPurchaseInvoice.Lines.TaxTotal = factura.MonedaOriginal == "USD" ? (double)concepto.Traslados[0].Importe : (factura.Moneda == "USD" ? ((double)concepto.Traslados[0].Importe / factura.TipoCambio) : (double)concepto.Traslados[0].Importe);
                    }
                    else
                    {
                        oPurchaseInvoice.Lines.TaxCode = "W0";
                        oPurchaseInvoice.Lines.TaxTotal = 0.000000;
                    }
                }
                else
                {
                    oPurchaseInvoice.Lines.TaxCode = "W0";
                    oPurchaseInvoice.Lines.TaxTotal = 0.000000;
                }

                oPurchaseInvoice.Lines.LineTotal = factura.MonedaOriginal == "USD" ? (double)concepto.Importe : factura.Moneda == "USD" ? ((double)concepto.Importe / factura.TipoCambio) : (double)concepto.Importe;
                oPurchaseInvoice.Lines.ItemDescription = concepto.Descripcion.Length >= 100 ? concepto.Descripcion.Substring(0, 99) : concepto.Descripcion;
                oPurchaseInvoice.Lines.WTLiable = concepto.Retenciones?.Length > 0 ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;

                if (factoryBaseKind == FactoryBaseKind.Massriv2007 || factoryBaseKind == FactoryBaseKind.ParKoiwa2009 || factoryBaseKind == FactoryBaseKind.Steuben2018)
                {
                    oPurchaseInvoice.Lines.UserFields.Fields.Item("U_ValidUUID").Value = "VALIDO";
                    if (factoryBaseKind == FactoryBaseKind.Steuben2018)
                    {
                        oPurchaseInvoice.Lines.UserFields.Fields.Item("U_UUId").Value = factura.UUID;
                    }
                    else
                    {
                        oPurchaseInvoice.Lines.UserFields.Fields.Item("U_UUID").Value = factura.UUID;
                        oPurchaseInvoice.Lines.UserFields.Fields.Item("U_Opcionenvio").Value = concepto.Contenedor ?? "";
                    }
                }

                if (factura.Conceptos.Count > 1 && concepto.Retenciones != null)
                {
                    foreach (var retencion in concepto.Retenciones)
                    {
                        if (retencion != null)
                        {
                            oPurchaseInvoice.WithholdingTaxData.SetCurrentLine(BaseLineRetenciones);

                            if (retencion.Impuesto == "001")
                            {
                                string CodigoImpuesto = DbActual.GetCodigoImpuesto("001", retencion.TasaOCuota) ?? throw new Exception($"No se encontró el impuesto para 001 y la tasa {retencion.TasaOCuota}");
                                oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                                oPurchaseInvoice.WithholdingTaxData.WTAmount = factura.MonedaOriginal == "USD" ? (double)retencion.Importe : factura.Moneda == "USD" ? ((double)retencion.Importe / factura.TipoCambio) : (double)retencion.Importe; ;
                                oPurchaseInvoice.WithholdingTaxData.Add();
                                BaseLineRetenciones++;
                            }
                            if (retencion.Impuesto == "002" && retencion.TasaOCuota > 0.0m)
                            {
                                string CodigoImpuesto = DbActual.GetCodigoImpuesto("002", retencion.TasaOCuota) ?? throw new Exception($"No se encontró el impuesto para 002 y la tasa {retencion.TasaOCuota}");
                                oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                                oPurchaseInvoice.WithholdingTaxData.WTAmount = factura.MonedaOriginal == "USD" ? (double)retencion.Importe : factura.Moneda == "USD" ? ((double)retencion.Importe / factura.TipoCambio) : (double)retencion.Importe;
                                oPurchaseInvoice.WithholdingTaxData.Add();
                                BaseLineRetenciones++;
                            }
                        }
                    }
                }

                //foreach (var retencion in concepto.Retenciones)
                //{
                //    if (retencion != null)
                //    {
                //        oPurchaseInvoice.WithholdingTaxData.SetCurrentLine(BaseLineRetenciones);

                //        if (retencion.Impuesto == "001")
                //        {
                //            string CodigoImpuesto = DbActual.GetCodigoImpuesto("001", retencion.TasaOCuota) ?? throw new Exception($"No se encontró el impuesto para 001 y la tasa {retencion.TasaOCuota}");
                //            oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                //            oPurchaseInvoice.WithholdingTaxData.WTAmount = (double)retencion.Importe;
                //            oPurchaseInvoice.WithholdingTaxData.Add();
                //            BaseLineRetenciones++;
                //        }
                //        if (retencion.Impuesto == "002")
                //        {
                //            string CodigoImpuesto = DbActual.GetCodigoImpuesto("002", retencion.TasaOCuota) ?? throw new Exception($"No se encontró el impuesto para 002 y la tasa {retencion.TasaOCuota}");
                //            oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                //            oPurchaseInvoice.WithholdingTaxData.WTAmount = (double)retencion.Importe;
                //            oPurchaseInvoice.WithholdingTaxData.Add();
                //            BaseLineRetenciones++;
                //        }
                //    }
                //}

                oPurchaseInvoice.Lines.Add();

                BaseLine++;
            }

            //Si hay retenciones
            if (factura.RetencionesOriginales.Count > 0)
            {
                if (factura.RetencionesOriginales[0] != null)
                {
                    if (factura.Conceptos.Count == 1)
                    {
                        int BaseLineRetenciones = 0;
                        var Impuesto001 = factura.RetencionesOriginales.Where(h => h.Impuesto == "001" && h.TasaOCuota == 0.100000m).ToArray();
                        var Impuesto002 = factura.RetencionesOriginales.Where(h => h.Impuesto == "002" && h.TasaOCuota == 0.106667m).ToArray();
                        var Impuesto003 = factura.RetencionesOriginales.Where(h => h.Impuesto == "002" && h.TasaOCuota == 0.000000m).ToArray();
                        var Impuesto004 = factura.RetencionesOriginales.Where(h => h.Impuesto == "002" && h.TasaOCuota == 0.040000m).ToArray();

                        if (Impuesto001.Length > 0)
                        {
                            double Total001 = 0.0d;
                            foreach (var item in Impuesto001)
                            {
                                Total001 = Total001 + (double)item.Importe;
                            }

                            string CodigoImpuesto = DbActual.GetCodigoImpuesto("001", 0.100000m) ?? throw new Exception($"No se encontró el impuesto para 001 y la tasa 0.100000");
                            oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                            oPurchaseInvoice.WithholdingTaxData.WTAmount = factura.MonedaOriginal == "USD" ? (double)Total001 : factura.Moneda == "USD" ? ((double)Total001 / factura.TipoCambio) : (double)Total001;
                            oPurchaseInvoice.WithholdingTaxData.Add();
                            BaseLineRetenciones++;
                        }

                        if (Impuesto002.Length > 0)
                        {
                            double Total002 = 0.0d;
                            foreach (var item in Impuesto002)
                            {
                                Total002 = Total002 + (double)item.Importe;
                            }

                            string CodigoImpuesto = DbActual.GetCodigoImpuesto("002", 0.106667m) ?? throw new Exception($"No se encontró el impuesto para 002 y la tasa 0.106667");
                            oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                            oPurchaseInvoice.WithholdingTaxData.WTAmount = factura.MonedaOriginal == "USD" ? (double)Total002 : factura.Moneda == "USD" ? ((double)Total002 / factura.TipoCambio) : (double)Total002;
                            oPurchaseInvoice.WithholdingTaxData.Add();
                            BaseLineRetenciones++;
                        }

                        if (Impuesto003.Length > 0)
                        {
                            //Cuando la retención viene con tasa o cuota 0 no se pasan las retenciones
                            //double Total003 = 0.0d;
                            //foreach (var item in Impuesto003)
                            //{
                            //    Total003 = Total003 + (double)item.Importe;
                            //}

                            //string CodigoImpuesto = DbActual.GetCodigoImpuesto("002", 0.0000000m) ?? throw new Exception($"No se encontró el impuesto para 002 y la tasa 0.000000");
                            //oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                            //oPurchaseInvoice.WithholdingTaxData.WTAmount = factura.Moneda == "USD" ? ((double)Total003 / factura.TipoCambio) : (double)Total003;
                            //oPurchaseInvoice.WithholdingTaxData.Add();
                            //BaseLineRetenciones++;
                        }

                        if (Impuesto004.Length > 0)
                        {
                            double Total004 = 0.0d;
                            foreach (var item in Impuesto004)
                            {
                                Total004 = Total004 + (double)item.Importe;
                            }

                            string CodigoImpuesto = DbActual.GetCodigoImpuesto("002", 0.040000m) ?? throw new Exception($"No se encontró el impuesto para 002 y la tasa 0.040000");
                            oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                            oPurchaseInvoice.WithholdingTaxData.WTAmount = factura.MonedaOriginal == "USD" ? (double)Total004 : factura.Moneda == "USD" ? ((double)Total004 / factura.TipoCambio) : (double)Total004;
                            oPurchaseInvoice.WithholdingTaxData.Add();
                            BaseLineRetenciones++;
                        }
                    }
                }
            }

            return oPurchaseInvoice;
        }
    }
}
