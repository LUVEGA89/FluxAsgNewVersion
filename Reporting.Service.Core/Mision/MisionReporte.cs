using Reporting.Service.Core.Empresa;
using Reporting.Service.Core.Quotation;
using Reporting.Service.Core.Servicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore;

namespace Reporting.Service.Core.Mision
{
    public class MisionReporte : BusinessObject<int>
    {
        public string QuoteXero { get; set; }
        public string InvoiceXero { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Client { get; set; }
        public string FluxReference { get; set; }
        public string ClientRef { get; set; }
        public decimal Costs { get; set; }
        public decimal Taxes { get; set; }
        public decimal TaxesCotizacion { get; set; }
        public decimal Taxes10 { get; set; }
        public decimal Wrapping { get; set;}
        public decimal Wrapping10 { get; set;}
        public int Obc { get; set;}
        public decimal ObcFee { get; set;}
        public decimal ObcCost { get; set; }
        public decimal ExtraBaggage { get; set; }
        public decimal EB10 { get; set; }
        public decimal ExtraCost { get; set; }
        public decimal FinalCost { get; set;}
        public string Schedule { get; set; }
        public decimal PickupCotizacion { get; set; }
        public decimal DeliberyCotizacion { get; set; }
        public decimal HotelCotizacion { get; set; }
        public decimal OtherCotizacion { get; set; }
        public decimal ProfitCotizacion { get; set; }
        public decimal ObcFeeCotizacion { get; set; }
        public decimal FlightsCotizacion { get; set; }
        public decimal PickupMision { get; set; }
        public decimal DeliberyMision { get; set; }
        public decimal HotelMision { get; set; }
        public decimal OtherMision { get; set; }
        public decimal ProfitMision { get; set; }
        public decimal ObcFeeMision { get; set; }
        public decimal FlightsMision { get; set; }
        public string Proveedores { get; set; }
        public string CommentsObc { get; set; }
        public string CommentsPickup { get; set; }
        public string CommentsCustom { get; set; }
        public string CommentsHotel { get; set; }
        public string CommentsDelibery { get; set; }
        public string CommentsOther { get; set; }
    }
}
