using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Testing.CovidStatisticsFilesTests.CsvContentModels
{
    [TestFixture]
    public class CovidStatisticsCsvContentTests
    {
        private CovidStatisticsCsvContent CreateCovidStatisticsCsvContent()
        {
            return new CovidStatisticsCsvContent();
        }

        [Test]
        public void AddFile_Add2FilesDifferentTypes_ShouldContain2Files()
        {
            // Arrange
            var covidStatisticsCsvContent = CreateCovidStatisticsCsvContent();
            List<TestedCsvContent> fileContent = new List<TestedCsvContent>
            {
                new TestedCsvContent(),
            };

            var fileContent2 = new List<HospitalCsvContent>
            {
                new HospitalCsvContent()
            };

            // Act
            covidStatisticsCsvContent.AddFileContent(
                fileContent);
            covidStatisticsCsvContent.AddFileContent(
                fileContent2);

            // Assert
            Assert.AreEqual(covidStatisticsCsvContent.FileContents.Count, 2);
        }

        [Test]
        public void AddFile_Add2FilesSameTypes_ShouldThrowError()
        {
            // Arrange
            var covidStatisticsCsvContent = CreateCovidStatisticsCsvContent();
            List<TestedCsvContent> fileContent = new List<TestedCsvContent>
            {
                new TestedCsvContent(),
            };

            var fileContent2 = new List<TestedCsvContent>
            {
                new TestedCsvContent()
            };

            // Act
            covidStatisticsCsvContent.AddFileContent(
                fileContent);

            // Assert
            Assert.Throws<CovidStatisticsCsvContentMultipleContentsOfTheSameTypeException>(() => covidStatisticsCsvContent.AddFileContent(fileContent2));
        }
    }
}
