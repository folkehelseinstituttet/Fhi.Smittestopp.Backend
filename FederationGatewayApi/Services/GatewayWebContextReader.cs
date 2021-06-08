using System;
using AutoMapper;
using FederationGatewayApi.Contracts;
using FederationGatewayApi.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace FederationGatewayApi.Services
{
    public class GatewayWebContextReader : IGatewayWebContextReader
    {
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
                _logger.LogError($"|SmitteStop:DownloadKeysFromGateway|GetItemsFromRequest: responseBody '{responseBody}' - {e.Message} - {e.StackTrace}");
                throw;
            }
        }
    }
}