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
    public class ReplaceVersionWithExactValueInPathAttributeTests
    {
        private const string VersionTemplate = "v{version}";

        private const string VersionTestCase1 = "v1";
        private const string VersionTestCase2 = "v2";
        private const string VersionTestCase12 = "v1.2";

        private const string ExampleEndpoint1 = "/someExampleEndpoint";
        private const string ExampleEndpoint2 = "/someOtherExampleEndpoint";
        
        [TestCase(VersionTestCase1)]
        [TestCase(VersionTestCase2)]
        [TestCase(VersionTestCase12)]
        public void TestApplyWithParameters(string version)
        {
            var replaceVersionWithExactValueInPathAttribute = new ReplaceVersionWithExactValueInPathAttribute();
            var schemaGeneratorMock = new Mock<ISchemaGenerator>();
            
            var paths = new OpenApiPaths
            {
                {VersionTemplate + ExampleEndpoint1, new OpenApiPathItem()},
                {VersionTemplate + ExampleEndpoint2, new OpenApiPathItem()},
            };
            var replacedPaths = new OpenApiPaths
            {
                {version + ExampleEndpoint1, new OpenApiPathItem()},
                {version + ExampleEndpoint2, new OpenApiPathItem()},
            };

            var swaggerDoc = new OpenApiDocument
            {
                Paths = paths,
                Info = new OpenApiInfo {Version = version}
            };

            replaceVersionWithExactValueInPathAttribute.Apply(swaggerDoc,
                new DocumentFilterContext(new[] {new ApiDescription()}, schemaGeneratorMock.Object,
                    new SchemaRepository()));
            
            swaggerDoc.Paths.Should().BeEquivalentTo(replacedPaths);
        }
    }
}