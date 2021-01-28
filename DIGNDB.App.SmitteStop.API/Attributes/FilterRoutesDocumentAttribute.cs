﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace DIGNDB.App.SmitteStop.API.Attributes
{
    public class FilterRoutesDocumentFilter : IDocumentFilter {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            List<KeyValuePair<string, OpenApiPathItem>> pathList =
                swaggerDoc.Paths.Where((x) => !x.Key.StartsWith("/api/v")).ToList();

            if (pathList.Count > 0)
            {
                pathList.ForEach(result => {
                    swaggerDoc.Paths.Remove(result.Key);
                });
            }
        }
    }
}