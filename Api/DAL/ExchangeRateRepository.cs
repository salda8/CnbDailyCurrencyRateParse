using System.Collections.Generic;
using CurrencyRate.DataStructures.Dto;
using CurrencyRate.DataStructures.Interfaces;

namespace CurrencyRate
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {
        private static Dictionary<string, ExchangeRate> exchangeRates = new Dictionary<string, ExchangeRate>();

        public int Count()
        {
            return exchangeRates.Count;
        }

        public Dictionary<string, ExchangeRate> Get()
        {
            return exchangeRates;
        }

        public ExchangeRate GetBySymbol(string symbol)
        {
            return exchangeRates[symbol];
        }

        public void Post(Dictionary<string, ExchangeRate> exchangeRatesPost)
        {
            exchangeRates = exchangeRatesPost;
            
        }

        public void Save()
        {
           
        }
    }
}