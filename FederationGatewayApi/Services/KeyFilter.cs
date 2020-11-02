using AutoMapper;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Enums;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Core.Helpers;

namespace FederationGatewayApi.Services
{
    public class KeyFilter : IKeyFilter
    {
        private readonly IMapper _mapper;
        private readonly IKeyValidator _keyValidator;
        private readonly IExposureKeyMapper _exposureKeyMapper;
        private readonly ILogger _logger;
        private readonly ITemporaryExposureKeyRepository _tempKeyRepository;
        private readonly IKeyValidationConfigurationService _keyValidationConfigurationService;

        private const int BatchSize = 10000;
        private const int OutdatedKeysDayOffset = 14;

        public KeyFilter(IMapper mapper,
                        IKeyValidator keyValidator,
                        IExposureKeyMapper exposureKeyMapper,
                        ILogger<KeyFilter> logger,
                        ITemporaryExposureKeyRepository keyRepository)
        {
            _mapper = mapper;
            _keyValidator = keyValidator;
            _exposureKeyMapper = exposureKeyMapper;
            _logger = logger;
            _tempKeyRepository = keyRepository;
        }

        public IList<TemporaryExposureKey> ValidateKeys(IList<TemporaryExposureKey> temporaryExposureKeys, out IList<string> validationErrors)
        {
            validationErrors = new List<string>();
            var errorMessage = String.Empty;
            IList<TemporaryExposureKey> acceptedKeys = new List<TemporaryExposureKey>();

            foreach (TemporaryExposureKey exposureKey in temporaryExposureKeys)
            {

                if (_keyValidator.ValidateKeyGateway(exposureKey, out errorMessage))
                {
                    acceptedKeys.Add(exposureKey);
                }
                else
                {
                    validationErrors.Add($"The key {"0x" + BitConverter.ToString(exposureKey.KeyData).Replace("-", "")} was  rejected: {Environment.NewLine}\t{errorMessage}");
                }
            }
            return acceptedKeys;
        }

        public IList<TemporaryExposureKey> MapKeys(IList<TemporaryExposureKeyGatewayDto> temporaryExposureKeys)
        {
            IList<TemporaryExposureKey> mappedKeys = new List<TemporaryExposureKey>();

            foreach (TemporaryExposureKeyGatewayDto exposureKeyDTO in temporaryExposureKeys)
            {
                var exposureKey = _mapper.Map<TemporaryExposureKeyGatewayDto, TemporaryExposureKey>(exposureKeyDTO);
                exposureKey.KeySource = KeySource.Gateway;

                mappedKeys.Add(exposureKey);
            }

            return mappedKeys;
        }

        public async Task<IList<TemporaryExposureKey>> RemoveKeyDuplicatesAsync(IList<TemporaryExposureKey> downloadedKeys)
        {
            var numberOfRecordsToSkip = 0;
            var filteredKeys = downloadedKeys;
            IList<TemporaryExposureKey> dbKeysBatch;
            do
            {
                var validRollingStartNumberThreshold =
                    ConvertOffsetDaysToRollingStartNumber(OutdatedKeysDayOffset);

                dbKeysBatch = await _tempKeyRepository.GetNextBatchOfKeysWithRollingStartNumberThresholdAsync(validRollingStartNumberThreshold, numberOfRecordsToSkip, BatchSize);
                filteredKeys = _exposureKeyMapper.FilterDuplicateKeys(filteredKeys, dbKeysBatch);
                numberOfRecordsToSkip += dbKeysBatch.Count;

            } while (dbKeysBatch.Count == BatchSize);

            return filteredKeys;
        }

        private long ConvertOffsetDaysToRollingStartNumber(int offsetDays)
        {
            return DateTimeOffset.Now.Date.AddDays(-(offsetDays + 1)).ToUnixEpoch();
        }
    }
}
