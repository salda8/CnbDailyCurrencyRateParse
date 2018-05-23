using CurrencyRate;
using Moq;
using System;
using System.Net.Http;
using CurrencyRate.DataStructures;
using Xunit;

namespace UnitTests
{
    public class ExchangeRateRepositoryTests : IDisposable
    {
        private readonly MockRepository mockRepository;

        public ExchangeRateRepositoryTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        [Theory]
        [InlineData("EUR", 3, "USD")]
        [InlineData("USD", 9, "EUR")]
        [InlineData("RUB", 15, "ISK")]
        public void TestCrossRatesCalculation(string target, decimal amount, string source)
        {
            var exchangeRateRepository = this.CreateExchangeRateSl();
            var result = exchangeRateRepository.ExchangeCurrency(source, "CZK", amount);
            var result1 = exchangeRateRepository.ExchangeCurrency(target, "CZK", amount);
            var result3 = exchangeRateRepository.ExchangeCurrency(source, target, amount);
            Assert.True(Math.Round(result.Castka / result1.Castka,5) == result3.Castka / amount);
        }
        
        private ExchangeRateRepository CreateExchangeRateRepository()
        {
            return new ExchangeRateRepository();
        }

        private ExchangeRateSl CreateExchangeRateSl()
        {
            return new ExchangeRateSl(CreateExchangeRateRepository(), new HttpClientWrapper(new HttpClient(), new DailyCurrencyRatesEndpointConfiguration("http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt")));
        }
    }
}