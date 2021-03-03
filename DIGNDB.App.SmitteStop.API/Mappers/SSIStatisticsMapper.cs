using AutoMapper;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.API.Mappers
{
    public class SSIStatisticsMapper : Profile
    {
        public SSIStatisticsMapper()
        {
            CreateMap<SSIStatistics, SSIStatisticsDto>()
                .ForMember(x => x.Date,
                    opts => opts.MapFrom(x => x.Date));
        }
    }
}