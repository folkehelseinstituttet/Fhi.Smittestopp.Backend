using Microsoft.Extensions.Configuration;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IExportKeyConfigurationService
    {
        void SetConfiguration(IConfiguration configuration);
        ExportKeyConfiguration GetConfiguration();
    }
}
