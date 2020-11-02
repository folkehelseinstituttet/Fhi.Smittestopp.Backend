using DIGNDB.App.SmitteStop.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Domain.Db;

namespace FederationGatewayApi.Contracts
{
    public interface IGatewayKeyDownloader
    {
         Task<List<TemporaryExposureKey>> DownloadKeysAsync();

    }
}
