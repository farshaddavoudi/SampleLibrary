using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ATA.Library.Server.Api.Infrastructure.Swagger.DocumentFilters
{
    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(
                    !path.Key.StartsWith("/odata/")
                        ? path.Key.Replace("v{version}", swaggerDoc.Info.Version)
                        : path.Key, path.Value);
            }
            swaggerDoc.Paths = paths;
        }
    }
}