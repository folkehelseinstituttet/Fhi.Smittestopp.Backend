using AutoMapper;

namespace FederationGatewayApi.Mappers
{
    public class CountryLegacyMapper : Profile
    {
        public CountryLegacyMapper()
        {
            // This code is not going to work after Country model changes. It won't even compile. It should be removed along with all V2 and V1 code after confirmation.

            /*
            CreateMap<Country, CountryLegacyDto>()
                .ForMember(m => m.NameInEnglish,
                    o => o.MapFrom(
                        e => e.EntityTranslations.FirstOrDefault(
                            t => t.LanguageCountry.Code == nameof(CountryLegacyDto.NameInEnglish)).Value))
                .ForMember(m => m.NameInDanish,
                o => o.MapFrom(
                    e => e.EntityTranslations.FirstOrDefault(
                        t => t.EntityPropertyName == nameof(CountryLegacyDto.NameInDanish)).Value));
        */
        }
    }
}