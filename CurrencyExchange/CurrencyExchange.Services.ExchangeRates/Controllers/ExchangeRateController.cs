using CurrencyExchange.Services.ExchangeRates.Models;
using CurrencyExchange.Services.ExchangeRates.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Services.ExchangeRates.Controllers
{
    [ApiController]
    [Route("api/exchangerate")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IFixerApiService fixerApiService;

        public ExchangeRateController(IFixerApiService fixerApiService)
        {
            this.fixerApiService = fixerApiService;
        }

        [HttpGet(Name = "GetLatestCurrencyRates")]
        public async Task<ActionResult<BaseCurrencyRate>> Get( string @base, string symbol)
        {
            var result = await fixerApiService.GetLatest(@base, symbol);
            return Ok(result);

        }
    }
}
