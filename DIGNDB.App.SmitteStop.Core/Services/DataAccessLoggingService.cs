using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Enums;
using DIGNDB.App.SmitteStop.Domain;
using Microsoft.Extensions.Logging;

namespace DIGNDB.App.SmitteStop.Core.Services
{
    /// <summary>
    /// Class used to log who and when was accessing specific data.
    /// </summary>
    /// <typeparam name="TCategoryName">The type who's name is used for the logger category name.</typeparam>
    public class DataAccessLoggingService<TCategoryName> : IDataAccessLoggingService<TCategoryName>
    {
        private readonly ILogger<TCategoryName> _logger;

        public DataAccessLoggingService(ILogger<TCategoryName> logger)
        {
            _logger = logger;
        }

        public void LogDataAccess<TId>(IEnumerable<IIdentifiedEntity<TId>> dataCollection, DataOperation dataOperation, IIdentity identity)
        {
            if (dataCollection == null || !dataCollection.Any())
                return;

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"Operation: {dataOperation}, ");

            if(identity.Name != null)
                stringBuilder.Append($"User performing: {identity.Name}, ");

            stringBuilder.Append($"Data type: {dataCollection.GetType().GetGenericArguments().FirstOrDefault()}, ");

            stringBuilder.Append($"Data ids: ");
            foreach (var data in dataCollection.SkipLast(1))
                stringBuilder.Append($"{data.Id.ToString()}, ");
            stringBuilder.Append($"{dataCollection.Last().Id.ToString()}.");

            _logger.LogInformation(stringBuilder.ToString());
        }
    }
}