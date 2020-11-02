using DIGNDB.App.SmitteStop.Domain;
using FederationGatewayApi.Models.Proto;
using System.Security.Cryptography.X509Certificates;

namespace FederationGatewayApi.Services
{
    public interface ISignatureService
    {
        /// <summary>
        /// Generates signature based on the batch object
        /// </summary>
        /// <param name="data">Data to sign</param>
        /// <returns>Signature in base64 format</returns>
        byte[] Sign(TemporaryExposureKeyGatewayBatchDto protoBatch, SortOrder keysSortOrder);
    }
}