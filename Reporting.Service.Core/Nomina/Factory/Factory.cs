using SAPbobsCOM;
using System;

namespace Reporting.Service.Core.Nomina.Factory
{
    public class Factory 
    {
        public ICreadorDB DbActual { get; set; }

        private Documents oPurchaseInvoice = null;

        public Nomina AgregarFactura(Nomina nomina)
        {
            Nomina _nomina = nomina;

            Company oCompany = DbActual.CreateOcompany();
            FactoryBaseKind factoryBaseKind = DbActual.AsignarTipo();
            string proveedor = "";
            proveedor = DbActual.AsignarProveedor(nomina.Empresa.Nombre);

            string CuentaServicios = DbActual.CuentaServicios;
            string CuentaHonorarios = DbActual.CuentaHonorarios;

            oCompany.Connect();
            string Referencia = "";

            int mierror = 0;

            try
            {
                if (oCompany.Connected)
                {
                    //Creamos el objeto de Invoice(de compra)
                    this.oPurchaseInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices);

                    oPurchaseInvoice.CardCode = proveedor;

                    Referencia = "SIE #" + nomina.Sequence.ToString();
                    this.oPurchaseInvoice.PaymentGroupCode = -1; // Venta de contado...
                    this.oPurchaseInvoice.NumAtCard = Referencia;
                    this.oPurchaseInvoice.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Service;
                    this.oPurchaseInvoice.JournalMemo = "Koiwa administraciones - F - " + nomina.FolioSap.ToString();
                    //this.oPurchaseInvoice.JournalMemo = "Facturación Nomina " + nomina.Sequence.ToString();
                    //Deberá llevar la fecha de facturación (En este caso seria la fecha de hoy ya que este proceso se hace enseguida de que se timbra la factura)
                    this.oPurchaseInvoice.DocDate = DateTime.Now;

                    //Validar si es la factura va a Steuben2018 para agregar el numero de la sucursal
                    if (factoryBaseKind == FactoryBaseKind.Steuben2018 && nomina.Store != null)
                    {
                        //Este campo va cambiar por sucursal y la sucursal se obtendra del tipo de base que estemos usando
                        foreach (var num in nomina.Store.datosSap)
                        {
                            if (num.Tipo == Store.StoreDatosSapKind.Steuben2018)
                            {
                                oPurchaseInvoice.BPL_IDAssignedToInvoice = int.Parse(num.CodigoSucursal);
                            }
                        }
                    }

                    //Si es okku_operaciones que se agregue a la sucursal 1
                    if (factoryBaseKind == FactoryBaseKind.OKKU_Operaciones)
                    {
                        //Este campo va cambiar por sucursal y la sucursal se obtendra del tipo de base que estemos usando
                        oPurchaseInvoice.BPL_IDAssignedToInvoice = 1;
                    }

                   //Agregamos el UUID a los campos de usuario
                    this.oPurchaseInvoice.UserFields.Fields.Item("U_UUID").Value = nomina.UUID;
                    
                    //Agregamos los items
                    //Item donde se almacena el total de la factura
                    oPurchaseInvoice.Lines.BaseLine = 0;
                    oPurchaseInvoice.Lines.AccountCode = CuentaServicios;
                    oPurchaseInvoice.Lines.ItemDescription = "SERVICIOS ADMINISTRATIVOS, NOMINA Y PERIODO #" + nomina.Periodo;
                    oPurchaseInvoice.Lines.LineTotal = (double)nomina.Total;
                    //Para agregar el u_uuid en steuben va en los detalles 
                    if (factoryBaseKind == FactoryBaseKind.Steuben2018)
                    {
                        this.oPurchaseInvoice.Lines.UserFields.Fields.Item("U_UUId").Value = nomina.UUID;
                    }
                    oPurchaseInvoice.Lines.Add();
                    //Item donde se almacena el valor calculado al 4% del total
                    //La medalla es para Frank
                    //MessageBox.Show("VALOR NUEVO"+ item.Sueldoparaporcentaje);
                    oPurchaseInvoice.Lines.BaseLine = 1;
                    oPurchaseInvoice.Lines.AccountCode = CuentaHonorarios;
                    oPurchaseInvoice.Lines.ItemDescription = "HONORARIOS " + (nomina.Tipo != 1 ? nomina.Descripcion : "");
                    oPurchaseInvoice.Lines.LineTotal = (double)(nomina.Sueldoparaporcentaje * nomina.Porcentaje);
                    if (factoryBaseKind == FactoryBaseKind.Steuben2018)
                    {
                        this.oPurchaseInvoice.Lines.UserFields.Fields.Item("U_UUId").Value = nomina.UUID;
                    }
                    oPurchaseInvoice.Lines.Add();

                    //Añadimos la factura a SAP
                    mierror = this.oPurchaseInvoice.Add();

                    //Sí la factura NO se agregó correctamente
                    if (mierror != 0)
                    {
                        int temp_int = mierror;
                        string temp_string = "";
                        oCompany.GetLastError(out temp_int, out temp_string);
                        _nomina.Error = temp_string;
                    }
                    else
                    {
                        _nomina.Error = null;
                    }
                }
                else
                {
                    int temp_int = mierror;
                    string temp_string = "";
                    oCompany.GetLastError(out temp_int, out temp_string);
                    _nomina.Error = temp_string;
                    _nomina.Error = "No fue posible conectarse a la base de datos:" + factoryBaseKind.ToString();
                }
            }
            catch(Exception ex)
            {
                _nomina.Error = ex.Message; 
            }
            finally
            {
                oCompany.Disconnect();
            }
            
            return _nomina;
        }
    }
}
