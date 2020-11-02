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
    public class RemoveVersionParameterAttributeTests
    {
        private const string VersionParameterName = "version";
        
        [Test]
        public void TestApplyWithParameters()
        {
            var removeVersionParameterAttribute = new RemoveVersionParameterAttribute();
            var schemaGeneratorMock = new Mock<ISchemaGenerator>();

            var openApiOperation = new OpenApiOperation();
            openApiOperation.Parameters.Add(new OpenApiParameter
            {
                Name = VersionParameterName
            });
            openApiOperation.Parameters.Add(new OpenApiParameter
            {
                Name = "someOtherJunk"
            });

            removeVersionParameterAttribute.Apply(openApiOperation,
                new OperationFilterContext(new ApiDescription(), schemaGeneratorMock.Object,
                    new SchemaRepository(), null));

            openApiOperation.Parameters.Should().NotContain(parameter => parameter.Name == VersionParameterName);
        }
        
        [Test]
        public void TestApplyWithNoParameters()
        {
            var removeVersionParameterAttribute = new RemoveVersionParameterAttribute();
            var schemaGeneratorMock = new Mock<ISchemaGenerator>();

            var openApiOperation = new OpenApiOperation();

            removeVersionParameterAttribute.Apply(openApiOperation,
                new OperationFilterContext(new ApiDescription(), schemaGeneratorMock.Object,
                    new SchemaRepository(), null));

            openApiOperation.Parameters.Should().NotContain(parameter => parameter.Name == VersionParameterName);
        }
    }
}