using CurrencyExchange.Services.ExchangeRates.Models;
using CurrencyExchange.Services.ExchangeRates.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Services.ExchangeRates.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IFixerApiService fixerApiService;

        public ExchangeRateController(IFixerApiService fixerApiService)
        {
            this.fixerApiService = fixerApiService;
        }

        [HttpGet("GetLatestCurrencyRates")]
        public async Task<ActionResult<BaseCurrencyRate>> GetLatest( string @base, string symbol)
        {
            var result = await fixerApiService.GetLatest(@base, symbol);
            return Ok(result);
        }

        [HttpGet("GetSymbols")]
        public async Task<ActionResult<SymbolsResponse>> GetSymbols()
        {
            var result = await fixerApiService.GetSymbols();
            return Ok(result);
        }
    }
}
