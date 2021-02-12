using AutoMapper;
using DIGNDB.App.SmitteStop.API.Attributes;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.V2.Controllers
{
    [ApiController]
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/countries")]
    public class CountriesControllerLegacy : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountriesControllerLegacy> _logger;
        private readonly IMapper _mapper;

        public CountriesControllerLegacy(
            ICountryService countryService,
            ILogger<CountriesControllerLegacy> logger,
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
        /// <returns>List of countries. Does not work anymore due to change in DB format and won't be fixed at all because it is a legacy code.</returns>
        /// <response code="200">Returns list of countries</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CountryCollectionDto), 200)]
        [ServiceFilter(typeof(MobileAuthorizationAttribute))]
        public async Task<IActionResult> GetAllCountries()
        {
            _logger.LogInformation($"{nameof(GetAllCountries)} endpoint called");

            var countries = await _countryService.GetVisibleCountries();
            _logger.LogInformation($"{nameof(GetAllCountries)} fetched successfully");

            //It will not work anymore since DB definition of translation table is changed.
            var countriesDto = _mapper.Map<IEnumerable<Country>, IEnumerable<CountryLegacyDto>>(countries);
            var countryCollection = new CountryCollectionLegacyDto { CountryCollection = countriesDto };

            return Ok(countryCollection);
        }
    }
}