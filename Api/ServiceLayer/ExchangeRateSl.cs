using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using CurrencyRate.DataStructures.Dto;
using CurrencyRate.DataStructures.Exceptions;
using CurrencyRate.DataStructures.Interfaces;

namespace CurrencyRate
{
    public class ExchangeRateSl : IExchangeRateSl
    {
        private readonly IExchangeRateRepository exchangeRateRepository;
        private readonly IHttpClientWrapper httpClient;

        public ExchangeRateSl(IExchangeRateRepository exchangeRateRepository, IHttpClientWrapper httpClient)
        {
            this.httpClient = httpClient;
            this.exchangeRateRepository = exchangeRateRepository;
        }

        public List<ExchangeRate> DownloadNewExchangeRates()
        {
            string[] splitedResult = httpClient.GetDailyCurrencyRate().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            Dictionary<string, ExchangeRate> exchangeRates = ParseResult(splitedResult);
            exchangeRateRepository.Post(exchangeRates);
            return exchangeRates.Values.ToList();
        }

        public ExchangedResult ExchangeCurrency(string sourceCurrency, string targetCurrency, decimal amount)
        {
            return new ExchangedResult
            {
                Mena = targetCurrency,
                Castka = ExchangeMoney(sourceCurrency, targetCurrency) * amount
            };
        }

        public List<ExchangeRate> GetExchangeRates()
        {
            if (exchangeRateRepository.Count() == 0)
            {
                return DownloadNewExchangeRates();
            }
            return exchangeRateRepository.Get().Values.ToList();
        }

        private static decimal ComputeCrossRate(ExchangeRate foundTargetCurrency, ExchangeRate foundSourceCurrency)
        {
            var targetToCzkRate = foundTargetCurrency.Kurz / foundTargetCurrency.Mnozstvi;
            var sourceToCzkRate = foundSourceCurrency.Kurz / foundSourceCurrency.Mnozstvi;

            return Math.Round(sourceToCzkRate / targetToCzkRate, 5);
        }

        private static Dictionary<string, ExchangeRate> ParseResult(IReadOnlyList<string> splitedResult)
        {
            var exchangeRates = new Dictionary<string, ExchangeRate>();
            for (var i = 2; i < splitedResult.Count; i++)
            {
                if (splitedResult[i] == string.Empty) continue;

                var singleLineSplited = splitedResult[i].Split("|");

                exchangeRates.Add(singleLineSplited[3].Trim(), new ExchangeRate
                (
                   singleLineSplited[3].Trim(),
                   decimal.Parse(singleLineSplited[4].Trim(), new CultureInfo("cs-CZ")),
                   singleLineSplited[1].Trim(),
                   int.Parse(singleLineSplited[2].Trim()),
                   singleLineSplited[0].Trim()

                ));
            }

            return exchangeRates;
        }

        private decimal ExchangeMoney(string sourceCurrency, string targetCurrency)
        {
            if (exchangeRateRepository.Count() == 0)
            {
                DownloadNewExchangeRates();
            }

            var foundSourceCurrency = exchangeRateRepository.GetBySymbol(sourceCurrency);
            if (foundSourceCurrency.Equals(default(ExchangeRate)))
            {
                throw new NotFoundException($"Currency rate for {sourceCurrency} not found.");

            }

            if (targetCurrency == "CZK")
            {
                return foundSourceCurrency.Kurz / foundSourceCurrency.Mnozstvi;
            }

            var foundTargetCurrency = exchangeRateRepository.GetBySymbol(targetCurrency);
            if (foundTargetCurrency.Equals(default(ExchangeRate)))
            {
                throw new NotFoundException($"Currency rate for {targetCurrency} not found.");
            }

            return ComputeCrossRate(foundTargetCurrency, foundSourceCurrency);
        }
    }
}