using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRate
{
    public class ExchangeRateSl
    {
        public ExchangeRateSl(ExchangeRateRepository exchangeRateRepository)
        {
            ExchangeRateRepository = exchangeRateRepository;
        }

        public ExchangeRateRepository ExchangeRateRepository { get; }

        public List<ExchangeRate> Get()
        {
            return ExchangeRateRepository.Get();
        }

        public ExchangedResult ExchangeCurrency(string sourceCurrency, string targetCurrency, decimal amount)
        {
            var result = new ExchangedResult();
            result.Mena = targetCurrency;
            result.Castka = ExchangeMoney(sourceCurrency, targetCurrency)*amount;
            return result;
        }

        private decimal ExchangeMoney(string sourceCurrency, string targetCurrency)
        {
            if (ExchangeRateRepository.DevizoveKurzy == null)
            {
                Get();
            }

            var exchchangeRates = ExchangeRateRepository.DevizoveKurzy;

            var foundSourceCurrency = exchchangeRates.TryGetValue(sourceCurrency, out ExchangeRate sourceCurrencyExchange);
            if (!foundSourceCurrency)
            {
                throw new ArgumentNullException($"Invalid source currency {sourceCurrency} not found.");
            }

            if (targetCurrency == "CZK")
            {
                var kurz = sourceCurrencyExchange;
                return (kurz.Kurz / kurz.Mnozstvi);
            }

            var foundTargetCurrency = exchchangeRates.TryGetValue(targetCurrency, out ExchangeRate targetCurrencyExchange);
            if (!foundTargetCurrency)
            {
                throw new ArgumentNullException($"Invalid target currency {targetCurrency} not found.");
            }
            
            //USD
            var targetToCzkRate = (targetCurrencyExchange.Kurz / targetCurrencyExchange.Mnozstvi);
            var sourceToCzkRate = (sourceCurrencyExchange.Kurz / sourceCurrencyExchange.Mnozstvi);

            return (sourceToCzkRate / targetToCzkRate);
        }
    }
}
