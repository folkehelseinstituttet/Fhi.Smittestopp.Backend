using DIGNDB.App.SmitteStop.Core.Models;
using System.Threading.Tasks;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Core.Contracts
{
    public interface IAppleService
    {
        AppleQueryBitsDto BuildQueryBitsDto(string deviceToken);
        AppleUpdateBitsDto BuildUpdateBitsDto(string deviceToken);
        Task<AppleResponseDto> ExecuteQueryBitsRequest(string deviceToken);
        Task<AppleResponseDto> ExecuteUpdateBitsRequest(string deviceToken);
    }
}
