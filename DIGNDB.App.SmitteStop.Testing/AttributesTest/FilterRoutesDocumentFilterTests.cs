using DIGNDB.App.SmitteStop.API.Attributes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Moq;
using NUnit.Framework;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DIGNDB.App.SmitteStop.Testing.AttributesTest
{
    [TestFixture]
    public class FilterRoutesDocumentFilterTests
    {
        private const string VersionPrefix = "/v";
        
        [Test]
        public void TestApply()
        {
            var filterRoutesDocumentFilter = new FilterRoutesDocumentFilter();
            var schemaGeneratorMock = new Mock<ISchemaGenerator>();

            var paths = new OpenApiPaths
            {
                {VersionPrefix + "someVersionedEndpoint", new OpenApiPathItem()},
                {VersionPrefix + "someOtherVersionedEndpoint", new OpenApiPathItem()},
                {"EndpointWithoutVersion", new OpenApiPathItem()}
            };

            var swaggerDoc = new OpenApiDocument
            {
                Paths = paths
            };
            var context = new DocumentFilterContext(new[] {new ApiDescription()}, schemaGeneratorMock.Object,
                new SchemaRepository());
            
            filterRoutesDocumentFilter.Apply(swaggerDoc,  context);

            swaggerDoc.Paths.Keys.Should().NotContain(key => !key.StartsWith(VersionPrefix));
        }
    }
}