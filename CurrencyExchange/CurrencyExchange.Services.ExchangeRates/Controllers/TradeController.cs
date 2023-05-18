using CurrencyExchange.Services.ExchangeRates.Models;
using CurrencyExchange.Services.ExchangeRates.Services;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.Services.ExchangeRates.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TradeController : Controller
    {
        private readonly ITradeService tradeService;
        private readonly ILogger<TradeController> logger;

        public TradeController(ITradeService tradeService, ILogger<TradeController> logger)
        {
            this.tradeService = tradeService;
            this.logger = logger;
        }

        [HttpPost("MakeTrade")]
        public async Task<IActionResult> MakeTrade([FromBody] Trade trade)
        {
            var rate = await tradeService.RetrieveRate(trade.CurrencyFrom.ToUpper(), trade.CurrencyTo.ToUpper());
            
            if(!rate.Equals(Decimal.Zero))
            {
                var result = await tradeService.MakeTrade(trade, rate);
                return Ok(result);
            }
            else
            {
                var message = "Couldn't retrieve the currency exchange rate";
                logger.LogInformation(message);
                return BadRequest(message);
            }
                
        }
    }
}
