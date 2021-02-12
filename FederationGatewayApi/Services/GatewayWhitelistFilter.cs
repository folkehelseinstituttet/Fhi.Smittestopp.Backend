using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Db;
using FederationGatewayApi.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FederationGatewayApi.Services
{
    public class GatewayWhitelistFilter : IGatewayWhitelistFilter
    {
        private readonly ILogger<GatewayWhitelistFilter> _logger;
        private readonly ICountryService _countryService;

        public GatewayWhitelistFilter(
            ILogger<GatewayWhitelistFilter> logger,
            ICountryService countryService)
        {
            _logger = logger;
            _countryService = countryService;
        }

        public async Task FilterKeys([NotNull]IList<TemporaryExposureKey> keys)
        {
            var whitelistHashSet = await _countryService.GetWhitelistHashSet();
            var removedKeysIds = new List<Guid>();
            var keysToBeChecked = new List<TemporaryExposureKey>(keys);

            foreach (TemporaryExposureKey key in keysToBeChecked.Where(key => !whitelistHashSet.Contains(key.Origin.Id)))
            {
                keys.Remove(key);
                removedKeysIds.Add(key.Id);
            }

            var joinedRemovedKeys = string.Join(",", removedKeysIds.ToString());
            _logger.LogInformation(
                $"Filtered key ids due to country whitelist: {joinedRemovedKeys}");
        }
    }
}