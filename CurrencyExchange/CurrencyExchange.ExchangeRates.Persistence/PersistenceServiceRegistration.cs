using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyExchange.ExchangeRates.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
           ConfigurationManager configuration)
        {
            services.AddDbContext<ExchangeRatesDbContext>(opt
                => opt.UseSqlServer(configuration.GetConnectionString("ExchangeRatesConnectionString")));

            return services;
        }
    }
}
