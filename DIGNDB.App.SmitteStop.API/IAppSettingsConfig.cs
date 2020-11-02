using Microsoft.Extensions.Configuration;

namespace DIGNDB.App.SmitteStop.API
{
    public interface IAppSettingsConfig
    {
        IConfiguration Configuration { get; }
    }
}