using ATA.Library.Server.Api.Controllers;
using ATA.Library.Server.Api.Infrastructure.Swagger.DocumentFilters;
using ATA.Library.Server.Api.Infrastructure.Swagger.OperationFilters;
using ATA.Library.Server.Model.AppSettings;
using ATA.Library.Shared.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ATA.Library.Server.Api.Infrastructure.Swagger
{
    public class SwaggerInstaller : IServiceInstaller
    {
        public void InstallServices(IServiceCollection services, AppSettings appSettings, IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            //Add services to use Example Filters in swagger
            //services.AddSwaggerExamples();

            //Add services and configuration to use swagger
            services.AddSwaggerGen(options =>
            {
                //Collect all referenced projects output XML document file paths  
                var apiAssembly = Assembly.GetExecutingAssembly();
                var xmlDocs = apiAssembly.GetReferencedAssemblies()
                    .Union(new AssemblyName[] { apiAssembly.GetName() })
                    .Select(a => Path.Combine(Path.GetDirectoryName(apiAssembly.Location) ?? string.Empty, $"{a.Name}.xml"))
                    .Where(File.Exists).ToArray();

                Array.ForEach(xmlDocs, (d) =>
                {
                    options.IncludeXmlComments(d, true);
                });
                //var xmlDocPath = Path.Combine(AppContext.BaseDirectory, "ApiDoc.xml");
                //show controller XML comments like summary
                //options.IncludeXmlComments(xmlDocPath, true);
                //options.IncludeXmlComments(xmlParamsDocPath, true);

                options.EnableAnnotations();

                string description = AppStrings.AppPersianFullName;

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = $"{AppStrings.AppEnglishFullName} APIs V1",
                    Description = description,
                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = $"{AppStrings.AppEnglishFullName} APIs V2",
                    Description = description
                });

                //options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                //{
                //    In = ParameterLocation.Header,
                //    Description = "Please insert JWT with Bearer into field",
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey
                //});
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        new string[] { }
                //    }
                //});

                // Add Controller type to tag property of SwaggerDoc to seperate in Middleware
                options.DocumentFilter<ApiAndODataControllersSeparation>();

                // Remove version parameter from all Operations
                options.OperationFilter<RemoveVersionParameters>();

                // Add SSO RefreshToken Header To Calls
                options.OperationFilter<AddRequiredHeaderParameters>();

                // Add OData Query Parameters
                options.OperationFilter<AddODataParametersToODataEndpoints>();

                //set version "api/v{version}/[controller]" from current swagger doc verion
                options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                //Separate and categorize end-points by doc version
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out var methodInfo)) return false;

                    var actionDescriptor = apiDesc.ActionDescriptor;
                    var controllerType = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)actionDescriptor).ControllerTypeInfo;
                    var controllerBaseType = controllerType.BaseType;

                    // Because OData endpoints are not versioned
                    if (controllerBaseType == typeof(BaseODataController))
                        return true;

                    var versions = (methodInfo.DeclaringType ?? throw new InvalidOperationException())
                        .GetCustomAttributes<ApiVersionAttribute>(true)
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v}" == docName);
                });

                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });

            services.AddSwaggerGenNewtonsoftSupport(); // explicit opt-in
        }
    }
}