using AutoMapper;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Helpers;
using DIGNDB.App.SmitteStop.DAL.Repositories;
using DIGNDB.App.SmitteStop.Domain;
using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.App.SmitteStop.Domain.Enums;
using DIGNDB.App.SmitteStop.Domain.Settings;
using FederationGatewayApi.Config;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Models;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using TemporaryExposureKeyGatewayBatchProtoDto = FederationGatewayApi.Models.Proto.TemporaryExposureKeyGatewayBatchDto;
using TemporaryExposureKeyGatewayDtoProto = FederationGatewayApi.Models.Proto.TemporaryExposureKeyGatewayDto;

namespace FederationGatewayApi.Services
{
    public class EuGatewayService : IEuGatewayService
    {
        private const string DownloadBatchTagName = "batchTag";
        private const SortOrder KeysSortOrderUsedToCreateAndVerifyUploadRequestSignature = SortOrder.ASC;

        private readonly IGatewayHttpClient _gatewayHttpClient;
        private readonly ITemporaryExposureKeyRepository _tempKeyRepository;
        private readonly ISignatureService _signatureService;
        public readonly IEncodingService _encodingService;
        private readonly IKeyFilter _keyFilter;
        private readonly IGatewayWebContextReader _webContextReader;
        private readonly IMapper _mapper;
        private readonly EuGatewayConfig _euGatewayConfig;
        private readonly ISettingsService _settingsService;
        private readonly IEpochConverter _epochConverter;
        private readonly IEFGSKeyStoreService _storeService;

        public ILogger<EuGatewayService> _logger { get; set; }
        public IKeyValidator _keyValidator { get; set; }

        public EuGatewayService(ITemporaryExposureKeyRepository tempKeyRepository,
                               ISignatureService signatureService,
                               IEncodingService encodingService,
                               IKeyFilter keyFilter,
                               IGatewayWebContextReader gatewayWebContextReader,
                               IMapper mapper,
                               ILogger<EuGatewayService> logger,
                               EuGatewayConfig euGatewayConfig,
                               ISettingsService settingsService,
                               IEpochConverter epochConverter,
                               IGatewayHttpClient gatewayHttpClient,
                               IEFGSKeyStoreService storeService
                               )
        {
            _signatureService = signatureService;
            _tempKeyRepository = tempKeyRepository;
            _encodingService = encodingService;
            _keyFilter = keyFilter;
            _webContextReader = gatewayWebContextReader;
            _mapper = mapper;
            _logger = logger;
            _euGatewayConfig = euGatewayConfig;
            _settingsService = settingsService;
            _epochConverter = epochConverter;
            _gatewayHttpClient = gatewayHttpClient;
            _storeService = storeService;
        }

        public void UploadKeysToTheGateway(int fromLastNumberOfDays, int batchSize, int? batchCountLimit = null)
        {
            if (fromLastNumberOfDays <= 0) throw new ArgumentException($"UploadKeysToTheGateway: Incorrect fromLastNumberOfDays {fromLastNumberOfDays}");
            if (batchSize <= 0) throw new ArgumentException($"UploadKeysToTheGateway: Incorrect batchSize {batchSize}");
            if (batchCountLimit.HasValue && batchCountLimit.Value <= 0) throw new ArgumentException($"UploadKeysToTheGateway: Incorrect batchCountLimit {batchCountLimit}");

            if (batchCountLimit.HasValue)
            {
                _logger.LogInformation($"Number of batches to process are limited to {batchCountLimit}");
            }

            var stats = new BatchUploadStats();
            BatchStatus lastStatus = new BatchStatus();
            do
            {
                if (IsLimitReached(currentValue: stats.CurrentBatchNumber, limitValue: batchCountLimit))
                {
                    _logger.LogInformation($"Limit of {batchCountLimit} batches has been reached. Stopping upload process");
                    break;
                }
                ++stats.CurrentBatchNumber;
                lastStatus = UploadNextBatch(batchSize, fromLastNumberOfDays);

                stats.TotalKeysProcessed += lastStatus.KeysProcessed;
                stats.TotalKeysSent += lastStatus.KeysSent;
            }
            while (lastStatus.ProcessedSuccessfully && lastStatus.NextBatchExists);

            if (lastStatus.ProcessedSuccessfully)
            {
                _logger.LogInformation($"Upload ended. Last batch processed successfully. Upload status: {stats.TotalKeysSent} keys ({ stats.CurrentBatchNumber} batches) has been sent  from {stats.TotalKeysProcessed} records processed.");
            }
            else
            {
                _logger.LogError($"Upload interrupted. Last batch processed with error. Upload status: {stats.TotalKeysSent} keys ({ stats.CurrentBatchNumber - 1} batches) has been sent from {stats.TotalKeysProcessed} records processed.");
                throw new InvalidOperationException($"Upload interrupted! Error occurred while sending batch {stats.CurrentBatchNumber}.");
            }
        }

        private BatchStatus UploadNextBatch(int batchSize, int keyAgeLimitInDays)
        {
            var lastSyncState = _settingsService.GetGatewayUploadState();

            // Select only keys from last N days (by date of record creation)
            var uploadedOnAndAfterTicks = lastSyncState.CreationDateOfLastUploadedKey;
            var uploadedOnAndAfter = uploadedOnAndAfterTicks.HasValue ? new DateTime(uploadedOnAndAfterTicks.Value, DateTimeKind.Utc) : DateTime.UnixEpoch;

            int batchSizePlusOne = batchSize + 1; // if it will return n + 1 then there is at last one more records to send

            // Get key package - collection of the records created (uploaded by mobile app) in the db after {uploadedOn}
            IList<TemporaryExposureKey> keyPackage = _tempKeyRepository.GetKeysOnlyFromApiOriginCountryUploadedAfterTheDateForGatewayUpload(
                  uploadedOnAndLater: uploadedOnAndAfter,
                  numberOfRecordToSkip: lastSyncState.NumberOfKeysProcessedFromTheLastCreationDate,
                  maxCount: batchSizePlusOne,
                  new KeySource[] { KeySource.SmitteStopApiVersion2 });

            // Take all record uploaded after the date.
            var currBatchStatus = new BatchStatus() { NextBatchExists = keyPackage.Count == batchSizePlusOne };

            keyPackage = keyPackage.Take(batchSize).ToList();
            currBatchStatus.KeysProcessed = keyPackage.Count;
            currBatchStatus.ProcessedSuccessfully = true;

            if (!keyPackage.Any())
            {
                _logger.LogInformation("KeyPackage is empty. Stopping the upload process.");
                return currBatchStatus;
            }
            _logger.LogInformation($"{keyPackage.Count} records picked.");

            var oldestKeyFromPackage = keyPackage.Last(); // be aware that it could not be present in the batch. This is the last record (have oldest CreationOn) that has been processed.

            // Create Batch based on package but filter in on RollingStartNumber. Batch.Size <= Package.Size
            // We can send data not older than N days ago (age of the key save in key.RollingStartNumber)
            DateTime createdAfter = DateTime.UtcNow.AddDays(-keyAgeLimitInDays);
            var createdAfterTimestamp = _epochConverter.ConvertToEpoch(createdAfter);

            var filteredKeysForBatch =
                keyPackage
                    .Where(k => k.RollingStartNumber > createdAfterTimestamp)
                    .Where(k => k.DaysSinceOnsetOfSymptoms >= KeyValidator.DaysSinceOnsetOfSymptomsValidRangeMin)
                    .Where(k => k.DaysSinceOnsetOfSymptoms <= KeyValidator.DaysSinceOnsetOfSymptomsValidRangeMax)
                    .ToList();
            currBatchStatus.KeysToSend = filteredKeysForBatch.Count;

            var batch = CreateGatewayBatchFromKeys(filteredKeysForBatch);
            TemporaryExposureKeyGatewayBatchProtoDto protoBatch = MapBatchDtoToProtoAndSortForSigning(batch);

            _logger.LogInformation($"{currBatchStatus.KeysToSend} records to send after filtering by age.");

            if (protoBatch.Keys.Any())
            {
                _logger.LogInformation($"Sending batch...");
                currBatchStatus.ProcessedSuccessfully = TrySendKeyBatchToTheGateway(protoBatch, KeysSortOrderUsedToCreateAndVerifyUploadRequestSignature);
            }
            else
            {
                _logger.LogInformation($"Nothing to sent for this package. All picked keys are older then {keyAgeLimitInDays} days or have incorrect value of DaysSinceOnsetOfSymptoms.");
            }

            if (currBatchStatus.ProcessedSuccessfully)
            {
                currBatchStatus.KeysSent = protoBatch.Keys.Count;

                var currSyncState = new GatewayUploadState();
                var lastSentKeyTicks = oldestKeyFromPackage.CreatedOn.Ticks;
                // If the date of the last sent element (recently) has changed in compare the date from the previous batch?
                //  NO - Update the state: Do not change the date, only offset (+=)
                //  YES - Update the state: Change date to the date from last sent element. Update the index to the offset from the first appearance of that date.)
                if (lastSyncState.CreationDateOfLastUploadedKey == lastSentKeyTicks)
                {
                    currSyncState.CreationDateOfLastUploadedKey = lastSyncState.CreationDateOfLastUploadedKey;
                    currSyncState.NumberOfKeysProcessedFromTheLastCreationDate = lastSyncState.NumberOfKeysProcessedFromTheLastCreationDate + currBatchStatus.KeysProcessed;
                }
                else
                {
                    currSyncState.CreationDateOfLastUploadedKey = oldestKeyFromPackage.CreatedOn.Ticks;
                    // find offset form fist element with CreationDateOfLastUploadedKey to last sent element
                    var indexOfTheFirstKeyWithTheDate = keyPackage.Select((k, i) => new { Key = k, Index = i })
                                                               .First(o => o.Key.CreatedOn.Ticks == lastSentKeyTicks)
                                                               .Index;

                    currSyncState.NumberOfKeysProcessedFromTheLastCreationDate = currBatchStatus.KeysProcessed - indexOfTheFirstKeyWithTheDate;
                }
                // save new sync status
                _settingsService.SaveGatewaySyncState(currSyncState);
            }
            else
            {
                _logger.LogError($"Error sending the batch! Stopping upload process.");
            }
            return currBatchStatus;
        }

        private TemporaryExposureKeyGatewayBatchDto CreateGatewayBatchFromKeys(IList<TemporaryExposureKey> keyPackage)
        {
            var batchKeys = keyPackage.Select(entityKey => _mapper.Map<TemporaryExposureKeyGatewayDto>(entityKey)).ToList();
            return new TemporaryExposureKeyGatewayBatchDto() { Keys = batchKeys };
        }

        /*
         * Info from EFGS team:
         * EFGS will never change a existing batch.
         * Batching process is triggered all 5 minutes. And then the batching searches all uploaded keys of the last 5 minutes. And these keys will be put into a new batch
         * 
         */
        public void DownloadKeysFromGateway(int forLastNumberOfDays)
        {
            var lastDownloadState = _settingsService.GetGatewayDownloadState();
            string lastSyncedBatchTag = lastDownloadState.LastSyncedBatchTag;

            var lastSyncDateTicks = lastDownloadState.LastSyncDate;
            var lastSyncDate = lastSyncDateTicks.HasValue ? new DateTime(lastSyncDateTicks.Value, DateTimeKind.Utc) : DateTime.UnixEpoch;

            DateTime todayDate = DateTime.UtcNow.Date;
            // It there hasn't been sync for more than {maximumNumberOfDaysBack} change date to the oldest possible and reset the batchTag
            DateTime oldestDatePossible = todayDate.AddDays(-forLastNumberOfDays + 1);
            var startDate = lastSyncDate < oldestDatePossible ? oldestDatePossible : lastSyncDate;
            if (startDate != lastSyncDate)
            {
                lastSyncedBatchTag = null; // date has changed BatchTag need to be reset
                _logger.LogWarning($"CHECK IF DOWNLOAD IS CALLED PERIODICALLY. Last Download has been at {lastSyncDate.Date}." +
                    $" The gateway have history only for last {forLastNumberOfDays} days (from <{oldestDatePossible},{todayDate}>).");
            }

            var currentDownloadDate = startDate;
            while (currentDownloadDate <= todayDate)
            {
                var lastProcessedBatchTag = DownloadKeysFromGatewayFromGivenDate(currentDownloadDate, lastSyncedBatchTag);
                //save lastProcessedBatchTag
                var currentState = new GatewayDownloadState()
                {
                    LastSyncDate = currentDownloadDate.Ticks,
                    LastSyncedBatchTag = lastProcessedBatchTag
                };
                _settingsService.SaveGatewaySyncState(currentState);

                currentDownloadDate = currentDownloadDate.AddDays(1);
                lastSyncedBatchTag = null;
            }

        }

        private string DownloadKeysFromGatewayFromGivenDate(DateTime date, string lastSyncedBatchTag)
        {
            _logger.LogInformation($"|SmitteStop:DownloadKeysFromGateway| Execution started for the date {date}.");

            var dateInputString = date.ToString("yyyy-MM-dd");
            var requestUrl = $"{_euGatewayConfig.UrlNormalized}diagnosiskeys/download/{dateInputString}";
            string batchTag;
            string nextBatchTag = lastSyncedBatchTag;

            long acceptedKeysTotalCount = 0;
            long downloadedKeysTotalCount = 0;
            int batchNumber = 0;

            IList<string> allTags = new List<string>();
            do
            {
                batchTag = nextBatchTag;
                HttpResponseMessage responseMessage;
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl))
                {
                    // batchTag is null for the first batch
                    if (batchTag != null) requestMessage.Headers.Add(DownloadBatchTagName, batchTag);

                    _logger.LogInformation($"|SmitteStop:DownloadKeysFromGateway| Calling endpoint: {requestUrl} with batchTag: {batchTag} (null for the first batch)");
                    try
                    {
                        responseMessage = _gatewayHttpClient.SendAsync(requestMessage).Result;
                    }
                    catch (Exception e)
                    {
                        var errMessage = "|SmitteStop:DownloadKeysFromGateway| GatewayClient GetAsync: Error in retrieving data from the gateway server: ||" + e;
                        _logger.LogError(errMessage);
                        throw;
                    }
                }


                var jsonString = GetResponseOrThrow(responseMessage);
                _logger.LogInformation($"|SmitteStop:DownloadKeysFromGateway| Parsed response successfully. JsonResponse is not null or empty: {string.IsNullOrEmpty(jsonString)} ");

                if (lastSyncedBatchTag != null && lastSyncedBatchTag == batchTag)
                {
                    // This is another call for the same day. Do not save the keys one more time. Just check if nextBatchTag exists this time.
                    _logger.LogInformation($"|SmitteStop:DownloadKeysFromGateway| Batch has been downloaded before. Just checking is nextBatchTag exist. ");
                }
                else
                {
                    var keys = _webContextReader.GetItemsFromRequest(jsonString);
                    _logger.LogInformation($"|SmitteStop:DownloadKeysFromGateway| Received {keys?.Count } keys.");
                    var acceptedKeys = _storeService.FilterAndSaveKeys(keys);
                    _logger.LogInformation($"|SmitteStop:DownloadKeysFromGateway| Accepted {acceptedKeys?.Count } keys.");

                    downloadedKeysTotalCount += keys.Count;
                    acceptedKeysTotalCount += acceptedKeys.Count;
                }

                nextBatchTag = GetValidNextBatchTagOrThrow(responseMessage, allTags);
                _logger.LogInformation($"|SmitteStop:DownloadKeysFromGateway| NextBatch: {nextBatchTag}");
                if (nextBatchTag != null)
                {
                    allTags.Add(nextBatchTag);
                }

                ++batchNumber;
            } while (nextBatchTag != null);

            // save last BatchTag - date and batch number
            _logger.LogInformation($"|SmitteStop:DownloadKeysFromGateway| Execution ended for the date {date}. {downloadedKeysTotalCount} keys downloaded in {batchNumber} batches. Accepted {acceptedKeysTotalCount} of them. All processed BatchTags:{String.Join(", ", allTags.ToArray())}");

            _logger.LogInformation($"|SmitteStop:DownloadKeysFromGateway| Finishing. Last  not null batchTag from successful download: {batchTag}");
            // return last downloaded batchTag
            return batchTag;
        }

        private string GetResponseOrThrow(HttpResponseMessage responseMessage)
        {
            var responseBodyString = _webContextReader.ReadHttpContextStream(responseMessage);

            var success = IsDownloadRequestSuccess(responseMessage.StatusCode, responseBodyString);
            if (!success)
            {
                // Fail the Job for retry
                throw new InvalidOperationException($"|SmitteStop:DownloadKeysFromGateway| Endpoint returned code: {responseMessage.StatusCode} and message: {responseBodyString}");
            }
            return responseBodyString;
        }

        private bool IsDownloadRequestSuccess(HttpStatusCode statusCode, string responseBodyString)
        {
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                case HttpStatusCode.NotFound:
                    _logger.LogWarning($"NotFound: Message: {responseBodyString}");
                    return true;
                case HttpStatusCode.BadRequest:
                    _logger.LogError($"Invalid BatchTag used. Message: {responseBodyString}");
                    break;
                case HttpStatusCode.Forbidden:
                    _logger.LogError($"Forbidden call in cause of missing or invalid client certificate. Message: {responseBodyString}");
                    break;
                case HttpStatusCode.NotAcceptable:
                    _logger.LogError($"Data format or content is not valid. Message: {responseBodyString}");
                    break;
                case HttpStatusCode.Gone:
                    _logger.LogError($"Date for download expired. Date does not more exists. Message: {responseBodyString}");
                    break;
                default:
                    _logger.LogError($"Response code was not recognized. Status: {statusCode}.  Message: {responseBodyString}");
                    break;
            }
            return false;
        }

        private string GetValidNextBatchTagOrThrow(HttpResponseMessage responseMessage, IList<string> allTags)
        {
            var headerName = "nextBatchTag";
            var nextBatchTag = responseMessage.Headers.Contains(headerName) ? responseMessage.Headers.GetValues(headerName).FirstOrDefault() : null;
            if (nextBatchTag == null)
            {
                return null;
            }

            if (nextBatchTag.Trim().ToLower() == "null")
            {
                nextBatchTag = null;

            }
            if (allTags.Contains(nextBatchTag))
            {
                var tags = String.Join(", ", allTags.ToArray());
                var notUniqueTagErrorMessage = $"|SmitteStop:DownloadKeysFromGateway| NextBatchTag is the same as previous ones. Throwing error to prevent infinite loop. NextBatchTag: {nextBatchTag}, Previous: {tags}";
                _logger.LogError(notUniqueTagErrorMessage);
                throw new InvalidOperationException(notUniqueTagErrorMessage);
            }
            return nextBatchTag;
        }

        private bool TrySendKeyBatchToTheGateway(TemporaryExposureKeyGatewayBatchProtoDto protoBatch, SortOrder keySortOrderForSignature)
        {
            var upoadKeysEndpointUrl = _euGatewayConfig.UrlNormalized + EuGatewayContract.Endpoint.KeysUploadEndpoint;
            var batchBytes = protoBatch.ToByteArray();

            //sign
            var signing = _signatureService.Sign(protoBatch, keySortOrderForSignature);
            var signingBase64 = _encodingService.EncodeToBase64(signing);
            var body = new ByteArrayContent(batchBytes);

            var uniqueTag = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            body.Headers.Add("batchTag", uniqueTag);
            body.Headers.Add("Content-Type", "application/protobuf;version=1.0");
            body.Headers.Add("batchSignature", signingBase64);

            var response = _gatewayHttpClient.PostAsync(upoadKeysEndpointUrl, body).Result;
            var message = response.Content.ReadAsStringAsync().Result;
            var code = response.StatusCode;

            switch (code)
            {
                case System.Net.HttpStatusCode.OK:
                    _logger.LogInformation($"Response - OK. Message: {message}");
                    var containsHtml = message.Contains("<body>");
                    _logger.LogError($"UeGateway response with code 200 with HTML in the response: {containsHtml}. UeGateway Server is down!");
                    // Server is down - https://github.com/eu-federation-gateway-service/efgs-federation-gateway/issues/151
                    break;
                case System.Net.HttpStatusCode.Created:
                    _logger.LogInformation("Keys successfully uploaded.");
                    return true;
                case System.Net.HttpStatusCode.MultiStatus:
                    _logger.LogWarning($"Data partially added with warnings: {message}.");
                    return true;
                case System.Net.HttpStatusCode.BadRequest:
                    _logger.LogError($"Bad request: {message}");
                    break;
                case System.Net.HttpStatusCode.Forbidden:
                    _logger.LogError($"Forbidden call in cause of missing or invalid client certificate. Message: {message}");
                    break;
                case System.Net.HttpStatusCode.NotAcceptable:
                    _logger.LogError($"Data format or content is not valid. Massage:{message}");
                    break;
                case System.Net.HttpStatusCode.Conflict:
                    _logger.LogError($"Data already exist. Message: {message}");
                    break;
                case System.Net.HttpStatusCode.RequestEntityTooLarge:
                    _logger.LogError($"Payload to large.  Message: {message}");
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    _logger.LogError($"Are not able to write data. Retry please. Message: {message}");
                    break;
                default:
                    _logger.LogError($"Response code was not recognized. Status code: {code}, message: {message}");
                    break;
            }
            return false;
        }

        private TemporaryExposureKeyGatewayBatchProtoDto MapBatchDtoToProtoAndSortForSigning(TemporaryExposureKeyGatewayBatchDto dto)
        {
            var protoKeys = dto.Keys.Select(gatewayDto => _mapper.Map<TemporaryExposureKeyGatewayDtoProto>(gatewayDto)).ToList();

            var protoBatch = new TemporaryExposureKeyGatewayBatchProtoDto();
            protoBatch.Keys.AddRange(protoKeys);

            return protoBatch;
        }

        private bool IsLimitReached(long currentValue, long? limitValue)
        {
            return limitValue.HasValue && currentValue >= limitValue.Value;
        }
    }
}
