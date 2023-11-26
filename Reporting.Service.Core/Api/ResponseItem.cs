using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resporting.Service.Core.Api
{
    public class ResponseItem
    {
        public string type { get; set; }
        public string itemName { get; set; }
        public string id { get; set; }
        public string idWithType { get; set; }
        public string stateCode { get; set; }
        public string cityID { get; set; }
        public string cityCode { get; set; }
        public string cityName { get; set; }
        public string countryCode { get; set; }
        public string country { get; set; }
        public object provinceName { get; set; }
        public string entered { get; set; }
        public string gmtOffset { get; set; }
        public string timeZoneName { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
        public int poiCategoryTypeId { get; set; }
        public string displayName { get; set; }
        public double rank { get; set; }
        public double score { get; set; }
        public double proximity { get; set; }
        public string subType { get; set; }
        public object airportCode { get; set; }
        public int timeZoneID { get; set; }
        public bool fromSavedSearch { get; set; }
    }
}
