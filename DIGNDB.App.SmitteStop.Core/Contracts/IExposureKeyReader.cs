using System.IO;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IExposureKeyReader
    {
        Task<TemporaryExposureKeyBatchDto> ReadParametersFromBody(Stream requestBody);
    }
}
