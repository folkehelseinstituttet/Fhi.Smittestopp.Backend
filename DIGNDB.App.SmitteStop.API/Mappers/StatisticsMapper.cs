using AutoMapper;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using System;

namespace DIGNDB.App.SmitteStop.API.Mappers
{
    public class StatisticsMapper : Profile
    {
        public StatisticsMapper()
        {
            CreateMap<Tuple<CovidStatistics, ApplicationStatistics>, StatisticsDto>()
                .ForMember(x => x.ApplicationStatistics,
                    cfg => cfg.MapFrom(x => x.Item2))
                .ForMember(x => x.CovidStatistics,
                    cfg => cfg.MapFrom(x => x.Item1));
        }
    }
}