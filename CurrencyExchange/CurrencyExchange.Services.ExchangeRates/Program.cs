using AutoMapper;
using CurrencyExchange.ExchangeRates.Persistence;
using CurrencyExchange.Services.ExchangeRates.Caching;
using CurrencyExchange.Services.ExchangeRates.Repositories;
using CurrencyExchange.Services.ExchangeRates.Seed;
using CurrencyExchange.Services.ExchangeRates.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<ICurrencyRateRepository, CurrencyRateRepository>();
builder.Services.AddScoped<ISymbolRepository, SymbolRepository>();
builder.Services.AddScoped<ITradeRepository, TradeRepository>();
builder.Services.AddScoped<ITradeService, TradeService>();
builder.Services.AddHttpClient<IFixerApiService, FixerApiService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiConfigs:FixerApi:Uri"]));
builder.Services.AddDbContext<ExchangeRatesDbContext>(opt
               => opt.UseSqlServer(builder.Configuration.GetConnectionString("ExchangeRatesConnectionString")));
builder.Services.AddHostedService<PeriodicActions>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var fixerApiService = services.GetRequiredService<IFixerApiService>();
        var currencyRepo = services.GetRequiredService<ICurrencyRateRepository>();
        var symbolRepo = services.GetRequiredService<ISymbolRepository>();
        var mapper = services.GetRequiredService<IMapper>();

        await SymbolSeed.SeedAsync(fixerApiService, symbolRepo, mapper);
        var seedCurrenciecService = new CurrencyRatesSeed(fixerApiService, currencyRepo, symbolRepo, mapper);

        await seedCurrenciecService.SeedAsync();
        //Log.Information("Application Starting");
    }
    catch (Exception ex)
    {
        //Log.Warning(ex, "An error occured while starting the application");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
