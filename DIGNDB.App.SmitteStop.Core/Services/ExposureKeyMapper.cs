using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    public class ExposureKeyMapper : IExposureKeyMapper
    {
        /*
         * Constant to convert second into 10min interval
         * Because rolling start needs to be stored as increments of 10mins
        */
        private const int secTo10min = 60 * 10;
        private readonly IEpochConverter _epochConverter;

        public ExposureKeyMapper(IEpochConverter epochConverter)
        {
            _epochConverter = epochConverter;
        }
        public List<TemporaryExposureKey> FromDtoToEntity(TemporaryExposureKeyBatchDto dto)
        {
            return dto.keys.Select(key => new TemporaryExposureKey()
            {
                CreatedOn = DateTime.UtcNow,
                RollingStartNumber = _epochConverter.ConvertToEpoch(key.rollingStart),
                KeyData = key.key,
                RollingPeriod = (long)(key.rollingDurationSpan.TotalMinutes / 10.0d),
                TransmissionRiskLevel = key.transmissionRiskLevel,
                DaysSinceOnsetOfSymptoms = key.daysSinceOnsetOfSymptoms,
                SharingConsentGiven = dto.sharingConsentGiven
            }).ToList();
        }

        public Domain.Proto.TemporaryExposureKey FromEntityToProto(TemporaryExposureKey source)
        {
            return new Domain.Proto.TemporaryExposureKey(
                    source.KeyData,
                    (int)source.RollingStartNumber / secTo10min,
                    (int)source.RollingPeriod,
                    (int)source.TransmissionRiskLevel
                );
        }

        public Domain.Proto.TemporaryExposureKeyExport FromEntityToProtoBatch(IList<TemporaryExposureKey> dtoKeys)
        {
            var keysByTime = dtoKeys.OrderBy(k => k.CreatedOn);
            var startTimes = new DateTimeOffset(keysByTime.First().CreatedOn);
            var endTimes = new DateTimeOffset(keysByTime.Last().CreatedOn);
            var batch = new Domain.Proto.TemporaryExposureKeyExport
            {
                BatchNum = 1,
                BatchSize = 1,
                StartTimestamp = (ulong)(startTimes.ToUnixTimeSeconds()),
                EndTimestamp = (ulong)(endTimes.ToUnixTimeSeconds()),
                Region = "DK"
            };
            batch.Keys.AddRange(dtoKeys.Select(x => FromEntityToProto(x)).ToList());

            return batch;
        }
    }
}
