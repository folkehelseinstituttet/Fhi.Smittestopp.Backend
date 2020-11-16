using System;
using System.Collections.Generic;
using System.Security.Principal;
using DIGNDB.App.SmitteStop.API.Services;
using DIGNDB.App.SmitteStop.Core.Contracts;
using DIGNDB.App.SmitteStop.Core.Enums;
using DIGNDB.App.SmitteStop.Core.Models;
using DIGNDB.App.SmitteStop.Core.Services;
using DIGNDB.App.SmitteStop.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.ExpressionBuilders.Logging;
using NUnit.Framework;
using Log = Moq.Contrib.ExpressionBuilders.Logging.Log;

namespace DIGNDB.App.SmitteStop.Testing.ServiceTest
{
    [TestFixture]
    public class DataAccessLoggingServiceTests
    {
        private Mock<ILogger<DataAccessLoggingServiceTests>> _loggerMock;
        private Mock<IIdentity> _identityMock;

        private IDataAccessLoggingService<DataAccessLoggingServiceTests> _service;

        private const string ExampleUserName = "Lasse";


        [SetUp]
        public void Init()
        {
            _loggerMock = new Mock<ILogger<DataAccessLoggingServiceTests>>();
            _identityMock = new Mock<IIdentity>(MockBehavior.Strict);

            _service = new DataAccessLoggingService<DataAccessLoggingServiceTests>(_loggerMock.Object);
        }

        [Test]
        public void TestLogDataAccess()
        {
            // Arrange
            var dataCollection = new List<ExampleEntity>
            {
                new ExampleEntity {Id = 1},
                new ExampleEntity {Id = 2},
                new ExampleEntity {Id = 3}
            };
            _identityMock.Setup(identity => identity.Name)
                .Returns(ExampleUserName);

            _loggerMock.Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<object>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));

            // Act
            _service.LogDataAccess(dataCollection, DataOperation.Create, _identityMock.Object);

            // Assert
            const string expectedLogMessage =
                "Operation: Create, User performing: Lasse, Data type: DIGNDB.App.SmitteStop.Testing.ServiceTest.DataAccessLoggingServiceTests+ExampleEntity, Data ids: 1, 2, 3.";
            _loggerMock.Verify(Log.With.LogLevel(LogLevel.Information)
                .And.EventId(0)
                .And.LogMessage(expectedLogMessage), Times.Once);
        }

        [Test]
        public void TestLogDataAccessEmptyCollection()
        {
            // Arrange
            var dataCollection = new List<ExampleEntity>();
            _identityMock.Setup(identity => identity.Name)
                .Returns(ExampleUserName);

            // Act
            _service.LogDataAccess(dataCollection, DataOperation.Create, _identityMock.Object);

            // Assert
            _loggerMock.Verify(
                x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()), Times.Never());
        }

        [Test]
        public void TestLogDataAccessWithNullIdentity()
        {
            // Arrange
            var dataCollection = new List<ExampleEntity>
            {
                new ExampleEntity {Id = 1},
                new ExampleEntity {Id = 2},
                new ExampleEntity {Id = 3}
            };
            _identityMock.Setup(identity => identity.Name)
                .Returns((string?) null);

            // Act
            _service.LogDataAccess(dataCollection, DataOperation.Create, _identityMock.Object);

            // Assert
            // Assert
            const string expectedLogMessage =
                "Operation: Create, Data type: DIGNDB.App.SmitteStop.Testing.ServiceTest.DataAccessLoggingServiceTests+ExampleEntity, Data ids: 1, 2, 3.";
            _loggerMock.Verify(Log.With.LogLevel(LogLevel.Information)
                .And.EventId(0)
                .And.LogMessage(expectedLogMessage), Times.Once);
        }

        private class ExampleEntity : IIdentifiedEntity<long>
        {
            public long Id { get; set; }
        }
    }
}
