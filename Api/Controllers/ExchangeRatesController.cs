using CurrencyRate.DataStructures.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyRate.Controllers
{
    [Route("api/[controller]")]
    public class ExchangeRatesController : Controller
    {
        private readonly IExchangeRateSl exchangeRate;

        public ExchangeRatesController(IExchangeRateSl exchangeRate)
        {
            this.exchangeRate = exchangeRate;
        }

        // PATCH api/ExchangeRates
        [HttpPatch()]
        public IActionResult DownloadNewExchangeRates()
        {
            exchangeRate.DownloadNewExchangeRates();
            return NoContent();
        }

        // GET api/ExchangeRates/download
        [HttpGet()]
        [Route("download")]
        [ResponseCache(Duration=60)]
        public IActionResult Get()
        {
            return Ok(exchangeRate.GetExchangeRates());
        }

        // GET api/ExchangeRates
        [HttpGet]
        [ResponseCache(Duration=60, VaryByQueryKeys = new string[]{"zdrojovaMena", "ciloveMena", "mnozstvi"})]
        public IActionResult Get([FromQuery(Name = "zdrojovaMena")]string sourceCurrency, [FromQuery(Name = "cilovaMena")]string targetCurrency, [FromQuery(Name = "mnozstvi")]int amount)
        {
            if (string.IsNullOrWhiteSpace(sourceCurrency) || string.IsNullOrWhiteSpace(targetCurrency) || amount == 0)
            {
                return BadRequest();
            }

            return Ok(exchangeRate.ExchangeCurrency(sourceCurrency, targetCurrency, amount));
        }
    }
}