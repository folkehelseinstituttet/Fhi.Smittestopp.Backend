using DIGNDB.App.SmitteStop.Domain.Db;
using DIGNDB.APP.SmitteStop.Jobs.Config;
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
        private int _extraTestsTotal = 20000;

        private MockRepository mockRepository;

        private Mock<IDateTimeResolver> _mockDateTimeResolver;
        private ICovidStatisticsCsvDataRetrieveService _covidStatisticsCsvDataRetrieveService;
        private GetCovidStatisticsJobConfig _config;
        private static readonly DateTime DateTimeToday = new DateTime(2020, 10, 10, 10, 22, 20);
        private static readonly DateTime DateToday = DateTimeToday.Date;
        private static readonly DateTime DateYesterday = DateToday.AddDays(-1);
        private CovidStatisticsCsvContent _sampleCsvContent;
        private CovidStatistics _statisticsObjectThatMatchesSampleCsvContent;
        private List<TestedCsvContent> _sampleTestedCsvContent;
        private List<VaccinatedCsvContent> _sampleVaccinatedCsvContent;
        private List<HospitalCsvContent> _sampleHospitalCsvContent;
        private List<TimeLocationCsvContent> _sampleConfirmedCasesTodayCsvContent;
        private List<LocationCsvContent> _sampleConfirmedCasesTotalCsvContent;
        private List<DeathsCsvContent> _sampleDeathcsCasesTotalCsvContent;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);

            _mockDateTimeResolver = mockRepository.Create<IDateTimeResolver>();
            _mockDateTimeResolver.Setup(x => x.GetDateTime()).Returns(DateTimeToday);
            _mockDateTimeResolver.Setup(x => x.GetDateXDaysAgo(1)).Returns(DateToday);
            _mockDateTimeResolver.Setup(x => x.GetDateXDaysAgo(2)).Returns(DateYesterday);
            
            _covidStatisticsCsvDataRetrieveService = new CovidStatisticsCsvDataRetrieveService(_mockDateTimeResolver.Object);

            BuildSampleCsvContentAndCorrespondingStatisticsEntry();
        }

        [SetUp]
        public void SetupConfig()
        {
            _config = new GetCovidStatisticsJobConfig()
            {
                MakeAlertIfDataIsMissingAfterHour = 16,
                CovidStatisticsFolder = "test",
                TestsConductedTotalExtra = 20000
            };
        }

        private CovidStatisticsBuilder CreateCovidStatisticsBuilder()
        {
            return new CovidStatisticsBuilder(_mockDateTimeResolver.Object, _covidStatisticsCsvDataRetrieveService, _config);
        }

        private void BuildSampleCsvContentAndCorrespondingStatisticsEntry()
        {
            _sampleTestedCsvContent = new List<TestedCsvContent>
            {
                new TestedCsvContent
                {
                    Date = DateYesterday,
                    Negative = 1,
                    Positive = 2
                },
                new TestedCsvContent
                {
                    Date = DateToday,
                    Negative = 10,
                    Positive = 20
                }
            };

            _sampleHospitalCsvContent = new List<HospitalCsvContent>
            {
                new HospitalCsvContent
                {
                    Date = DateYesterday,
                    HospitalAdmitted = 3,
                    IcuPatients = 4,
                },
                new HospitalCsvContent
                {
                    Date = DateToday,
                    HospitalAdmitted = 30,
                    IcuPatients = 40,
                }
            };

            _sampleVaccinatedCsvContent = new List<VaccinatedCsvContent>
            {
                new VaccinatedCsvContent
                {
                    Date = DateToday,
                    FirstDose = 3,
                    FirstDoseTotal = 4,
                    SecondDose = 1,
                    SecondDoseTotal = 2,
                    Region = VaccinatedCsvContent.NorwayRegionName
                },
                new VaccinatedCsvContent
                {
                    Date = DateYesterday,
                    FirstDose = 3,
                    SecondDose = 4,
                    Region = VaccinatedCsvContent.NorwayRegionName
                },
                new VaccinatedCsvContent
                {
                    Date = DateToday,
                    FirstDose = 5,
                    SecondDose = 6,
                    Region = "otherRegionShouldBeIgnored"
                },
            };

            _sampleConfirmedCasesTodayCsvContent = new List<TimeLocationCsvContent>
            {
                new TimeLocationCsvContent
                {
                    Date = DateToday,
                    ConfirmedCasesToday = 7,
                    Region = "norge",
                }
            };

            _sampleConfirmedCasesTotalCsvContent = new List<LocationCsvContent>
            {
                new LocationCsvContent
                {
                    Date = new DateTime(1900, 01, 01),
                    ConfirmedCasesTotal = 14,
                    Region = "norge",
                }
            };

            _sampleDeathcsCasesTotalCsvContent = new List<DeathsCsvContent>
            {
                new DeathsCsvContent
                {
                    Date = new DateTime(1900, 01, 01),
                    Age = "age",
                    Region = "norge",
                    DeathsCasesTotal = 666
                }
            };

            BuildSampleCsvContentFromSampleFilesAndCorrespondingStatisticsEntry();
        }

        private void BuildSampleCsvContentFromSampleFilesAndCorrespondingStatisticsEntry()
        {
            _sampleCsvContent = new CovidStatisticsCsvContent();

            _sampleCsvContent.AddFileContent(_sampleTestedCsvContent);
            _sampleCsvContent.AddFileContent(_sampleHospitalCsvContent);
            _sampleCsvContent.AddFileContent(_sampleVaccinatedCsvContent);
            _sampleCsvContent.AddFileContent(_sampleConfirmedCasesTodayCsvContent);
            _sampleCsvContent.AddFileContent(_sampleConfirmedCasesTotalCsvContent);
            _sampleCsvContent.AddFileContent(_sampleDeathcsCasesTotalCsvContent);

            BuildStatisticsEntryFromSampleCsvContent();
        }

        private void BuildStatisticsEntryFromSampleCsvContent()
        {
            _statisticsObjectThatMatchesSampleCsvContent = new CovidStatistics
            {
                ConfirmedCasesToday = _sampleConfirmedCasesTodayCsvContent[0].ConfirmedCasesToday,
                ConfirmedCasesTotal = _sampleConfirmedCasesTotalCsvContent[0].ConfirmedCasesTotal,

                IcuAdmittedToday = _sampleHospitalCsvContent[1].IcuPatients,
                IcuAdmittedTotal = _sampleHospitalCsvContent[0].IcuPatients +
                                   _sampleHospitalCsvContent[1].IcuPatients,
                PatientsAdmittedToday = _sampleHospitalCsvContent[1].HospitalAdmitted,
                PatientsAdmittedTotal = _sampleHospitalCsvContent[0].HospitalAdmitted + 
                                        _sampleHospitalCsvContent[1].HospitalAdmitted,

                TestsConductedToday = _sampleTestedCsvContent[1].Positive + _sampleTestedCsvContent[1].Negative,
                TestsConductedTotal = _sampleTestedCsvContent[1].Positive + _sampleTestedCsvContent[1].Negative +
                                      _sampleTestedCsvContent[0].Positive + _sampleTestedCsvContent[0].Negative + _extraTestsTotal,
                
                VaccinatedFirstDoseToday = _sampleVaccinatedCsvContent[0].FirstDose,
                VaccinatedFirstDoseTotal = _sampleVaccinatedCsvContent[0].FirstDoseTotal,
                VaccinatedSecondDoseToday = _sampleVaccinatedCsvContent[0].SecondDose,
                VaccinatedSecondDoseTotal = _sampleVaccinatedCsvContent[0].SecondDoseTotal,

                DeathsCasesTotal = _sampleDeathcsCasesTotalCsvContent[0].DeathsCasesTotal,
                
                ModificationDate = DateTimeToday,
                EntryDate = DateTimeToday.Date,
            };
        }

        [Test]
        public void BuildStatistics_CorrectData_ShouldCalculateStatisticsRight()
        {
            // Arrange
            var covidStatisticsBuilder = CreateCovidStatisticsBuilder();

            // Act
            var result = covidStatisticsBuilder.BuildStatistics(_sampleCsvContent);
            
            _statisticsObjectThatMatchesSampleCsvContent.ModificationDate = result.ModificationDate;

            // Assert
            Assert.IsTrue(_statisticsObjectThatMatchesSampleCsvContent.Equals(result));
        }

        [Test]
        public void BuildStatistics_FileMissing_ShouldThrowAppropriateException()
        {
            // Arrange
            _sampleCsvContent = new CovidStatisticsCsvContent();
            _sampleCsvContent.AddFileContent(_sampleConfirmedCasesTodayCsvContent);
            _sampleCsvContent.AddFileContent(_sampleConfirmedCasesTotalCsvContent);
            _sampleCsvContent.AddFileContent(_sampleTestedCsvContent);
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

