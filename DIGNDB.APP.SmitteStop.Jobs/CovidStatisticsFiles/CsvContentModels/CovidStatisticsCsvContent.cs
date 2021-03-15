using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels
{
    public class CovidStatisticsCsvContent
    {
        public IReadOnlyCollection<IEnumerable<CovidStatisticCsvFileContent>> FileContents =>
            _fileContents.AsReadOnly();
        private readonly List<IEnumerable<CovidStatisticCsvFileContent>> _fileContents;

        public CovidStatisticsCsvContent()
        {
            _fileContents = new List<IEnumerable<CovidStatisticCsvFileContent>>();
        }

        public void AddFile(IEnumerable<CovidStatisticCsvFileContent> fileContent)
        {
            var existingFileOfGivenType = _fileContents.FirstOrDefault(x => fileContent.GetType() == x.GetType());
            if (existingFileOfGivenType == null)
            {
                _fileContents.Add(fileContent);
            }
            else
            {
                throw new CovidStatisticsCsvContentMultipleContentsOfTheSameTypeException();
            }
        }
    }
}