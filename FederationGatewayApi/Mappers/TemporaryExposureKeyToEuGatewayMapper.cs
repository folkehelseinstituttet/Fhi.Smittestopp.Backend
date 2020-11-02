using AutoMapper;
using FederationGatewayApi.Models;
using System.Collections.Generic;
using System.Linq;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.Domain.Db;
using System;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace FederationGatewayApi.Mappers
{
    public class TemporaryExposureKeyToEuGatewayMapper : Profile
    {
        
        /*
         * Constant to convert second into 10min interval
         * Because rolling start needs to be stored as increments of 10mins
        */
        private const int SecTo10min = 60 * 10;

        private const int UnknownOnsetOffset = 2000;

        public TemporaryExposureKeyToEuGatewayMapper()
        {

            CreateMap<TemporaryExposureKeyCountry, string>().ConvertUsing(d => d.Country.Code);

            CreateMap<TemporaryExposureKey, TemporaryExposureKeyGatewayDto>()
                .ForMember(d => d.KeyData, d => d.MapFrom(source => source.KeyData))
                .ForMember(d => d.RollingStartIntervalNumber, d => d.MapFrom(source => source.RollingStartNumber / SecTo10min))
                .ForMember(d => d.RollingPeriod, d => d.MapFrom(source => source.RollingPeriod))
                .ForMember(d => d.TransmissionRiskLevel, d => d.MapFrom(source => source.TransmissionRiskLevel))
                .ForMember(d => d.VisitedCountries, opt => opt.ConvertUsing(new CountryCodeConverter(), source => source.VisitedCountries))
                .ForMember(d => d.ReportType, d => d.MapFrom(source => Enum.GetName(typeof(ReportType), source.ReportType)))
                .ForMember(d => d.Origin, d => d.MapFrom(source => source.Origin.Code))
                .ForMember(d => d.DaysSinceOnsetOfSymptoms, d => d.MapFrom(source => source.DaysSinceOnsetOfSymptoms.HasValue ? source.DaysSinceOnsetOfSymptoms.Value : UnknownOnsetOffset))
                .ReverseMap()
                    .ForMember(s => s.RollingStartNumber, opt => opt.MapFrom(dest => dest.RollingStartIntervalNumber * SecTo10min))
                    .ForMember(s => s.Origin, opt => opt.MapFrom<OriginCountryIsoCodeResolver>())
                    .ForMember(s => s.DaysSinceOnsetOfSymptoms, opt => opt.MapFrom(dto => MapDaysSinceOnsetOfSymptoms(dto.DaysSinceOnsetOfSymptoms)))
                    .ForMember(s => s.VisitedCountries, opt => opt.MapFrom<VisitedCountriesResolver>())
                    .ForMember(s => s.ReportType, opt => opt.MapFrom(dest => Enum.Parse(typeof(ReportType), dest.ReportType)))
                    .ForMember(s => s.CreatedOn, opt => opt.MapFrom(dest => DateTime.UtcNow));
        }

        private int? MapDaysSinceOnsetOfSymptoms(int value)
        {
            if (value >= KeyValidator.DaysSinceOnsetOfSymptomsValidRangeMin &&
                value <= KeyValidator.DaysSinceOnsetOfSymptomsValidRangeMax)
                return value;

            if (value >= KeyValidator.DaysSinceOnsetOfSymptomsInvalidRangeMin &&
                value <= KeyValidator.DaysSinceOnsetOfSymptomsInvalidRangeMax)
                return value;

            return null;
        }
    }

    public class CountryCodeConverter : IValueConverter<IEnumerable<TemporaryExposureKeyCountry>, IList<string>>
    {
        public IList<string> Convert(IEnumerable<TemporaryExposureKeyCountry> sourceMember, ResolutionContext context)
        {
            return sourceMember.Select(s => s.Country.Code).OrderBy(c => c).ToList();
        }
    }

    
}
