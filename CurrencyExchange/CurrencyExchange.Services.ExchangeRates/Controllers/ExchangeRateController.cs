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
        private readonly ILogger<ExchangeRateController> logger;

        public ExchangeRateController(IFixerApiService fixerApiService, ILogger<ExchangeRateController> logger)
        {
            this.fixerApiService = fixerApiService;
            this.logger = logger;
        }

        [HttpGet("GetLatestCurrencyRates")]
        public async Task<ActionResult<BaseCurrencyRate>> GetLatest(string @base, string symbol)
        {
            var result = await fixerApiService.GetLatest(@base.ToUpper(), symbol.ToUpper());
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
            
            if (result == null)
            {
                var message = "GetSymbols endpoint returned null";
                logger.LogInformation(message);
                return BadRequest(message);
            }

            var symbols = result.Symbols?.Keys;
                        
            if (symbols == null || symbols.Count == 0)
            {
                var message = "There is no symbols to process";
                logger.LogInformation(message);
                return BadRequest(message);
            }
                

            var resultForAll = await fixerApiService.GetLatestForAll(symbols);

            return Ok(resultForAll);
        }
    }
}
