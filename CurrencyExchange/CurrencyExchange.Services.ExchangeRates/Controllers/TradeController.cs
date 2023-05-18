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

        public TradeController(ITradeService tradeService)
        {
            this.tradeService = tradeService;
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
                return BadRequest("Fixer Api issue");
            }
                
        }
    }
}
