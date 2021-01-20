using AutoMapper;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using System.Linq;

namespace FederationGatewayApi.Mappers
{
    public class CountryMapper : Profile
    {
        public CountryMapper()
        {
            CreateMap<Country, CountryDto>()
                .ForMember(m => m.TranslatedName,
                    o => o.MapFrom(
                        e => e.EntityTranslations.Single().Value));
        }
    }
}