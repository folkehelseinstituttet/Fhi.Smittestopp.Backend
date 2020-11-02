using System.Collections.Generic;
using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DIGNDB.App.SmitteStop.API.Attributes
{
    public class FilterRoutesDocumentFilter : IDocumentFilter {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            List<KeyValuePair<string, OpenApiPathItem>> pathList =
                swaggerDoc.Paths.Where((x) => !x.Key.StartsWith("/v")).ToList();

            if (pathList.Count > 0)
            {
                pathList.ForEach(result => {
                    swaggerDoc.Paths.Remove(result.Key);
                });
            }
        }
    }
}