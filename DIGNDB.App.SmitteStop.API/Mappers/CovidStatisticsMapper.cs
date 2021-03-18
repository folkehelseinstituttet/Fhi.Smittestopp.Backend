using AutoMapper;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.API.Mappers
{
    public class CovidStatisticsMapper : Profile
    {
        public CovidStatisticsMapper()
        {
            CreateMap<CovidStatistics, CovidStatisticsDto>()
                .ForMember(x => x.Date,
                    opts => opts.MapFrom(x => x.ModificationDate));
        }
    }
}