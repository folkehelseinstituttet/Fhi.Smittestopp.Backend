using DIGNDB.APP.SmitteStop.Jobs.CovidStatisticsFiles.Utils;
using NUnit.Framework;
using System;

namespace DIGNDB.App.SmitteStop.Testing.CovidStatisticsFilesTests.Utils
{
    [TestFixture]
    public class DateTimeResolverTests
    {
        private readonly DateTime _sampleDateTime = new DateTime(2020, 12, 5, 10, 22, 33);
        private DateTimeResolver CreateDateTimeResolver()
        {
            return new DateTimeResolver();
        }

        [Test]
        public void GetDateXDaysAgo_SetDateThenGetDateYesterday_ShouldReturnCorrectDate()
        {
            // Arrange
            var dateTimeResolver = CreateDateTimeResolver();

            // Act
            dateTimeResolver.SetDateTime(_sampleDateTime);
            var result = dateTimeResolver.GetDateXDaysAgo(1);

            // Assert
            Assert.AreEqual(_sampleDateTime.Date.AddDays(-1), result);
        }

        [Test]
        public void GetDateToday_SetDateTimeToThenGetDateTime_ShouldReturnCorrectDateTime()
        {
            // Arrange
            var dateTimeResolver = CreateDateTimeResolver();

            // Act
            dateTimeResolver.SetDateTime(_sampleDateTime);
            var result = dateTimeResolver.GetDateToday();

            // Assert
            Assert.AreEqual(_sampleDateTime.Date, result);
        }

        [Test]
        public void GetDateTimeNow_SetDateTimeToThenGetDateTime_ShouldReturnCorrectDateTime()
        {
            // Arrange
            var dateTimeResolver = CreateDateTimeResolver();
            // Act
            dateTimeResolver.SetDateTime(_sampleDateTime);
            var result = dateTimeResolver.GetDateTimeNow();

            // Assert
            Assert.AreEqual(_sampleDateTime, result);
        }
    }
}
