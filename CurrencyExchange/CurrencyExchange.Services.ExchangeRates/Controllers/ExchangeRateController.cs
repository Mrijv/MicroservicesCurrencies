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
        public async Task<ActionResult<BaseCurrencyRate>> GetLatest(string @base, string symbol)
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

        [HttpGet("GetLatestForAll")]
        public async Task<ActionResult<Dictionary<string, BaseCurrencyRate>>> GetLatestForAll()
        {
            var result = await fixerApiService.GetSymbols();

            //log
            if (result == null)
                return Ok("GetSymbols endpoint returned null");        

            var symbols = result.Symbols?.Keys;

            //log
            if (symbols == null || symbols.Count == 0)
                return Ok("There is no symbols to process");

            var resultForAll = await fixerApiService.GetLatestForAll(symbols);

            return Ok(resultForAll);
        }
    }
}
