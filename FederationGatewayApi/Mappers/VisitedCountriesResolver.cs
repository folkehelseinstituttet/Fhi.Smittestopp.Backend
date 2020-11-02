using AutoMapper;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using FederationGatewayApi.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FederationGatewayApi.Mappers
{
    public class VisitedCountriesResolver : IValueResolver<TemporaryExposureKeyGatewayDto, TemporaryExposureKey, IEnumerable<TemporaryExposureKeyCountry>>
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<VisitedCountriesResolver> _logger;

        public VisitedCountriesResolver(ICountryRepository countryRepository, ILogger<VisitedCountriesResolver> logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }

        public IEnumerable<TemporaryExposureKeyCountry> Resolve(
            TemporaryExposureKeyGatewayDto source,
            TemporaryExposureKey destination,
            IEnumerable<TemporaryExposureKeyCountry> destMember,
            ResolutionContext context)
        {
            if (source.VisitedCountries == null)
            {
                return new List<TemporaryExposureKeyCountry>();
            }

            var visitedDbCountries = _countryRepository.FindByIsoCodes(source.VisitedCountries).ToList();

            // log countries that haven't been found
            if (source.VisitedCountries != null && visitedDbCountries.Count() != source.VisitedCountries.Count)
            {
                var notMappedCountries = source.VisitedCountries
                    .Where(countryCodeFromSource => !visitedDbCountries.Where(country => country.Code.ToLower() == countryCodeFromSource.ToLower()).Any())
                    .ToList();
                var notMappedCountriesStr = string.Join(", ", notMappedCountries);
                _logger.LogError($"Country codes have not been found in the DataBase: {notMappedCountriesStr}");
            }

            var interectionEnties = visitedDbCountries.Select(
                country => new TemporaryExposureKeyCountry()
                {
                    Country = country,
                    CountryId = country.Id,
                    TemporaryExposureKey = destination,
                    TemporaryExposureKeyId = destination.Id
                })
                .ToList();
            return interectionEnties;
        }
    }
}
