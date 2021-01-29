using DIGNDB.App.SmitteStop.DAL.Context;
using DIGNDB.App.SmitteStop.Domain.Db;
using System;
using System.Linq;

namespace DIGNDB.App.SmitteStop.DAL.Repositories
{
    public class SettingRepository : GenericRepository<Setting>, ISettingRepository
    {
        public SettingRepository(DigNDB_SmittestopContext context) : base(context) { }

        public Setting FindSettingByKey(string key)
        {
            Setting setting = new Setting();

            try
            {
                setting = _context.Setting.SingleOrDefault(x => x.Key == key);
            }
            catch (InvalidOperationException e)
            {
                throw new InvalidOperationException("The Setting entry was not a unique key.", innerException: e);
            }
            catch (ArgumentNullException e)
            {
                throw new ArgumentNullException("Source or predicate is null", innerException: e);
            }

            return setting;
        }

        public void SetSetting(string key, string value)
        {
            Setting setting = FindSettingByKey(key);
            if (setting == null)
            {
                setting = new Setting()
                {
                    Key = key,
                    Value = value
                };
                CreateSetting(setting);
            }
            else
            {
                setting.Value = value;
                UpdateSetting(setting);
            }
        }

        private void CreateSetting(Setting setting)
        {
            _context.Setting.Add(setting);
            _context.SaveChanges();
        }

        private void UpdateSetting(Setting setting)
        {
            _context.Setting.Update(setting);
            _context.SaveChanges();
        }
    }
}
