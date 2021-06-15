using AutoMapper;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace FederationGatewayApi.Services
{
    public class GatewayWebContextReader : IGatewayWebContextReader
    {
        private const string GatewayMessage = "Could not find any batches for given date";

        private readonly IMapper _mapper;
        private readonly ILogger<GatewayWebContextReader> _logger;

        public GatewayWebContextReader(IMapper mapper, ILogger<GatewayWebContextReader> logger)
        {
            _mapper = mapper;
            _logger = logger;
        }

        public string ReadHttpContextStream(HttpResponseMessage webContext)
        {
            using var reader = new StreamReader(webContext.Content.ReadAsStreamAsync().Result);
            return reader.ReadToEndAsync().Result;
        }

        public IList<TemporaryExposureKeyGatewayDto> GetItemsFromRequest(string responseBody)
        {
            try
            {
                var batchProtoObject = Models.Proto.TemporaryExposureKeyGatewayBatchDto.Parser.ParseJson(responseBody);
                var batchKeys = batchProtoObject.Keys.ToList();
                var mappedKeys = batchKeys.Select(entityKey => _mapper.Map<TemporaryExposureKeyGatewayDto>(entityKey));
                return mappedKeys.ToList();
            }
            catch (Exception e)
            {
                try
                {
                    var gatewayMessage = JsonSerializer.Deserialize<GatewayMessage>(responseBody);
                    if (!gatewayMessage.message.Equals(GatewayMessage))
                    {
                        throw;
                    }

                    var warning = $"|SmitteStop:DownloadKeysFromGateway|GetItemsFromRequest: responseBody {responseBody}";
                    _logger.LogWarning(warning);
                    return new List<TemporaryExposureKeyGatewayDto>();

                }
                catch (Exception ex)
                {
                    var secondException = $"Second exception: {ex.Message} - {ex.StackTrace}";
                    var errorMessage = $"|SmitteStop:DownloadKeysFromGateway|GetItemsFromRequest: responseBody '{responseBody}' - {e.Message} - {e.StackTrace}.\n{secondException}";
                    _logger.LogError(errorMessage);
                    throw;
                }
            }
        }
    }
}