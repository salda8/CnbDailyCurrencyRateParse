using System.Collections.Generic;
using CurrencyRate.DataStructures.Dto;

namespace CurrencyRate.DataStructures.Interfaces
{
    public interface IExchangeRateRepository
    {
        int Count();

        Dictionary<string, ExchangeRate> Get();

        ExchangeRate GetBySymbol(string symbol);

        void Post(Dictionary<string, ExchangeRate> exchangeRates);

        void Save();
    }
}