using CurrencyExchange.Services.ExchangeRates.Entities;
using CurrencyExchange.Services.ExchangeRates.Repositories;

namespace CurrencyExchange.Services.ExchangeRates.Caching
{
    public class PeriodicActions : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public PeriodicActions(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken = default)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    ICurrencyRateRepository currencyRateRepository =
                        scope.ServiceProvider.GetRequiredService<ICurrencyRateRepository>();

                    var items = await currencyRateRepository.ListAllAsync();

                    foreach (var item in items)
                    {
                        DateTime dt = DateTime.Now;

                        if (item.CreatedDate < dt.AddMinutes(-30))
                            await currencyRateRepository.DeleteAsync(item);
                    }

                    await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
                }  
                
            }
        }
    
    }
}
