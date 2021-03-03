using AutoMapper;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using System;

namespace DIGNDB.App.SmitteStop.API.Mappers
{
    public class CovidStatisticsMapper : Profile
    {
        public CovidStatisticsMapper()
        {
            CreateMap<Tuple<SSIStatistics, ApplicationStatistics>, CovidStatisticsDto>()
                .ForMember(x => x.ApplicationStatistics,
                    cfg => cfg.MapFrom(x => x.Item2))
                .ForMember(x => x.SSIStatistics,
                    cfg => cfg.MapFrom(x => x.Item1));
        }
    }
}