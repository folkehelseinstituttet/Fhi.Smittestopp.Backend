using DIGNDB.App.SmitteStop.Domain.Db;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public interface ISettingRepository
    {
        Setting FindSettingByKey(string key);
        void SetSetting(string key, string value);
    }
}
