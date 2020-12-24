using ATA.Library.Server.Api.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace ATA.Library.Server.Api.Infrastructure.Swagger.OperationFilters
{
    /// <summary>
    /// Adds the supported odata parameters for odata endpoints 
    /// ONLY if no parameters are defined already.
    /// </summary>
    public class AddODataParametersToODataEndpoints : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var actionDescriptor = context.ApiDescription.ActionDescriptor;
            var controllerType = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)actionDescriptor).ControllerTypeInfo;
            var controllerBaseType = controllerType.BaseType;

            if (controllerBaseType == typeof(BaseODataController))
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                var odataParameters = new List<OpenApiParameter>
                {
                    new OpenApiParameter
                    {
                        Name = "SSOToken",
                        In = ParameterLocation.Header,
                        Description = "SSO Authentication RefreshToken",
                        Required = true
                    },
                    new OpenApiParameter
                    {
                        Name = "$filter",
                        Description = "Filter the results using OData syntax.",
                        Required = false,
                        In = ParameterLocation.Query,
                        Schema = new OpenApiSchema {Type = "string"}
                    },
                    new OpenApiParameter
                    {
                        Name = "$orderby",
                        Description = "Order the results using OData syntax.",
                        Required = false,
                        In = ParameterLocation.Query,
                        Schema = new OpenApiSchema {Type = "string"}
                    },
                    new OpenApiParameter
                    {
                        Name = "$skip",
                        Description = "The number of results to skip.",
                        Required = false,
                        In = ParameterLocation.Query,
                        Schema = new OpenApiSchema {Type = "integer"}
                    },
                    new OpenApiParameter
                    {
                        Name = "$top",
                        Description = "The number of results to return.",
                        Required = false,
                        In = ParameterLocation.Query,
                        Schema = new OpenApiSchema {Type = "integer"}
                    },
                    new OpenApiParameter
                    {
                        Name = "$count",
                        Description = "Return the total count.",
                        Required = false,
                        In = ParameterLocation.Query,
                        Schema = new OpenApiSchema {Type = "bool"}
                    }
                };

                odataParameters.ForEach(p => operation.Parameters.Add(p));
            }
        }
    }
}
