using Reporting.Service.Core.Servicio;
using System;
using System.Collections.Generic;
using WikiCore;

namespace Reporting.Service.Core.Quotation
{
    public class Quotation : BusinessObject<int>
    {
        public Quotation() 
        {
            this.QuotationDetails = new List<QuotationDetails>();
        }

        public bool IsUpdate { get; set; }
        public Quotation Padre { get; set; }
        public int PadreId { get; set; }
        public Empresa.Empresa Empresa { get; set; }
        public string Client { get; set; }
        public int ClienteId { get; set; }
        public string ClientRef { get; set; }
        public string Referencia { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int Service { get; set; }
        public Servicio.Servicio ServiceObject { get; set; }
        public string CreadaPor { get; set; }
        public DateTime RegistradaEl { get; set; }
        public string RegistradaPor { get; set; }
        public QuotationStatus Status { get; set; }

        #region Servicio Obc
        public int ObcServiceType { get; set; }
        public DateTime ObcPickupDate { get; set; }
        public string ObcCosts { get; set; }
        public string ObcSchedule { get; set; }
        public string ObcHotel { get; set; }
        public string ObcDelibery { get; set; }
        public string ObcCustoms { get; set; }
        public string ObcOther { get; set; }
        public string ObcProfit { get; set; }
        public decimal ObcSellPrice { get; set; }
        public decimal ObcPickup { get; set; }
        public DateTime ObcDisclaimerEta { get; set; }
        #endregion

        #region Servicio Charter
        public int CharterServiceType { get; set; }
        public string CharterBuy { get; set; }
        public string CharterAircraft { get; set; }
        public string CharterPositioning { get; set; }
        public string CharterRute { get; set; }
        public string CharterLiveLeg { get; set; }
        public string CharterCosts { get; set; }
        #endregion

        #region Servicio nfo
        public string NfoBuy { get; set; }
        public string NfoOption { get; set; }
        public int NfoServiceType { get; set; }
        public string NfoAirline { get; set; }
        public string NfoRute { get; set; }
        public string NfoCosts { get; set; }
        #endregion

        #region Servicio Hotshot
        public string HostshotBuy { get; set; }
        public string HostshotOption { get; set; }
        public string HostshotTransitTime { get; set; }
        public string HostshotCosts { get; set; }
        #endregion
        public List<QuotationDetails> QuotationDetails { get; set; }
    }
    public class QuotationDetails
    {
        public int Obc { get; set; }
        public int Day { get; set; }
        public decimal ObcFee { get; set; }
        public decimal Flights { get; set; }
    }
}
