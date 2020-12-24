using ATA.Library.Shared.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;

namespace ATA.Library.Server.Api.Infrastructure.Swagger
{
    public static class SwaggerMiddlewares
    {
        // ApplicationBuilder
        public static void UseSwaggerAndUI(this IApplicationBuilder app, IHostEnvironment env)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            #region Swagger middleware for generate "Open API Documentation" in swagger.json
            // Api Endpoints
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api/swagger/{documentname}/swagger.json";
                c.PreSerializeFilters.Add(FilterOnlyApiControllers);
            });

            // OData Endpoints
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "odata/swagger/{documentname}/swagger.json";
                c.PreSerializeFilters.Add(FilterOnlyODataControllers);

            });

            #endregion

            #region Swagger middleware for generate UI from swagger.json
            // Api Endpoints
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api/swagger";
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "V1 Doc");
                c.SwaggerEndpoint("/api/swagger/v2/swagger.json", "V2 Doc");
                c.DocumentTitle = $"API Swagger {AppStrings.AppEnglishFullName}";
                c.DocExpansion(DocExpansion.None);
            });

            // OData Endpoints
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "odata/swagger";
                c.SwaggerEndpoint("/odata/swagger/v1/swagger.json", "V1 Doc");
                c.DocumentTitle = $"OData Swagger {AppStrings.AppEnglishFullName}";
            });
            #endregion
        }

        private static void FilterOnlyApiControllers(OpenApiDocument swaggerDoc, HttpRequest req)
        {
            // Filter Paths
            var apiPaths = swaggerDoc.Paths.Where(p => p.Key.StartsWith("/api/"));
            var paths = new OpenApiPaths();
            foreach (var path in apiPaths)
                paths.Add(path.Key, path.Value);
            swaggerDoc.Paths = paths;

            // Filter XmlComments
            var allTags = swaggerDoc.Tags;
            var tags = allTags.Where(tag => tag.Reference.ExternalResource != "odata").ToList();
            swaggerDoc.Tags = tags;
        }

        private static void FilterOnlyODataControllers(OpenApiDocument swaggerDoc, HttpRequest req)
        {
            // Filter Paths
            var apiPaths = swaggerDoc.Paths.Where(p => p.Key.StartsWith("/odata/"));
            var paths = new OpenApiPaths();
            foreach (var path in apiPaths)
                paths.Add(path.Key, path.Value);
            swaggerDoc.Paths = paths;

            // Filter XmlComments
            var allTags = swaggerDoc.Tags;
            var tags = allTags.Where(tag => tag.Reference.ExternalResource == "odata").ToList();
            swaggerDoc.Tags = tags;
        }

    }
}