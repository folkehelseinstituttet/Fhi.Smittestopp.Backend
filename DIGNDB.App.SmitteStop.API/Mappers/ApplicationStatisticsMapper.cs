using AutoMapper;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.API.Mappers
{
    public class ApplicationStatisticsMapper : Profile
    {
        public ApplicationStatisticsMapper()
        {
            CreateMap<ApplicationStatistics, ApplicationStatisticsDto>();
        }
    }
}