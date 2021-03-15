using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.APP.SmitteStop.Jobs.Config;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    public class FetchKeyNumbersServiceTests
    {
        private readonly Mock<IFileSystem> _mockedFileSystem = new Mock<IFileSystem>();
        private GetCovidStatisticsJobConfig _config;

        [SetUp]
        public void setupConfig()
        {
            _config = new GetCovidStatisticsJobConfig()
            {
                MakeAlertIfDataIsMissingAfterHour = 16,
                CovidStatisticsFolder = "test"
            };
        }
        [Test]
        public async Task FetchTestNumbersFromDate_Correct_URL_Format()
        {
            _mockedFileSystem.Setup(x => x.GetFileStream(It.IsAny<string>())).Returns(
                    new MemoryStream(Encoding.UTF8.GetBytes("test"))
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(_mockedFileSystem.Object, _config);
            DateTime date = new DateTime(2021, 3, 1);
            string expectedUrl = $"{_config.CovidStatisticsFolder}/data_covid19_lab_by_time_2021-03-01.csv";
            service.FetchTestNumbersFromDate(date);

            _mockedFileSystem.Verify(service => service.GetFileStream(expectedUrl), Times.Once);
            _mockedFileSystem.Reset();
        }

        [Test]
        public async Task FetchHospitalNumbersFromDate_Correct_URL_Format()
        {
            _mockedFileSystem.Setup(service => service.GetFileStream(It.IsAny<string>())).Returns(
                new MemoryStream(Encoding.UTF8.GetBytes("test"))
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(_mockedFileSystem.Object, _config);
            DateTime date = new DateTime(2021, 3, 1);
            string expectedUrl = $"{_config.CovidStatisticsFolder}/data_covid19_hospital_by_time_2021-03-01.csv";
            service.FetchHospitalNumbersFromDate(date);

            _mockedFileSystem.Verify(service => service.GetFileStream(expectedUrl), Times.Once);
            _mockedFileSystem.Reset();
        }

        [Test]
        public async Task FetchVaccineNumbersFromDate_Correct_URL_Format()
        {
            _mockedFileSystem.Setup(service => service.GetFileStream(It.IsAny<string>())).Returns(
                new MemoryStream(Encoding.UTF8.GetBytes("test"))
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(_mockedFileSystem.Object, _config);
            DateTime date = new DateTime(2021, 3, 1);
            string expectedUrl = $"{_config.CovidStatisticsFolder}/data_covid19_sysvak_by_time_location_2021-03-01.csv";
            service.FetchVaccineNumbersFromDate(date);

            _mockedFileSystem.Verify(service => service.GetFileStream(expectedUrl), Times.Once);
            _mockedFileSystem.Reset();
        }

        [Test]
        public async Task FetchTestNumbersFromDate_Correct_Response()
        {
            string expectedString = "Test numbers from date";
            StringContent expectedContent = new StringContent(expectedString);
            _mockedFileSystem.Setup(service => service.GetFileStream(It.IsAny<string>())).Returns(
                new MemoryStream(Encoding.UTF8.GetBytes(expectedString))
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(_mockedFileSystem.Object, _config);
            DateTime date = new DateTime(2021, 3, 1);
            Stream response = service.FetchVaccineNumbersFromDate(date);
            string responseString = await new StreamReader(response).ReadToEndAsync();

            Assert.AreEqual(expectedString, responseString);
            _mockedFileSystem.Reset();
        }

        [Test]
        public async Task FetchHospitalNumbersFromDate_Correct_Response()
        {
            string expectedString = "Hospital numbers from date";
            StringContent expectedContent = new StringContent(expectedString);
            _mockedFileSystem.Setup(service => service.GetFileStream(It.IsAny<string>())).Returns(
                new MemoryStream(Encoding.UTF8.GetBytes(expectedString))
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(_mockedFileSystem.Object, _config);
            DateTime date = new DateTime(2021, 3, 1);
            Stream response = service.FetchHospitalNumbersFromDate(date);
            string responseString = await new StreamReader(response).ReadToEndAsync();

            Assert.AreEqual(expectedString, responseString);
            _mockedFileSystem.Reset();
        }

        [Test]
        public async Task FetchVaccineNumbersFromDate_Correct_Response()
        {
            string expectedString = "Vaccine numbers from date";
            StringContent expectedContent = new StringContent(expectedString);
            _mockedFileSystem.Setup(service => service.GetFileStream(It.IsAny<string>())).Returns(
                new MemoryStream(Encoding.UTF8.GetBytes(expectedString))
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(_mockedFileSystem.Object, _config);
            DateTime date = new DateTime(2021, 3, 1);
            Stream response = service.FetchVaccineNumbersFromDate(date);
            string responseString = await new StreamReader(response).ReadToEndAsync();

            Assert.AreEqual(expectedString, responseString);
            _mockedFileSystem.Reset();
        }
    }
}
