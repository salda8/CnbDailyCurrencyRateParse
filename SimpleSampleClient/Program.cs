using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Xml;

namespace SimpleSampleClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:59445");
                var method = new HttpMethod("PATCH");
                var resource = "api/ExchangeRates";

                Console.WriteLine("DOWNLOADING NEW DAILY CURRENCY RATES");
                DownloadNewExchangeRates(method, resource, client);

                Console.WriteLine("\n*************************************");
                Console.WriteLine("GETTING CURRENCY RATES AS XML");
                GetCurrencyRatesAndSaveItAsXml(resource, client);

                Console.WriteLine("\n*************************************");
                Console.WriteLine("CONVERTING CURRENCY RATES");
                ConvertCurrencies(resource, client);

                Console.WriteLine("Press any key to  exit.");
                Console.ReadKey();
            }
        }

        private static void ConvertCurrencies(string resource, HttpClient client)
        {
            FromUsdToEur(resource, client);
            FromUsdToCzk(resource, client);
        }

        private static void FromUsdToCzk(string resource, HttpClient client)
        {
            var sb = new StringBuilder();
            sb.Append(resource);
            sb.Append("?");
            sb.Append("zdrojovaMena=USD&");
            sb.Append("cilovaMena=CZK&");
            sb.Append("mnozstvi=1");
            var response = client.GetAsync(sb.ToString()).Result;
            Console.WriteLine("FROM USD  TO CZK:");
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        }

        private static void FromUsdToEur(string resource, HttpClient client)
        {
            var sb = new StringBuilder();
            sb.Append(resource);
            sb.Append("?");
            sb.Append("zdrojovaMena=USD&");
            sb.Append("cilovaMena=EUR&");
            sb.Append("mnozstvi=1");
            var response = client.GetAsync(sb.ToString()).Result;
            Console.WriteLine("FROM USD  TO EUR:");
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);
           
        }

        private static void DownloadNewExchangeRates(HttpMethod method, string resource, HttpClient client)
        {
            using (var request = new HttpRequestMessage(method, resource))
            {
                var response = client.SendAsync(request).Result;
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
        }

        private static void GetCurrencyRatesAndSaveItAsXml(string resource, HttpClient client)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{resource}/download"))
            {
                request.Headers.Add("ACCEPT", "application/xml");
                var response = client.SendAsync(request).Result;
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(response.Content.ReadAsStringAsync().Result);
                xmlDocument.Save(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "exchangeRates.xml"));
                Console.WriteLine(xmlDocument.OuterXml);
            }
        }
    }
}