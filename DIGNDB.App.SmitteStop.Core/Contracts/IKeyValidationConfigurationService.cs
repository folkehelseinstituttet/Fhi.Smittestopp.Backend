using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Domain.Configuration;
using Microsoft.Extensions.Configuration;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IKeyValidationConfigurationService
    {
        void SetConfiguration(IConfiguration configuration);
        KeyValidationConfiguration GetConfiguration();
    }
}
