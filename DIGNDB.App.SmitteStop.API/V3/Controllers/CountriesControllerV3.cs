using AutoMapper;
using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.V3.Controllers
{
    [ApiController]
    [ApiVersion("3")]
    [Route("api/v{version:apiVersion}/countries")]
    public class CountriesControllerV3 : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountriesControllerV3> _logger;
        private readonly IMapper _mapper;

        public CountriesControllerV3(
            ICountryService countryService,
            ILogger<CountriesControllerV3> logger,
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
        [Produces("application/json")]
        [ProducesResponseType(typeof(CountryCollectionDto), 200)]
        [ServiceFilter(typeof(MobileAuthorizationAttribute))]
        public async Task<IActionResult> GetAllCountries(string countryCode = "EN")
        {
            _logger.LogInformation($"{nameof(GetAllCountries)} endpoint called with countryCode {countryCode}");

            var countries = await _countryService.GetVisibleCountries(countryCode);
            _logger.LogInformation($"{nameof(GetAllCountries)} fetched successfully");
            var countriesDto = _mapper.Map<IEnumerable<Country>, IEnumerable<CountryDto>>(countries);
            var countryCollection = new CountryCollectionDto { CountryCollection = countriesDto };

            return Ok(countryCollection);
        }
    }
}