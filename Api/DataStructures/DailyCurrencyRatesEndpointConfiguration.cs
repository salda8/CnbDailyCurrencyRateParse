using System;

namespace CurrencyRate.DataStructures
{
    public class DailyCurrencyRatesEndpointConfiguration
    {
        public Uri Endpoint { get; }

        public DailyCurrencyRatesEndpointConfiguration(string endpoint)
        {
            Endpoint = new Uri(endpoint);
        }
    }
}
