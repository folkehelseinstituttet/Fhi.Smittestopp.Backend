namespace DIGNDB.App.SmitteStop.Domain.Configuration
{
    public interface IPackageBuilderConfig
    {
        int MaxKeysPerFile { get; }
        int FetchCommandTimeout { get; }
    }
}
