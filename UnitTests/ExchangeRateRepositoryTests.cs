using CurrencyRate;
using Moq;
using System;
using Xunit;

namespace UnitTests
{
    public class ExchangeRateRepositoryTests : IDisposable
    {
        private MockRepository mockRepository;

        public ExchangeRateRepositoryTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void TestGet()
        {
            ExchangeRateRepository exchangeRateRepository = this.CreateExchangeRateRepository();

            var lists = exchangeRateRepository.Get();

            Assert.NotEmpty(lists);
        }

        [Theory]
        [InlineData("EUR", 10, "USD")]
        public void TestCrossRatesCalculation(string target, decimal amount, string source)
        {
            var exchangeRateRepository = this.CreateExchangeRateSl();
            var result = exchangeRateRepository.ExchangeCurrency(source, "CZK", amount);
            var result1 = exchangeRateRepository.ExchangeCurrency(target, "CZK", amount);
            var result3 = exchangeRateRepository.ExchangeCurrency(source, target, amount);
            Assert.True(result.Castka / result1.Castka == result3.Castka / amount);
        }

        private ExchangeRateRepository CreateExchangeRateRepository()
        {
            return new ExchangeRateRepository();
        }

        private ExchangeRateSl CreateExchangeRateSl()
        {
            return new ExchangeRateSl(CreateExchangeRateRepository());
        }
    }
}