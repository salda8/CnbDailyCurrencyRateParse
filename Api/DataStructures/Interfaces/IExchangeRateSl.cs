using System.Collections.Generic;
using CurrencyRate.DataStructures.Dto;

namespace CurrencyRate.DataStructures.Interfaces
{
    public interface IExchangeRateSl
    {
        List<ExchangeRate> DownloadNewExchangeRates();

        ExchangedResult ExchangeCurrency(string sourceCurrency, string targetCurrency, decimal amount);

        List<ExchangeRate> GetExchangeRates();
    }
}