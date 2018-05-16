using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyRate
{
    public class ExchangeRateRepository
    {
        public List<ExchangeRate> DevizoveKurzy = new List<ExchangeRate>();
        private DateTime LastUpdated;

        public List<ExchangeRate> Get()
        {
            var httpClient = new HttpClient();
            var result = httpClient.GetStringAsync("http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt").Result;
            var splitedResult = result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            LastUpdated = DateTime.Now;

            for (var i = 2; i< splitedResult.Length; i++ )
            {
                if (splitedResult[i] == String.Empty) continue;

                var singleLineSplited = splitedResult[i].Split("|");
                

                DevizoveKurzy.Add(new ExchangeRate()
                {
                    Kod = singleLineSplited[3].Trim(),
                    Kurz = Decimal.Parse(singleLineSplited[4].Trim()),
                    Mnozstvi = int.Parse(singleLineSplited[2].Trim()),
                    Mena = singleLineSplited[1].Trim(),
                    Zeme = singleLineSplited[0].Trim()
                });
             }
           // if (LastUpdated < DateTime.Now ) GetNewDevizovyKurz();
            return DevizoveKurzy;
        }

        public ExchangedResult ChangeCurrency(string sourceCurrency, decimal ammount, string targetCurrency)
        {
            var result = new ExchangedResult();
            result.Mena = targetCurrency;
            result.Castka = ExchangeMoney(sourceCurrency, ammount, targetCurrency);
            return result;
        }

        public void Post()
        {

        }
        //USD/EUR = 0.85 -> USD/CZK * CZK/EUR OR CZK/EUR / CZK/USD
        decimal ExchangeMoney(string sourceCurrency, decimal ammount, string targetCurrency)
        {
                               
            var sourceCurrencyExchange = DevizoveKurzy.SingleOrDefault(x => x.Kod == sourceCurrency);
            if (sourceCurrencyExchange.Equals(default(ExchangeRate)))
            {
                throw new ArgumentNullException("Invalid target currency not found");
            }

            var targetCurrencyExchange = DevizoveKurzy.SingleOrDefault(x => x.Kod == targetCurrency);
            if (targetCurrencyExchange.Equals(default(ExchangeRate)))
            {
                throw new ArgumentNullException("Invalid source currency not found");
            }

            if (targetCurrency == "CZK")
            {
                var kurz = DevizoveKurzy.Single(x => x.Kod == sourceCurrency);
                return (kurz.Kurz / kurz.Mnozstvi) * ammount;
            }   
            //USD
            var targetToCzkRate = (targetCurrencyExchange.Kurz / targetCurrencyExchange.Mnozstvi);
            var sourceToCzkRate = (sourceCurrencyExchange.Kurz / sourceCurrencyExchange.Mnozstvi);

            return ammount * (targetToCzkRate / sourceToCzkRate);

        }
    }
}
