using Resporting.Service.Core.Api;
//using Resporting.Service.Core.Cliente;
using Resporting.Service.Core.Proveedor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Resporting.Service.Core.Airport
{
    public class AirportManager : Catalog<Airport, int, AirportCriteria>
    {
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override Airport LoadItem(IDataReader dr)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(Airport item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(Airport item)
        {
            throw new NotImplementedException();
        }
        //public async Task<List<Airport>> GetAirportsAsync(AirportCriteria criteria)
        //{
        //    List<Airport> airports = new List<Airport>();

        //    try
        //    {
        //        HttpClientServiceA shttpclient = new HttpClientServiceA();
        //        var resulta = shttpclient.HttpclientInstance.GetAsync($"{criteria.Name}/0/9/0/0").GetAwaiter().GetResult();
        //        var reta = resulta.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        //        JsonSerializer serializer = new JsonSerializer();

        //        using (StringReader stringReader = new StringReader(reta))
        //        using (JsonReader jsonReader = new JsonTextReader(stringReader))
        //        {
        //            Response response = serializer.Deserialize<Response>(jsonReader);

        //            foreach (var item in response.SearchItems)
        //            {
        //                Airport _airport = new Airport
        //                {
        //                    itemName = item.itemName,
        //                    id = item.id
        //                };

        //                airports.Add(_airport);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Failed to make HTTP request: {ex.Message}");
        //    }

        //    return airports; 
        //}
        public async Task<List<Airport>> GetAirportsAsync(AirportCriteria criteria)
        {
            List<Airport> airports = new List<Airport>();

            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prGetAirportsForQuote");
                this.Database.AddInParameter(cmd, "@Criteria", DbType.String, criteria.Name);
                
                IDataReader dr = this.Database.ExecuteReader(cmd);
                while (dr.Read())
                {
                    Airport _airport = new Airport
                    {
                        itemName = (string)dr["Nombre"],
                        id = (string)dr["Siglas"]
                    };

                    airports.Add(_airport);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get airports: {ex.Message}");
            }

            return airports;
        }
        public async Task UpdateAirportsAsync()
        {
            try
            {
                HttpClientServiceA shttpclient = new HttpClientServiceA();

                char[] numerosYLetras = new char[26];

                // Llenar el array con letras del alfabeto en mayúsculas
                for (int i = 0; i < 26; i++)
                {
                    numerosYLetras[i] = (char)(i + 'A');
                }

                foreach (var criteria in numerosYLetras)
                {
                    var resulta = shttpclient.HttpclientInstance.GetAsync($"{criteria}/0/100000/0/0").GetAwaiter().GetResult();
                    var reta = resulta.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    JsonSerializer serializer = new JsonSerializer();

                    using (StringReader stringReader = new StringReader(reta))
                    using (JsonReader jsonReader = new JsonTextReader(stringReader))
                    {
                        Response response = serializer.Deserialize<Response>(jsonReader);

                        foreach (var item in response.SearchItems)
                        {
                            try
                            {
                                DbCommand cmd = this.Database.GetStoredProcCommand("prAddAirport");
                                this.Database.AddInParameter(cmd, "@Nombre", DbType.String, item.itemName);
                                this.Database.AddInParameter(cmd, "@Siglas", DbType.String, item.id);
                                this.Database.AddInParameter(cmd, "@Pais", DbType.String, item.country);

                                this.Database.ExecuteNonQuery(cmd);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to update airports: {ex.Message}");
            }
        }
    }

    public class HttpClientServiceA
    {
        private static readonly HttpClient _httpClient;
        static HttpClientServiceA()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://www.priceline.com/svcs/ac/index/flights/");
            _httpClient.DefaultRequestHeaders.ConnectionClose = true;
        }
        public HttpClient HttpclientInstance = _httpClient;
    }
}
