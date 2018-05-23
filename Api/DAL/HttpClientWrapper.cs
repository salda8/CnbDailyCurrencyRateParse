using System.Net.Http;
using CurrencyRate.DataStructures;
using CurrencyRate.DataStructures.Interfaces;

namespace CurrencyRate
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        
        private readonly HttpClient httpClient;
        private readonly DailyCurrencyRatesEndpointConfiguration configuration;

        public HttpClientWrapper(HttpClient httpClient, DailyCurrencyRatesEndpointConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        } 

        public string GetDailyCurrencyRate()
        {
            return httpClient.GetStringAsync(configuration.Endpoint).Result;
        }
    }
}