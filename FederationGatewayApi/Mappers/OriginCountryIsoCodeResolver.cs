using AutoMapper;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using FederationGatewayApi.Models;
using System;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace FederationGatewayApi.Mappers
{
    public class OriginCountryIsoCodeResolver : IValueResolver<TemporaryExposureKeyGatewayDto, TemporaryExposureKey, Country>
    {
        private readonly ICountryRepository countryRepositor;

        public OriginCountryIsoCodeResolver(ICountryRepository _countryRepositor)
        {
            countryRepositor = _countryRepositor;
        }

        public Country Resolve(TemporaryExposureKeyGatewayDto source, TemporaryExposureKey destination, Country destMember, ResolutionContext context)
        {
            var isoCode = source.Origin;
            var country = countryRepositor.FindByIsoCode(source.Origin);
            if (country == null)
            {
                throw new ArgumentException($"Country with code {isoCode} not found!");
            }
            return country;
        }
    }
}
