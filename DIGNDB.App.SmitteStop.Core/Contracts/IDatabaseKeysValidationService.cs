using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public interface IDatabaseKeysValidationService
    {
        void FindAndRemoveInvalidKeys(int batchSize);
    }
}
