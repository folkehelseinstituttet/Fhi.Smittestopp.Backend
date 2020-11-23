using DIGNDB.App.SmitteStop.Domain.Configuration;
using DIGNDB.App.SmitteStop.Domain.Dto;
using Microsoft.Extensions.Logging;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IExposureKeyValidator
    {
        void ValidateParameterAndThrowIfIncorrect(TemporaryExposureKeyBatchDto parameter, KeyValidationConfiguration config);
    }
}
