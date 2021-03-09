using DIGNDB.APP.SmitteStop.Jobs.CovidStatistics.Services;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    public class FetchKeyNumbersServiceTests
    {
        private readonly Mock<IWebServiceWrapper> mockedWebService = new Mock<IWebServiceWrapper>();

        [Test]
        public async Task FetchTestNumbersFromDate_Correct_URL_Format()
        {
            mockedWebService.Setup(service => service.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = new StringContent("test")
                    }
                )
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(mockedWebService.Object);
            DateTime date = new DateTime(2021, 3, 1);
            string expectedUrl = "https://raw.githubusercontent.com/folkehelseinstituttet/surveillance_data/master/covid19/data_covid19_lab_by_time_2021-03-01.csv";
            await service.FetchTestNumbersFromDate(date);

            mockedWebService.Verify(service => service.GetAsync(expectedUrl), Times.Once);
            mockedWebService.Reset();
        }

        [Test]
        public async Task FetchHospitalNumbersFromDate_Correct_URL_Format()
        {
            mockedWebService.Setup(service => service.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = new StringContent("test")
                    }
                )
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(mockedWebService.Object);
            DateTime date = new DateTime(2021, 3, 1);
            string expectedUrl = "https://raw.githubusercontent.com/folkehelseinstituttet/surveillance_data/master/covid19/data_covid19_hospital_by_time_2021-03-01.csv";
            await service.FetchHospitalNumbersFromDate(date);

            mockedWebService.Verify(service => service.GetAsync(expectedUrl), Times.Once);
            mockedWebService.Reset();
        }

        [Test]
        public async Task FetchVaccineNumbersFromDate_Correct_URL_Format()
        {
            mockedWebService.Setup(service => service.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = new StringContent("test")
                    }
                )
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(mockedWebService.Object);
            DateTime date = new DateTime(2021, 3, 1);
            string expectedUrl = "https://raw.githubusercontent.com/folkehelseinstituttet/surveillance_data/master/covid19/data_covid19_sysvak_by_time_location_2021-03-01.csv";
            await service.FetchVaccineNumbersFromDate(date);

            mockedWebService.Verify(service => service.GetAsync(expectedUrl), Times.Once);
            mockedWebService.Reset();
        }

        [Test]
        public async Task FetchLatestTestNumbers_Correct_URL_Format()
        {
            mockedWebService.Setup(service => service.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = new StringContent("test")
                    }
                )
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(mockedWebService.Object);
            string expectedUrl = "https://raw.githubusercontent.com/folkehelseinstituttet/surveillance_data/master/covid19/data_covid19_lab_by_time_latest.csv";
            await service.FetchLatestTestNumbers();

            mockedWebService.Verify(service => service.GetAsync(expectedUrl), Times.Once);
            mockedWebService.Reset();
        }

        [Test]
        public async Task FetchLatestHospitalNumbers_Correct_URL_Format()
        {
            mockedWebService.Setup(service => service.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = new StringContent("test")
                    }
                )
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(mockedWebService.Object);
            string expectedUrl = "https://raw.githubusercontent.com/folkehelseinstituttet/surveillance_data/master/covid19/data_covid19_hospital_by_time_latest.csv";
            await service.FetchLatestHospitalNumbers();

            mockedWebService.Verify(service => service.GetAsync(expectedUrl), Times.Once);
            mockedWebService.Reset();
        }

        [Test]
        public async Task FetchLatestVaccineNumbers_Correct_URL_Format()
        {
            mockedWebService.Setup(service => service.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = new StringContent("test")
                    }
                )
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(mockedWebService.Object);
            string expectedUrl = "https://raw.githubusercontent.com/folkehelseinstituttet/surveillance_data/master/covid19/data_covid19_sysvak_by_time_location_latest.csv";
            await service.FetchLatestVaccineNumbers();

            mockedWebService.Verify(service => service.GetAsync(expectedUrl), Times.Once);
            mockedWebService.Reset();
        }

        [Test]
        public async Task FetchTestNumbersFromDate_Correct_Response()
        {
            string expectedString = "Test numbers from date";
            StringContent expectedContent = new StringContent(expectedString);
            mockedWebService.Setup(service => service.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = expectedContent
                    }
                )
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(mockedWebService.Object);
            DateTime date = new DateTime(2021, 3, 1);
            Stream response = await service.FetchVaccineNumbersFromDate(date);
            string responseString = await new StreamReader(response).ReadToEndAsync();

            Assert.AreEqual(expectedString, responseString);
            mockedWebService.Reset();
        }

        [Test]
        public async Task FetchHospitalNumbersFromDate_Correct_Response()
        {
            string expectedString = "Hospital numbers from date";
            StringContent expectedContent = new StringContent(expectedString);
            mockedWebService.Setup(service => service.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = expectedContent
                    }
                )
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(mockedWebService.Object);
            DateTime date = new DateTime(2021, 3, 1);
            Stream response = await service.FetchHospitalNumbersFromDate(date);
            string responseString = await new StreamReader(response).ReadToEndAsync();

            Assert.AreEqual(expectedString, responseString);
            mockedWebService.Reset();
        }

        [Test]
        public async Task FetchVaccineNumbersFromDate_Correct_Response()
        {
            string expectedString = "Vaccine numbers from date";
            StringContent expectedContent = new StringContent(expectedString);
            mockedWebService.Setup(service => service.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = expectedContent
                    }
                )
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(mockedWebService.Object);
            DateTime date = new DateTime(2021, 3, 1);
            Stream response = await service.FetchVaccineNumbersFromDate(date);
            string responseString = await new StreamReader(response).ReadToEndAsync();

            Assert.AreEqual(expectedString, responseString);
            mockedWebService.Reset();
        }

        [Test]
        public async Task FetchLatestTestNumbers_Correct_Response()
        {
            string expectedString = "Latest test numbers";
            StringContent expectedContent = new StringContent(expectedString);
            mockedWebService.Setup(service => service.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = expectedContent
                    }
                )
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(mockedWebService.Object);
            Stream response = await service.FetchLatestTestNumbers();
            string responseString = await new StreamReader(response).ReadToEndAsync();

            Assert.AreEqual(expectedString, responseString);
            mockedWebService.Reset();
        }

        [Test]
        public async Task FetchLatestHospitalNumbers_Correct_Response()
        {
            string expectedString = "Latest hospital numbers";
            StringContent expectedContent = new StringContent(expectedString);
            mockedWebService.Setup(service => service.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = expectedContent
                    }
                )
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(mockedWebService.Object);
            Stream response = await service.FetchLatestHospitalNumbers();
            string responseString = await new StreamReader(response).ReadToEndAsync();

            Assert.AreEqual(expectedString, responseString);
            mockedWebService.Reset();
        }

        [Test]
        public async Task FetchLatestVaccineNumbers_Correct_Response()
        {
            string expectedString = "Latest vaccine numbers";
            StringContent expectedContent = new StringContent(expectedString);
            mockedWebService.Setup(service => service.GetAsync(It.IsAny<string>())).Returns(
                Task.FromResult(
                    new HttpResponseMessage
                    {
                        Content = expectedContent
                    }
                )
            );

            FetchCovidStatisticsService service = new FetchCovidStatisticsService(mockedWebService.Object);
            Stream response = await service.FetchLatestVaccineNumbers();
            string responseString = await new StreamReader(response).ReadToEndAsync();

            Assert.AreEqual(expectedString, responseString);
            mockedWebService.Reset();
        }
    }
}
