using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace DIGNDB.App.SmitteStop.API.Attributes
{
    public class RemoveVersionParameterAttribute : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if(!operation.Parameters.Any()) return;

            var versionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "version");
            if (versionParameter != null)
                operation.Parameters.Remove(versionParameter);
        }
    }
}