using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.NotaCredito
{
    public class NotaCredito : BusinessObject<int>
    {
        public decimal Importe { get; set; }
        public string Vendedor { get; set; }
        private List<NotaCreditoItem> _item = new List<NotaCreditoItem>();
        private string _TipoDocumento;
        private int _Valor;

        public List<NotaCreditoItem> Items
        {
            get
            {
                return this._item;
            }
            set
            {
                this._item = value ?? throw new InvalidOperationException("La colección de articulos para la nota de crédito no puede ser un valor nulo.");
            }
        }

        public void AddItem(NotaCreditoItem item)
        {
            this._item.Add(item);
        }


        private List<NotaCreditoImagen> _item1 = new List<NotaCreditoImagen>();
        public List<NotaCreditoImagen> Imagenes
        {
            get
            {
                return this._item1;
            }
            set
            {
                this._item1 = value ?? throw new InvalidOperationException("La colección de imagenes para la nota de crédito no puede ser un valor nulo.");
            }
        }

        public void AddImagen(NotaCreditoImagen item)
        {
            this._item1.Add(item);
        }


        public string Cliente { get; set; }
        public string FolioOrigen { get; set; }
        public string FolioDestino { get; set; }

        public string TipoDocumento
        {
            get
            {
                return _TipoDocumento;
            }
            set
            {
                _TipoDocumento = value;
                if (_TipoDocumento == "S" && _Valor != 1 && Canal != "RETAIL")
                {
                    EstatusValor = "Aprobación pendiente por dirección";
                }

            }
        }

        public string ConceptoDescuentoDetalle { get; set; }

        public string Comentario { get; set; }

        public string Usuario { get; set; }

        public DateTime Fecha { get; set; }

        public int ConceptoDescuento { get; set; }

        public string Concepto { get; set; }

        public string Cuenta { get; set; }

        public string Canal { get; set; }

        public string Email { get; set; }

        public string FolioSap { get; set; }

        public int DocEntry { get; set; }

        public string FolioPagoSAP { get; set; }

        public string CardName { get; set; }

        public string ClienteName { get; set; }

        public int Valor
        {
            get
            {
                return _Valor;
            }
            set
            {

                _Valor = value;
                if (_Valor == 0)
                {
                    EstatusValor = "Aprobación pendiente por la gerencia";
                }                
                if (Valor == 1)
                {
                    if ((Canal == "RETAIL") && _TipoDocumento == "S")
                    {
                        EstatusValor = "Aprobación pendiente por credito y cobranza";
                    }  

                    if(ConceptoDescuento == 39)
                    {
                        EstatusValor = "Aprobación pendiente por credito y cobranza";
                    }
                    if (_TipoDocumento == "I")
                    {

                        EstatusValor = "Aprobación pendiente por la almacén";
                    }


                }
                if (_Valor == 2)
                {
                    EstatusValor = "Aprobación pendiente por dirección";
                }
                if (_Valor == 3)
                {
                    EstatusValor = "Autorizado por dirección";
                }
                if (_Valor == 4)
                {
                    EstatusValor = "SAP";
                }
                if (_Valor == 5)
                {
                    EstatusValor = "Rechazado";
                }
                if (_Valor == 10)
                {
                    EstatusValor = "No especificado";
                }
            }
        }

        public string ComentarioDetalle { get; set; }

        public string EstatusValor
        {
            get; set;
        }

        // Para valores de SAP durante el Timbrado
        public int ConexionSAP { get; set; }
        public string ErrorSAP { get; set; }
        public string DBSAP { get; set; }
        public string Error { get; set; }

        public Core.Anexo.Anexo Anexo { get; set; }

    }
    public partial class Empresa
    {
        public string Database { get; set; }
        public string Name { get; set; }

    }
}
