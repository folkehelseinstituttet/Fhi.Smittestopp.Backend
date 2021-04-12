using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using Moq;
using NUnit.Framework;
using System.IO;

namespace DIGNDB.App.SmitteStop.Testing.CovidStatisticsFilesTests.Services
{
    [TestFixture]
    public class StatisticsFileServiceTests
    {
        private IStatisticsFileService _statisticsFileService;
        private readonly Mock<IFileSystem> _mockedFileSystem = new Mock<IFileSystem>();

        private const string LocationFileNameToday = "data_covid19_msis_by_location_2021-04-09.csv";
        private const string TimeLocationFileNameToday = "data_covid19_msis_by_time_location_2021-04-09.csv";

        private readonly string[] _fileNames =
        {
            "data_covid19_msis_by_location_2021-04-09.csv", 
            "data_covid19_msis_by_location_2021-04-08.csv",
            "data_covid19_msis_by_location_2021-04-07.csv", 
            "data_covid19_msis_by_location_2021-04-06.csv",
            "data_covid19_msis_by_location_2021-04-05.csv", 
            "data_covid19_msis_by_location_2021-04-04.csv",
            "data_covid19_msis_by_location_2021-04-03.csv",
            "data_covid19_msis_by_location_2021-04-02.csv",
            "data_covid19_msis_by_location_2021-04-01.csv",
            "data_covid19_msis_by_location_2021-03-31.csv",
            "data_covid19_msis_by_location_2021-03-30.csv",
        };

        private readonly string[] _fileNamesDifferent =
        {
            "data_covid19_msis_by_location_2021-04-09.csv",
            "data_covid19_msis_by_time_location_2021-04-09.csv",
            "data_covid19_sysvak_by_time_location_2021-04-09.csv",
            "data_covid19_hospital_by_time_2021-04-09.csv",
            "data_covid19_lab_by_time_2021-04-09.csv",

            "data_covid19_msis_by_location_2021-04-08.csv",
            "data_covid19_msis_by_time_location_2021-04-08.csv",
            "data_covid19_sysvak_by_time_location_2021-04-08.csv",
            "data_covid19_hospital_by_time_2021-04-08.csv",
            "data_covid19_lab_by_time_2021-04-08.csv",
        };

        private const string Folder = "FakeDrive:\\fakePath\\GitHubStatisticsZipFiles";
        private const string FileNameDatePattern = "[0-9]{4}-[0-9]{2}-[0-9]{2}";
        private const string FileNameDateParsingFormat = "yyyy-MM-dd";

        [SetUp]
        public void SetUp()
        {
            _statisticsFileService = new StatisticsFileService(_mockedFileSystem.Object);
        }

        [Test]
        public void DeleteOldFiles_FilesToDelete_ReturnsCorrectCount()
        {
            // Arrange
            _mockedFileSystem.Setup(f => f.DeleteFile(It.IsAny<string>())).Verifiable();
            _mockedFileSystem.Setup(f => f.GetFileNamesFromDirectory(Folder)).Returns(_fileNames);
            const int days = 2;
            var expected = _fileNames.Length - days;

            // Act
            var path = Path.Join(Folder, LocationFileNameToday);
            var deletedFilesCount = _statisticsFileService.DeleteOldFiles(path, days, FileNameDatePattern, FileNameDateParsingFormat);
            
            // Assert
            Assert.That(deletedFilesCount, Is.EqualTo(expected));
        }

        [Test]
        public void DeleteOldFiles_NoFilesToDelete_ReturnsCorrectCount()
        {
            // Arrange
            _mockedFileSystem.Setup(f => f.DeleteFile(It.IsAny<string>())).Verifiable();
            _mockedFileSystem.Setup(f => f.GetFileNamesFromDirectory(Folder)).Returns(_fileNames);
            const int days = 6;
            var expected = _fileNames.Length - days;

            // Act
            var path = Path.Join(Folder, LocationFileNameToday);
            var deletedFilesCount = _statisticsFileService.DeleteOldFiles(path, days, FileNameDatePattern, FileNameDateParsingFormat);

            // Assert
            Assert.That(deletedFilesCount, Is.EqualTo(expected));
        }

        [Test]
        public void DeleteOldFiles_DifferentFilesToDelete_ReturnsCorrectCount()
        {
            // Arrange
            _mockedFileSystem.Setup(f => f.DeleteFile(It.IsAny<string>())).Verifiable();
            _mockedFileSystem.Setup(f => f.GetFileNamesFromDirectory(Folder)).Returns(_fileNamesDifferent);
            const int days = 1;
            var expected = 1;

            // Act
            var path = Path.Join(Folder, TimeLocationFileNameToday);
            var deletedFilesCount = _statisticsFileService.DeleteOldFiles(path, days, FileNameDatePattern, FileNameDateParsingFormat);

            // Assert
            Assert.That(deletedFilesCount, Is.EqualTo(expected));
        }
    }
}
