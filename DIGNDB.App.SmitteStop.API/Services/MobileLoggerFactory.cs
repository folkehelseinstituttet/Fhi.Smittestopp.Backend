using log4net;
using log4net.Repository;
using System.IO;
using System.Reflection;
using System.Xml;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public static class MobileLoggerFactory 
    {
        private static readonly string LOG_CONFIG_FILE = @"log4net.config";
        private static ILoggerRepository repository;

        static MobileLoggerFactory()
        {
            SetLog4NetConfiguration();
        }

        public static ILog GetLogger()
        {
            return LogManager.GetLogger(repository.Name, "MobileLog");
        }
        
        private static void SetLog4NetConfiguration()
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(LOG_CONFIG_FILE));

            repository = LogManager.CreateRepository(
                Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repository, log4netConfig["log4net"]);
        }
    }
}
