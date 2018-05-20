using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CurrencyRate
{
    public class ExchangeRateRepository
    {
        public Dictionary<string, ExchangeRate> DevizoveKurzy;
        private DateTime LastUpdated;

        public List<ExchangeRate> Get()
        {
            var httpClient = new HttpClient();
            var result = httpClient.GetStringAsync("http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt").Result;
            var splitedResult = result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            //if(LastUpdated.AddDays(1).TimeOfDay.Minutes>)
            LastUpdated = DateTime.Now;
            DevizoveKurzy = new Dictionary<string, ExchangeRate>();
            for (var i = 2; i < splitedResult.Length; i++)
            {
                if (splitedResult[i] == String.Empty) continue;

                var singleLineSplited = splitedResult[i].Split("|");

                
                DevizoveKurzy.Add(singleLineSplited[3].Trim(), new ExchangeRate()
                {
                    Kod = singleLineSplited[3].Trim(),
                    Kurz = Decimal.Parse(singleLineSplited[4].Trim()),
                    Mnozstvi = int.Parse(singleLineSplited[2].Trim()),
                    Mena = singleLineSplited[1].Trim(),
                    Zeme = singleLineSplited[0].Trim()
                });
            }
            // if (LastUpdated < DateTime.Now ) GetNewDevizovyKurz();
            return DevizoveKurzy.Values.ToList();
        }
               

        public void Save()
        {
        }

        //USD/EUR = 0.85 -> USD/CZK * CZK/EUR OR CZK/EUR / CZK/USD
       
    }
}