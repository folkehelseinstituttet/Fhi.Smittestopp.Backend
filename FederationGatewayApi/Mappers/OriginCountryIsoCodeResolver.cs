using AutoMapper;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using FederationGatewayApi.Models;
using System;

namespace FederationGatewayApi.Mappers
{
    public class OriginCountryIsoCodeResolver : IValueResolver<TemporaryExposureKeyGatewayDto, TemporaryExposureKey, Country>
    {
        private readonly ICountryRepository _countryRepository;

        public OriginCountryIsoCodeResolver(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public Country Resolve(TemporaryExposureKeyGatewayDto source, TemporaryExposureKey destination, Country destMember, ResolutionContext context)
        {
            var isoCode = source.Origin;
            var country = _countryRepository.FindByIsoCode(isoCode);
            if (country == null)
            {
                throw new ArgumentException($"Country with code {isoCode} not found!");
            }

            return country;
        }
    }
}
