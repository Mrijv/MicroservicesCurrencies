using AutoMapper;
using CurrencyExchange.Services.ExchangeRates.Models;
using CurrencyExchange.Services.ExchangeRates.Repositories;
using CurrencyExchange.Services.ExchangeRates.Services;
using Microsoft.Extensions.Logging;
using Moq;
using RichardSzalay.MockHttp;
using Shouldly;

namespace CurrencyExchange.ExchangeRates.Tests
{
    public class FixerApiServiceTests
    {
        Mock<ICurrencyRateRepository> mockCurrencyRepo =new ();
        Mock<IMapper> mockMapper = new ();
        Mock<ILogger<FixerApiService>> mockLogger = new();
        MockHttpMessageHandler mockHttp = new();

        [Fact]
        public async Task GetLatest_ShouldReturnBaseCurrencyRate_IfApiResponse200()
        {
            //Arange
            var jsonResponse = @"{
                ""base"": ""AED"",
                ""date"": ""2023-05-18"",
                ""rates"": {
                    ""AFN"": 23.855138
                },
                ""success"": true,
                ""timestamp"": 1684412763
            }";

            var @base = "AED";
            var symbols = "ALL";
            var apiUri = "/fixer/latest";
            var queryString = $"base={Uri.EscapeDataString(@base)}&symbols={Uri.EscapeDataString(symbols)}";

            mockHttp.When($"{apiUri}?{queryString}")
                 .Respond("application/json", jsonResponse);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://api.apilayer.com");

            var fixerApiService = new FixerApiService(client,
                mockCurrencyRepo.Object, mockMapper.Object, mockLogger.Object);
            //Act
            var result = await fixerApiService.GetLatest(@base, symbols);
            //Assert
            result.ShouldBeOfType<BaseCurrencyRate>();
        }

        [Fact]
        public async Task GetLatest_GetAsync_ThrowsExceptionOnErrorResponse()
        {
            //Arange
            var errorMessage = "An error occurred.";
            var @base = "AED";
            var symbols = "ALL";
            var apiUri = "/fixer/latest";
            var queryString = $"base={Uri.EscapeDataString(@base)}&symbols={Uri.EscapeDataString(symbols)}";

            mockHttp.When($"{apiUri}?{queryString}")
                .Throw(new Exception(errorMessage));

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("https://api.apilayer.com");

            var fixerApiService = new FixerApiService(client,
                mockCurrencyRepo.Object, mockMapper.Object, mockLogger.Object);
            //Act
            var result = await fixerApiService.GetLatest(@base, symbols);
            //Assert
            result.ShouldBeNull();
        }
    }
}
