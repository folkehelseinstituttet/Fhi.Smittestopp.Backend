using System;
using System.Globalization;
using DIGNDB.App.SmitteStop.Core.Contracts;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DIGNDB.APP.SmitteStop.API.Exceptions;

namespace DIGNDB.App.SmitteStop.API.Services
{
    public class StatisticsFileService : IStatisticsFileService
    {
        private readonly IFileSystem _fileSystem;

        public StatisticsFileService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public int DeleteOldFiles(string path, int days, string datePattern, string dateParsingFormat)
        {
            var folder = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var fileNameDate = ExtractDateFromFilename(fileName, datePattern, dateParsingFormat);

            var f = fileNameDate.AddDays(-days);
            var prefix = fileName.Substring(0, fileName.LastIndexOf('_'));
            var files = _fileSystem.GetFileNamesFromDirectory(folder).Where(f => f.Contains(prefix));
            var count = 0;
            foreach (var file in files)
            {
                var tempFileNameDate = ExtractDateFromFilename(file, datePattern, dateParsingFormat);
                if (tempFileNameDate > f)
                {
                    continue;
                }

                _fileSystem.DeleteFile(file);
                count++;
            }

            return count;
        }

        private static DateTime ExtractDateFromFilename(string filename, string zipPackageDatePattern, string zipPackageDateParsingFormat)
        {
            var zipPackageDateMatches = Regex.Matches(filename, zipPackageDatePattern);
            if (zipPackageDateMatches.Count == 0 || zipPackageDateMatches.Count > 1)
            {
                throw new GitHubControllerServerErrorException("Unable to extract date from zip file name");
            }

            var retVal = DateTime.ParseExact(zipPackageDateMatches[0].Value, zipPackageDateParsingFormat, CultureInfo.InvariantCulture);
            return retVal;
        }
    }
}
