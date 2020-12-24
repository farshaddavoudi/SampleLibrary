using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace ATA.Library.Server.Api.Infrastructure.Swagger.OperationFilters
{
    public class AddRequiredHeaderParameters : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.EndpointMetadata;
            var allowAnonymous = filterPipeline.Any(filter => filter is IAllowAnonymous);
            var isAuthorized = filterPipeline.Any(filter => filter is AuthorizeAttribute);

            if (isAuthorized && !allowAnonymous)
            {
                operation.Parameters ??= new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "SSOToken",
                    In = ParameterLocation.Header,
                    Description = "SSO Authentication Token",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "uuid",
                        Example = new OpenApiString("xxxxxxxx-xxxx-xxxx-xxxxxxxxxxxx"),
                        Pattern = "[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}"
                    }
                });
            }
        }
    }
}