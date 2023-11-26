using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WikiCore.Data;

namespace Reporting.Service.Core.CompletarFacturacionRecibida
{
    public class CompletarFacturacionRecibidaManager : Catalog<CompletarFacturacionRecibida, long, CompletarFacturacionRecibidaCriteria>
    {
        private string rfc;

        public CompletarFacturacionRecibidaManager(string rfc = "")
        {
            this.rfc = rfc;
        }

        protected override string FindPagedItemsProcedure => "prFindCompletarFacturacionRecibida";

        protected override CompletarFacturacionRecibida LoadItem(IDataReader dr)
        {
            CompletarFacturacionRecibida _fac = new CompletarFacturacionRecibida();

            _fac.Identifier = (long)dr["Sequence"];
            _fac.Uuid = (string)dr["Uuid"];
            _fac.Tipo = (string)dr["Tipo"];
            _fac.RfcReceptor = (string)dr["RfcReceptor"];
            _fac.NombreReceptor = dr["NombreReceptor"] == DBNull.Value ? "" : (string)dr["NombreReceptor"];
            _fac.RfcEmisor = (string)dr["RfcEmisor"];
            _fac.NombreEmisor = (string)dr["NombreEmisor"];
            _fac.UsoCfdi = dr["UsoCfdi"] == DBNull.Value ? "" : (string)dr["UsoCfdi"];
            _fac.MetodoPago = dr["MetodoPago"] == DBNull.Value ? "" : (string)dr["MetodoPago"];
            _fac.FormaPago = dr["FormaPago"] == DBNull.Value ? "" : (string)dr["FormaPago"];
            _fac.Serie = dr["Serie"] == DBNull.Value ? "" : (string)dr["Serie"];
            _fac.Folio = dr["Folio"] == DBNull.Value ? "" : (string)dr["Folio"];

            _fac.Subtotal = (decimal)dr["Subtotal"];
            _fac.Retenciones = (decimal)dr["Retenciones"];
            _fac.Traslados = (decimal)dr["Traslados"];
            _fac.Total = (decimal)dr["Total"];
            _fac.FechaTimbrado = (DateTime)dr["Timbrado"];
            _fac.Estatus = (string)dr["EstatusDocumento"];
            _fac.Sucursal = dr["Sucursal"] == DBNull.Value ? "" : (string)dr["Sucursal"];

            _fac.SapDoc = dr["DocNum"] == DBNull.Value ? 0 : (int)dr["DocNum"];
            _fac.FechaCaptura = dr["DocDate"] == DBNull.Value ? null : (DateTime?)dr["DocDate"];
            _fac.Monto = dr["DocTotal"] == DBNull.Value ? 0.0m : (decimal)dr["DocTotal"];
            _fac.Repetidos = dr["Repetidos"] == DBNull.Value ? "" : (string)dr["Repetidos"];
            _fac.NcRepetidos = dr["NcRepetidos"] == DBNull.Value ? "" : (string)dr["NcRepetidos"];

            return _fac;
        }

        //public Sat.v40.SatVersion40.Comprobante GetXmlv40(string xml)
        //{
        //    XmlDocument document = new XmlDocument();
        //    document.LoadXml(xml);
        //    return Sat.v40.SatVersion40.Satv40Manager.DesgloseXmlFactura(document);
        //}

        //public Sat.v40.SatVersion40.Comprobante CargarImpuestosRetenciones(Sat.v40.SatVersion40.Comprobante oComprobante)
        //{
        //    if (oComprobante.Impuestos != null)
        //    {
        //        if (oComprobante.Impuestos.TotalImpuestosRetenidos == 0m)
        //        {
        //            if (oComprobante.ImpuestosLocales != null)
        //            {
        //                oComprobante.Impuestos.TotalImpuestosRetenidos = oComprobante.ImpuestosLocales.TotaldeRetenciones;

        //                if (oComprobante.Impuestos.Retenciones == null)
        //                {
        //                    int length = oComprobante.ImpuestosLocales.RetencionesLocales.Length;
        //                    oComprobante.Impuestos.Retenciones = new Sat.v40.SatVersion40.ComprobanteImpuestosRetencion[length];

        //                    // retenciones en concepts                             
        //                    int cantiadadConceptos = oComprobante.Conceptos.Length;
        //                    if (cantiadadConceptos == 1)
        //                    {
        //                        if (oComprobante.Conceptos[0].Impuestos != null && oComprobante.Conceptos[0].Impuestos.Retenciones == null)
        //                        {
        //                            oComprobante.Conceptos[0].Impuestos.Retenciones = new Sat.v40.SatVersion40.ComprobanteConceptoImpuestosRetencion[length];
        //                        }
        //                    }
        //                    int contador = 0;

        //                    foreach (var item in oComprobante.ImpuestosLocales.RetencionesLocales)
        //                    {
        //                        Sat.v40.SatVersion40.ComprobanteImpuestosRetencion oRetencion = new Sat.v40.SatVersion40.ComprobanteImpuestosRetencion();
        //                        Sat.v40.SatVersion40.ComprobanteConceptoImpuestosRetencion oRetencionConcepto = new Sat.v40.SatVersion40.ComprobanteConceptoImpuestosRetencion();

        //                        oRetencion.Importe = item.Importe;
        //                        oRetencionConcepto.Importe = item.Importe;
        //                        oRetencionConcepto.Base = item.Importe;
        //                        string tipoImpuesto = string.Empty;
        //                        if (item.ImpLocRetenido == "ISR")
        //                        {
        //                            tipoImpuesto = "001";
        //                        }
        //                        if (item.ImpLocRetenido == "IVA")
        //                        {
        //                            tipoImpuesto = "002";
        //                        }

        //                        oRetencion.Impuesto = tipoImpuesto;
        //                        oRetencionConcepto.TipoFactor = "Tasa";

        //                        oRetencionConcepto.TasaOCuota = item.TasadeRetencion / 100;
        //                        oRetencionConcepto.Impuesto = tipoImpuesto;

        //                        oComprobante.Impuestos.Retenciones[contador] = oRetencion;
        //                        oComprobante.Conceptos[0].Impuestos.Retenciones[contador] = oRetencionConcepto;
        //                        contador++;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return oComprobante;
        //}


        protected override DbCommand PrepareAddStatement(CompletarFacturacionRecibida item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(long id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(long id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCompletarFacturaRecibida");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int64, id);
            this.Database.AddInParameter(cmd, "Empresa", DbType.String, this.rfc);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(CompletarFacturacionRecibida item)
        {
            throw new NotImplementedException();
        }
        protected override DbCommand PrepareFindPagedItemsStatement(CompletarFacturacionRecibidaCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, criteria.Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, criteria.Al);
            this.Database.AddInParameter(cmd, "@RfcReceptor", DbType.String, criteria.RfcReceptor);

            return cmd;
        }


        public XmlDetalle GetXmlNotSerializer(long Sequence)
        {
            XmlDetalle item = null;
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetXmlCompletarFacturaRecibida");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int64, Sequence);

            cmd.CommandTimeout = 0;

            IDataReader dr = this.Database.ExecuteReader(cmd);

            while (dr.Read())
            {
                item = new XmlDetalle();
                item.Xml = (string)dr["Xml"];
                item.CfdiVersion = (string)dr["CfdiVersion"];
            }

            return item;
        }

        public string CfdiVersionXml(XmlDocument document)
        {
            string versionFactura = string.Empty;
            XNamespace cfdi = "http://www.sat.gob.mx/cfd/3";
            XNamespace cfdi4 = "http://www.sat.gob.mx/cfd/4";

            XDocument xdoc = XDocument.Parse(document.InnerXml);

            var XComprobante = xdoc.Element(cfdi.GetName("Comprobante"));
            if (XComprobante == null)
            {
                XComprobante = xdoc.Element(cfdi4.GetName("Comprobante"));
            }
            if (XComprobante != null)
            {
                versionFactura = (string)XComprobante.Attribute("Version").Value;

            }
            return versionFactura;
        }


        public Comprobante GetXmlv33(string elXML)
        {
            Comprobante oComprobante = null;

            XmlSerializer oSerializer = new XmlSerializer(typeof(Comprobante));
            try
            {
                //string a stream
                byte[] byteArray = Encoding.ASCII.GetBytes(elXML);
                MemoryStream stream = new MemoryStream(byteArray);

                using (StreamReader reader = new StreamReader(stream))
                {
                    oComprobante = (Comprobante)oSerializer.Deserialize(reader);

                    //complementos
                    foreach (var oComplemento in oComprobante.Complemento)
                    {
                        foreach (var oComplementoInterior in oComplemento.Any)
                        {
                            if (oComplementoInterior.Name.Contains("TimbreFiscalDigital"))
                            {

                                XmlSerializer oSerializerComplemento = new XmlSerializer(typeof(TimbreFiscalDigital));
                                using (var readerComplemento = new StringReader(oComplementoInterior.OuterXml))
                                {
                                    oComprobante.TimbreFiscalDigital = (TimbreFiscalDigital)oSerializerComplemento.Deserialize(readerComplemento);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return oComprobante;
        }

        public Proveedor ValidateProveedor(string Rfc, string Empresa)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prValidaProveedorFacturacionRecibida");
            this.Database.AddInParameter(cmd, "@Rfc", DbType.String, Rfc);
            this.Database.AddInParameter(cmd, "@Empresa", DbType.String, Empresa);

            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);

            Proveedor proveedor = null;
            while (dr.Read())
            {
                proveedor = new Proveedor
                {
                    CardCode = (string)dr["CardCode"],
                    CardName = (string)dr["CardName"]
                };
            }

            return proveedor ?? throw new Exception("No existe el Proveedor.");
        }

    }
}
