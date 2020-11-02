using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DIGNDB.App.SmitteStop.API.V2.Controllers
{
    [ApiController]
    [ApiVersion("2")]
    [Route("v{version:apiVersion}/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountriesController> _logger;
        private readonly IMapper _mapper;

        public CountriesController(
            ICountryService countryService,
            ILogger<CountriesController> logger,
            IMapper mapper)
        {
            _countryService = countryService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns a list of countries.
        /// This list could be shown to Smitte|Stop user so that he marks which countries he visited.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /countries
        ///
        /// </remarks>
        /// <returns>List of countries.</returns>
        /// <response code="200">Returns list of countries</response>
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CountryCollectionDto), 200)]
        [ServiceFilter(typeof(MobileAuthorizationAttribute))]
        public async Task<IActionResult> GetAllCountries()
        {
            _logger.LogInformation($"{nameof(GetAllCountries)} endpoint called");

            var countries = await _countryService.GetVisibleCountries();
            _logger.LogInformation($"{nameof(GetAllCountries)} fetched successfully");

            var countriesDto = _mapper.Map<IEnumerable<Country>, IEnumerable<CountryDto>>(countries);
            var countryCollection = new CountryCollectionDto {CountryCollection = countriesDto};

            return Ok(countryCollection);
        }
    }
}