using AutoMapper;

namespace CurrencyExchange.Services.ExchangeRates.Profiles
{
    public class TradeProfile : Profile
    {
        public TradeProfile()
        {
            CreateMap<Entities.Trade, Models.Trade>().ReverseMap()
                .ForMember(dist=>dist.AmountFrom, act=>act.MapFrom(src=>src.Amount));
        }
    }
}
