using AutoMapper;
using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.V3.Controllers
{
    [ApiController]
    [ApiVersion("3")]
    [Route("api/v{version:apiVersion}/covidstatistics")]
    public class CovidStatisticsControllerV3 : ControllerBase
    {
        private readonly IApplicationStatisticsRepository _applicationStatisticsRepository;
        private readonly ISSIStatisticsRepository _ssiStatisticsRepository;
        private readonly ILogger<CovidStatisticsControllerV3> _logger;
        private readonly IMapper _mapper;

        private const string _apiVersion = "3";

        public CovidStatisticsControllerV3(
            ILogger<CovidStatisticsControllerV3> logger,
            IApplicationStatisticsRepository applicationStatisticsRepository,
            ISSIStatisticsRepository ssiStatisticsRepository,
            IMapper mapper)
        {

            _ssiStatisticsRepository = ssiStatisticsRepository;
            _applicationStatisticsRepository = applicationStatisticsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [MapToApiVersion(_apiVersion)]
        [ServiceFilter(typeof(MobileAuthorizationAttribute))]
        public async Task<IActionResult> GetCovidStatistics(string packageDate)
        {
            try
            {
                var applicationStatisticsDb = await _applicationStatisticsRepository.GetNewestEntryAsync();
                if (applicationStatisticsDb == null)
                {
                    throw new InvalidOperationException("No application statistics entries in the database");
                }
                SSIStatistics ssiStatisticsDb;
                if (packageDate != null)
                {

                    bool success = DateTime.TryParse(packageDate, out DateTime lastPackageDate);
                    if (!success)
                    {
                        _logger.LogError("Could not parse package date");
                        return BadRequest("Could not parse package date");
                    }
                    ssiStatisticsDb = await _ssiStatisticsRepository.GetEntryByDateAsync(lastPackageDate);
                }
                else
                {
                    ssiStatisticsDb = await _ssiStatisticsRepository.GetNewestEntryAsync();
                }

                if (ssiStatisticsDb != null)
                {
                    var resultsDbTuple = new Tuple<SSIStatistics, ApplicationStatistics>(ssiStatisticsDb, applicationStatisticsDb);
                    CovidStatisticsDto ssiStatisticsDto =
                        _mapper.Map<Tuple<SSIStatistics, ApplicationStatistics>, CovidStatisticsDto>(resultsDbTuple);
                    return Ok(ssiStatisticsDto);
                }
                return NoContent();
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e.ToString());
                return BadRequest();
            }
            catch (Exception e)
            {
                _logger.LogError($"Unexpected behaviour. Exception message: {e}");
                return StatusCode(500);
            }
        }
    }
}