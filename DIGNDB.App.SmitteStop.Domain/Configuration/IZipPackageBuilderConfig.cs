namespace DIGNDB.App.SmitteStop.Domain.Configuration
{
    public interface IZipPackageBuilderConfig
    {
        int MaxKeysPerFile { get; }

        int FetchCommandTimeout { get; }

        string CertificateThumbprint { get; }
    }
}
