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
        private readonly ICovidStatisticsRepository _covidStatisticsRepository;
        private readonly ILogger<CovidStatisticsControllerV3> _logger;
        private readonly IMapper _mapper;

        private const string _apiVersion = "3";

        public CovidStatisticsControllerV3(
            ILogger<CovidStatisticsControllerV3> logger,
            IApplicationStatisticsRepository applicationStatisticsRepository,
            ICovidStatisticsRepository covidStatisticsRepository,
            IMapper mapper)
        {

            _covidStatisticsRepository = covidStatisticsRepository;
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

                CovidStatistics covidStatisticsDb;
                if (packageDate != null)
                {
                    var success = DateTime.TryParse(packageDate, out DateTime lastPackageDate);
                    if (!success)
                    {
                        _logger.LogError("Could not parse package date");
                        return BadRequest("Could not parse package date");
                    }
                    covidStatisticsDb = await _covidStatisticsRepository.GetEntryByDateAsync(lastPackageDate);
                }
                else
                {
                    covidStatisticsDb = await _covidStatisticsRepository.GetNewestEntryAsync();
                }

                if (covidStatisticsDb == null)
                {
                    return NoContent();
                }

                var resultsDbTuple = new Tuple<CovidStatistics, ApplicationStatistics>(covidStatisticsDb, applicationStatisticsDb);
                var covidStatisticsDto = _mapper.Map<Tuple<CovidStatistics, ApplicationStatistics>, StatisticsDto>(resultsDbTuple);

                return Ok(covidStatisticsDto);
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