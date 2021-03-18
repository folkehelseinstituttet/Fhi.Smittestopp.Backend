using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.CsvContentModels;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Exceptions;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Services;
using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Utils;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DIGNDB.App.SmitteStop.Testing.CovidStatisticsFilesTests.Services
{
    [TestFixture]
    public class CovidStatisticsBuilderTests
    {
        private MockRepository mockRepository;

        private Mock<IDateTimeResolver> _mockDateTimeResolver;
        private ICovidStatisticsCsvDataRetrieveService _covidStatisticsCsvDataRetrieveService;
        private static readonly DateTime DateTimeToday = new DateTime(2020, 10, 10, 10, 22, 20);
        private static readonly DateTime DateToday = DateTimeToday.Date;
        private static readonly DateTime DateYesterday = DateToday.AddDays(-1);
        private CovidStatisticsCsvContent _sampleCsvContent;
        private CovidStatistics _statisticsObjectThatMatchesSampleCsvContent;
        private List<TestedCsvContent> _sampleTestedCsvContent;
        private List<VaccinatedCsvContent> _sampleVaccinatedCsvContent;
        private List<HospitalCsvContent> _sampleHospitalCsvContent;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDateTimeResolver = mockRepository.Create<IDateTimeResolver>();
            _mockDateTimeResolver.Setup(x => x.GetDateTimeNow()).Returns(DateTimeToday);
            _mockDateTimeResolver.Setup(x => x.GetDateXDaysAgo(1)).Returns(DateToday);
            _mockDateTimeResolver.Setup(x => x.GetDateXDaysAgo(2)).Returns(DateYesterday);
            _covidStatisticsCsvDataRetrieveService =
                new CovidStatisticsCsvDataRetrieveService(_mockDateTimeResolver.Object);
            BuildSampleCsvContentAndCorrespondingStatisticsEntry();
        }

        private CovidStatisticsBuilder CreateCovidStatisticsBuilder()
        {
            return new CovidStatisticsBuilder(
                _mockDateTimeResolver.Object,
                _covidStatisticsCsvDataRetrieveService);
        }

        private void BuildSampleCsvContentAndCorrespondingStatisticsEntry()
        {
            _sampleTestedCsvContent = new List<TestedCsvContent>()
            {
                new TestedCsvContent()
                {
                    Date = DateYesterday,
                    Negative = 1,
                    Positive = 2
                },
                new TestedCsvContent()
                {
                    Date = DateToday,
                    Negative = 10,
                    Positive = 20
                }
            };

            _sampleHospitalCsvContent = new List<HospitalCsvContent>()
            {
                new HospitalCsvContent()
                {
                    Date = DateYesterday,
                    HospitalAdmitted = 3,
                    IcuPatients = 4
                },
                new HospitalCsvContent()
                {
                    Date = DateToday,
                    HospitalAdmitted = 30,
                    IcuPatients = 40
                }
            };

            _sampleVaccinatedCsvContent = new List<VaccinatedCsvContent>()
            {
                new VaccinatedCsvContent()
                {
                    Date = DateYesterday,
                    FirstDose = 0.1,
                    SecondDose = 0.2,
                    Region = VaccinatedCsvContent.NorwayRegionName
                },
                new VaccinatedCsvContent()
                {
                    Date = DateToday,
                    FirstDose = 0.4,
                    SecondDose = 0.6,
                    Region = VaccinatedCsvContent.NorwayRegionName
                },
                new VaccinatedCsvContent()
                {
                    Date = DateToday,
                    FirstDose = 0.111,
                    SecondDose = 0.222,
                    Region = "otherRegionShouldBeIgnored"
                },
            };

            BuildSampleCsvContentFromSampleFilesAndCorrespondingStatisticsEntry();
        }

        private void BuildSampleCsvContentFromSampleFilesAndCorrespondingStatisticsEntry()
        {
            _sampleCsvContent = new CovidStatisticsCsvContent();
            _sampleCsvContent.AddFile(_sampleTestedCsvContent);
            _sampleCsvContent.AddFile(_sampleHospitalCsvContent);
            _sampleCsvContent.AddFile(_sampleVaccinatedCsvContent);
            BuildStatisticsEntryFromSampleCsvContent();
        }

        private void BuildStatisticsEntryFromSampleCsvContent()
        {
            _statisticsObjectThatMatchesSampleCsvContent = new CovidStatistics
            {
                Date = DateTimeToday,
                ConfirmedCasesToday = _sampleTestedCsvContent[1].Positive,
                ConfirmedCasesTotal = _sampleTestedCsvContent[1].Positive + _sampleTestedCsvContent[0].Positive,
                IcuAdmittedToday = _sampleHospitalCsvContent[1].IcuPatients,
                PatientsAdmittedToday = _sampleHospitalCsvContent[1].HospitalAdmitted,
                TestsConductedToday = _sampleTestedCsvContent[1].Positive + _sampleTestedCsvContent[1].Negative,
                TestsConductedTotal = _sampleTestedCsvContent[1].Positive + _sampleTestedCsvContent[1].Negative +
                                      _sampleTestedCsvContent[0].Positive + _sampleTestedCsvContent[0].Negative,
                VaccinatedFirstDoseToday = _sampleVaccinatedCsvContent[1].FirstDose - _sampleVaccinatedCsvContent[0].FirstDose,
                VaccinatedFirstDoseTotal = _sampleVaccinatedCsvContent[1].FirstDose,
                VaccinatedSecondDoseToday = _sampleVaccinatedCsvContent[1].SecondDose - _sampleVaccinatedCsvContent[0].SecondDose,
                VaccinatedSecondDoseTotal = _sampleVaccinatedCsvContent[1].SecondDose
            };
        }

        [Ignore("Awaits clarification 2021-03-17")]
        [Test]
        public void BuildStatistics_CorrectData_ShouldCalculateStatisticsRight()
        {
            // Arrange
            var covidStatisticsBuilder = CreateCovidStatisticsBuilder();
            // Act
            var result = covidStatisticsBuilder.BuildStatistics(
                _sampleCsvContent);

            // Assert
            Assert.IsTrue(_statisticsObjectThatMatchesSampleCsvContent.Equals(result));
        }

        [Test]
        public void BuildStatistics_FileMissing_ShouldThrowAppropriateException()
        {
            // Arrange
            _sampleCsvContent = new CovidStatisticsCsvContent();
            _sampleCsvContent.AddFile(_sampleTestedCsvContent);
            var covidStatisticsBuilder = CreateCovidStatisticsBuilder();
            // Act

            // Assert
            Assert.Throws<CovidStatisticsCsvDataPartiallyMissingException>(() => covidStatisticsBuilder.BuildStatistics(_sampleCsvContent));
        }

        [Test]
        public void BuildStatistics_DataNotUnique_ShouldThrowAppropriateException()
        {
            // Arrange
            _sampleHospitalCsvContent[0].Date = DateToday;
            var covidStatisticsBuilder = CreateCovidStatisticsBuilder();
            // Act

            // Assert
            Assert.Throws<CovidStatisticsCsvDataNotUniqueException>(() => covidStatisticsBuilder.BuildStatistics(_sampleCsvContent));
        }

        [Test]
        public void BuildStatistics_DataMissingForToday_ShouldThrowAppropriateException()
        {
            // Arrange
            _sampleHospitalCsvContent[1].Date = DateToday.AddDays(-2);
            var covidStatisticsBuilder = CreateCovidStatisticsBuilder();
            // Act

            // Assert
            Assert.Throws<CovidStatisticsCsvDataPartiallyMissingException>(() => covidStatisticsBuilder.BuildStatistics(_sampleCsvContent));
        }
    }
}

