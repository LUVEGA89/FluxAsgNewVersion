using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.CompletarFacturacionRecibida.Factory
{
    public class Factory
    {
        public ICreadorDB DbActual { get; set; }

        private Documents oPurchaseInvoice = null;

        private FacturaCompra ValidaConceptos(FacturaCompra factura)
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
                            foreach (var item in concepto.Retenciones)
                            {
                                factura.RetencionesOriginales.Add(item);
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

        public FacturaCompra AgregarFactura(FacturaCompra factura, System.Web.HttpFileCollectionBase ArchivosAnexos)
        {
            factura = DbActual.ValidaConceptos(factura);

            //factura = ValidaConceptos(factura);

            FacturaCompra _factura = factura;

            Company oCompany = DbActual.CreateOcompany();
            FactoryBaseKind factoryBaseKind = DbActual.AsignarTipo();
            string proveedor = "";
            proveedor = factura.Proveedor.CardCode;

            oCompany.Connect();
            var Referencia = "0";

            int mierror = 0;

            try
            {
                if (oCompany.Connected)
                {
                    //Creamos el objeto de Invoice(de compra)
                    this.oPurchaseInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices);
                    this.oPurchaseInvoice.CardCode = proveedor;
                    Referencia = factura.Sequence ?? "0";
                    this.oPurchaseInvoice.PaymentGroupCode = -1; // Venta de contado...
                    this.oPurchaseInvoice.NumAtCard = Referencia.ToString();
                    this.oPurchaseInvoice.DocType = BoDocumentTypes.dDocument_Service;
                    this.oPurchaseInvoice.JournalMemo = factura.Comentarios.Length >= 49 ? factura.Comentarios.Substring(0, 49) : factura.Comentarios; //truncar a 49 caracteres
                    this.oPurchaseInvoice.Comments = factura.Comentarios; //Aqui dejar los comentarios completos
                    this.oPurchaseInvoice.DocDate = DateTime.TryParse(factura.Fecha, out DateTime dateValue) ? dateValue : DateTime.Now;   //new DateTime(2020, 01, 27);//DateTime.Now;
                    this.oPurchaseInvoice.DocCurrency = factura.MonedaOriginal == "USD" ? "USD" : factura.Moneda;
                    if (factura.MonedaOriginal == "USD")//Si la factura viene en dolares no se pasa el tipo de cambio
                    {
                        this.oPurchaseInvoice.DocRate = factura.TipoCambioOriginal;
                    }
                    else
                    {
                        if (factura.Moneda == "USD")
                        {
                            this.oPurchaseInvoice.DocRate = factura.TipoCambio;
                        }
                    }







                    ////Agregamos el UUID a los campos de usuario
                    //this.oPurchaseInvoice.UserFields.Fields.Item("U_UUID").Value = factura.UUID;

                    //if (factoryBaseKind == FactoryBaseKind.ParKoiwa2009)
                    //{
                    //    this.oPurchaseInvoice.UserFields.Fields.Item("U_Contenedor").Value = factura.Conceptos[0].Contenedor ?? "";
                    //    this.oPurchaseInvoice.UserFields.Fields.Item("U_Proforma").Value = Referencia.ToString();
                    //}

                    //if (factoryBaseKind == FactoryBaseKind.Steuben2018)
                    //{
                    //    oPurchaseInvoice.BPL_IDAssignedToInvoice = factura.IdSucursal;
                    //}
                    //int BaseLine = 0;
                    ////int BaseLineRetenciones = 0;
                    //foreach (var concepto in factura.Conceptos)
                    //{
                    //    int BaseLineRetenciones = 0;
                    //    string[] cuenta = concepto.Cuenta.Split('/');

                    //    oPurchaseInvoice.Lines.Currency = factura.MonedaOriginal == "USD" ? "USD" : factura.Moneda;

                    //    oPurchaseInvoice.Lines.BaseLine = BaseLine;

                    //    //if (factoryBaseKind == FactoryBaseKind.ParKoiwa2009)
                    //    //{
                    //    oPurchaseInvoice.Lines.AccountCode = cuenta[0];//AcctCode
                    //    //}
                    //    //else
                    //    //{
                    //    //    oPurchaseInvoice.Lines.AccountCode = cuenta[2];//ActId
                    //    //}


                    //    if (concepto.Traslados != null)
                    //    {
                    //        if (concepto.Traslados.Count() > 0)
                    //        {
                    //            oPurchaseInvoice.Lines.TaxCode = DbActual.GetCodigoImpuesto("002", concepto.Traslados[0].TasaOCuota) ?? throw new Exception($"No se encontró el impuesto para 002 y la tasa {concepto.Traslados[0].TasaOCuota}"); //"W3";
                    //            oPurchaseInvoice.Lines.TaxTotal = factura.MonedaOriginal == "USD" ? (double)concepto.Traslados[0].Importe : (factura.Moneda == "USD" ? ((double)concepto.Traslados[0].Importe / factura.TipoCambio) : (double)concepto.Traslados[0].Importe);
                    //        }
                    //        else
                    //        {
                    //            oPurchaseInvoice.Lines.TaxCode = "W0";
                    //            oPurchaseInvoice.Lines.TaxTotal = 0.000000;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        oPurchaseInvoice.Lines.TaxCode = "W0";
                    //        oPurchaseInvoice.Lines.TaxTotal = 0.000000;
                    //    }

                    //    oPurchaseInvoice.Lines.LineTotal = factura.MonedaOriginal == "USD" ? (double)concepto.Importe : factura.Moneda == "USD" ? ((double)concepto.Importe / factura.TipoCambio) : (double)concepto.Importe;
                    //    oPurchaseInvoice.Lines.ItemDescription = concepto.Descripcion.Length >= 100 ? concepto.Descripcion.Substring(0, 99) : concepto.Descripcion;
                    //    oPurchaseInvoice.Lines.WTLiable = concepto.Retenciones != null ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;

                    //    if (factoryBaseKind == FactoryBaseKind.Massriv2007 || factoryBaseKind == FactoryBaseKind.ParKoiwa2009 || factoryBaseKind == FactoryBaseKind.Steuben2018)
                    //    {
                    //        this.oPurchaseInvoice.Lines.UserFields.Fields.Item("U_ValidUUID").Value = "VALIDO";
                    //        if (factoryBaseKind == FactoryBaseKind.Steuben2018)
                    //        {
                    //            this.oPurchaseInvoice.Lines.UserFields.Fields.Item("U_UUId").Value = factura.UUID;
                    //        }
                    //        else
                    //        {
                    //            this.oPurchaseInvoice.Lines.UserFields.Fields.Item("U_UUID").Value = factura.UUID;
                    //            this.oPurchaseInvoice.Lines.UserFields.Fields.Item("U_Opcionenvio").Value = concepto.Contenedor ?? "";
                    //        }
                    //    }

                    //    if (factura.Conceptos.Count > 1 && concepto.Retenciones != null)
                    //    {
                    //        foreach (var retencion in concepto.Retenciones)
                    //        {
                    //            if (retencion != null)
                    //            {
                    //                oPurchaseInvoice.WithholdingTaxData.SetCurrentLine(BaseLineRetenciones);

                    //                if (retencion.Impuesto == "001")
                    //                {
                    //                    string CodigoImpuesto = DbActual.GetCodigoImpuesto("001", retencion.TasaOCuota) ?? throw new Exception($"No se encontró el impuesto para 001 y la tasa {retencion.TasaOCuota}");
                    //                    oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                    //                    oPurchaseInvoice.WithholdingTaxData.WTAmount = factura.MonedaOriginal == "USD" ? (double)retencion.Importe : factura.Moneda == "USD" ? ((double)retencion.Importe / factura.TipoCambio) : (double)retencion.Importe; ;
                    //                    oPurchaseInvoice.WithholdingTaxData.Add();
                    //                    BaseLineRetenciones++;
                    //                }
                    //                if (retencion.Impuesto == "002" && retencion.TasaOCuota > 0.0m)
                    //                {
                    //                    string CodigoImpuesto = DbActual.GetCodigoImpuesto("002", retencion.TasaOCuota) ?? throw new Exception($"No se encontró el impuesto para 002 y la tasa {retencion.TasaOCuota}");
                    //                    oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                    //                    oPurchaseInvoice.WithholdingTaxData.WTAmount = factura.MonedaOriginal == "USD" ? (double)retencion.Importe : factura.Moneda == "USD" ? ((double)retencion.Importe / factura.TipoCambio) : (double)retencion.Importe;
                    //                    oPurchaseInvoice.WithholdingTaxData.Add();
                    //                    BaseLineRetenciones++;
                    //                }
                    //            }
                    //        }
                    //    }

                    //    //foreach (var retencion in concepto.Retenciones)
                    //    //{
                    //    //    if (retencion != null)
                    //    //    {
                    //    //        oPurchaseInvoice.WithholdingTaxData.SetCurrentLine(BaseLineRetenciones);

                    //    //        if (retencion.Impuesto == "001")
                    //    //        {
                    //    //            string CodigoImpuesto = DbActual.GetCodigoImpuesto("001", retencion.TasaOCuota) ?? throw new Exception($"No se encontró el impuesto para 001 y la tasa {retencion.TasaOCuota}");
                    //    //            oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                    //    //            oPurchaseInvoice.WithholdingTaxData.WTAmount = (double)retencion.Importe;
                    //    //            oPurchaseInvoice.WithholdingTaxData.Add();
                    //    //            BaseLineRetenciones++;
                    //    //        }
                    //    //        if (retencion.Impuesto == "002")
                    //    //        {
                    //    //            string CodigoImpuesto = DbActual.GetCodigoImpuesto("002", retencion.TasaOCuota) ?? throw new Exception($"No se encontró el impuesto para 002 y la tasa {retencion.TasaOCuota}");
                    //    //            oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                    //    //            oPurchaseInvoice.WithholdingTaxData.WTAmount = (double)retencion.Importe;
                    //    //            oPurchaseInvoice.WithholdingTaxData.Add();
                    //    //            BaseLineRetenciones++;
                    //    //        }
                    //    //    }
                    //    //}

                    //    oPurchaseInvoice.Lines.Add();

                    //    BaseLine++;
                    //}

                    ////Si hay retenciones
                    //if (factura.RetencionesOriginales.Count > 0)
                    //{
                    //    if (factura.RetencionesOriginales[0] != null)
                    //    {
                    //        if (factura.Conceptos.Count == 1)
                    //        {
                    //            int BaseLineRetenciones = 0;
                    //            var Impuesto001 = factura.RetencionesOriginales.Where(h => h.Impuesto == "001" && h.TasaOCuota == 0.100000m).ToArray();
                    //            var Impuesto002 = factura.RetencionesOriginales.Where(h => h.Impuesto == "002" && h.TasaOCuota == 0.106667m).ToArray();
                    //            var Impuesto003 = factura.RetencionesOriginales.Where(h => h.Impuesto == "002" && h.TasaOCuota == 0.000000m).ToArray();

                    //            if (Impuesto001.Length > 0)
                    //            {
                    //                double Total001 = 0.0d;
                    //                foreach (var item in Impuesto001)
                    //                {
                    //                    Total001 = Total001 + (double)item.Importe;
                    //                }

                    //                string CodigoImpuesto = DbActual.GetCodigoImpuesto("001", 0.100000m) ?? throw new Exception($"No se encontró el impuesto para 001 y la tasa 0.100000");
                    //                oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                    //                oPurchaseInvoice.WithholdingTaxData.WTAmount = factura.MonedaOriginal == "USD" ? (double)Total001 : factura.Moneda == "USD" ? ((double)Total001 / factura.TipoCambio) : (double)Total001;
                    //                oPurchaseInvoice.WithholdingTaxData.Add();
                    //                BaseLineRetenciones++;
                    //            }

                    //            if (Impuesto002.Length > 0)
                    //            {
                    //                double Total002 = 0.0d;
                    //                foreach (var item in Impuesto002)
                    //                {
                    //                    Total002 = Total002 + (double)item.Importe;
                    //                }

                    //                string CodigoImpuesto = DbActual.GetCodigoImpuesto("002", 0.106667m) ?? throw new Exception($"No se encontró el impuesto para 002 y la tasa 0.106667");
                    //                oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                    //                oPurchaseInvoice.WithholdingTaxData.WTAmount = factura.MonedaOriginal == "USD" ? (double)Total002 : factura.Moneda == "USD" ? ((double)Total002 / factura.TipoCambio) : (double)Total002;
                    //                oPurchaseInvoice.WithholdingTaxData.Add();
                    //                BaseLineRetenciones++;
                    //            }

                    //            if (Impuesto003.Length > 0)
                    //            {
                    //                //Cuando la retención viene con tasa o cuota 0 no se pasan las retenciones
                    //                //double Total003 = 0.0d;
                    //                //foreach (var item in Impuesto003)
                    //                //{
                    //                //    Total003 = Total003 + (double)item.Importe;
                    //                //}

                    //                //string CodigoImpuesto = DbActual.GetCodigoImpuesto("002", 0.0000000m) ?? throw new Exception($"No se encontró el impuesto para 002 y la tasa 0.000000");
                    //                //oPurchaseInvoice.WithholdingTaxData.WTCode = CodigoImpuesto;
                    //                //oPurchaseInvoice.WithholdingTaxData.WTAmount = factura.Moneda == "USD" ? ((double)Total003 / factura.TipoCambio) : (double)Total003;
                    //                //oPurchaseInvoice.WithholdingTaxData.Add();
                    //                //BaseLineRetenciones++;
                    //            }
                    //        }
                    //    }
                    //}









                    this.oPurchaseInvoice = DbActual.FillPurchaseInvoice(oPurchaseInvoice, factura, DbActual, Referencia);

                    //Añadimos la factura a SAP
                    mierror = this.oPurchaseInvoice.Add();

                    //Sí la factura NO se agregó correctamente
                    if (mierror != 0)
                    {
                        int temp_int = mierror;
                        string temp_string = "";
                        oCompany.GetLastError(out temp_int, out temp_string);
                        _factura.Error = temp_string;
                    }
                    else
                    {
                        var folioSAP = oCompany.GetNewObjectKey();

                        //Agregar Anexos
                        int lretcode;
                        int nResult;

                        var DocNum = folioSAP;
                        int contAttachments = 0;
                        Attachments2 oAtt = oCompany.GetBusinessObject(BoObjectTypes.oAttachments2);

                        var ruta = $"{AppDomain.CurrentDomain.BaseDirectory}Anexos";
                        if (!Directory.Exists(Path.GetFullPath(ruta)))
                            Directory.CreateDirectory(Path.GetFullPath(ruta));

                        var finFiles = ArchivosAnexos.Count;
                        for (int i = 0; i < finFiles; i++)
                        {
                            var file = ArchivosAnexos[i];

                            var Nombre = Path.GetFileNameWithoutExtension(file.FileName);
                            var Extencion = Path.GetExtension(file.FileName).Substring(1);
                            var filename = $"{Nombre}.{Extencion}";
                            var path = Path.Combine(Path.GetFullPath(ruta), filename);

                            file.SaveAs(path);

                            oAtt.Lines.FileName = Nombre;
                            oAtt.Lines.FileExtension = Extencion;
                            oAtt.Lines.SourcePath = Path.GetFullPath(ruta); // path;
                            oAtt.Lines.Override = BoYesNoEnum.tYES;
                            oAtt.Lines.Add();
                        }

                        int lretcode1 = oAtt.Add();

                        int SeqAttEntry = -1;
                        string errorCode = string.Empty;

                        if (lretcode1 == 0)
                        {
                            SeqAttEntry = int.Parse(oCompany.GetNewObjectKey());

                            bool ExistCreditNote = oPurchaseInvoice.GetByKey(int.Parse(folioSAP));
                            if (ExistCreditNote)
                            {
                                oPurchaseInvoice.AttachmentEntry = SeqAttEntry;

                                int CodePurchaseInvoice = oPurchaseInvoice.Update();

                                if (CodePurchaseInvoice != 0)
                                {
                                    _factura.Error = $"La factura se agrego correctamente a SAP pero, no fue posible ligar los anexos a esta.";
                                    return _factura;
                                }
                            }
                        }
                        else
                        {
                            //No se pudieron agregar los anexos
                            errorCode = oCompany.GetLastErrorDescription();
                            _factura.Error = $"La factura se agrego correctamente a SAP pero, no fue posible agregar los anexos.";
                            return _factura;
                        }

                        //Agregar Anexos

                        _factura.Error = null;
                    }
                }
                else
                {
                    int temp_int = mierror;
                    string temp_string = "";
                    oCompany.GetLastError(out temp_int, out temp_string);
                    _factura.Error = temp_string;
                    _factura.Error = "No fue posible conectarse a la base de datos:" + factoryBaseKind.ToString();
                }
            }
            catch (Exception ex)
            {
                _factura.Error = ex.Message;
            }
            finally
            {
                oCompany.Disconnect();
            }

            return _factura;
        }
    }
}

