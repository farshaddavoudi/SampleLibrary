using ATA.Library.Server.Api.Controllers;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace ATA.Library.Server.Api.Infrastructure.Swagger.DocumentFilters
{
    public class ApiAndODataControllersSeparation : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // Filter Controller Xml Comments
            var odataControllerNames = new List<string>();
            var actionDescriptors = context.ApiDescriptions.Select(d => d.ActionDescriptor);
            foreach (var actionDescriptor in actionDescriptors)
            {
                var controllerType = ((ControllerActionDescriptor)actionDescriptor).ControllerTypeInfo;
                var controllerBaseType = controllerType.BaseType;
                if (controllerBaseType == typeof(BaseODataController))
                    odataControllerNames.Add(((ControllerActionDescriptor)actionDescriptor).ControllerName);
            }

            var allTags = swaggerDoc.Tags;
            var tags = new List<OpenApiTag>();
            foreach (var tag in allTags)
            {
                // This tag will be used in Swagger Middleware PreSerializeFilters option
                // to separate ApiControllers and OData Controllers
                var isODataControllerTag = odataControllerNames.Contains(tag.Name);
                tag.Reference = isODataControllerTag ? new OpenApiReference { ExternalResource = "odata" }
                    : new OpenApiReference { ExternalResource = "api" };
                tags.Add(tag);
            }

            swaggerDoc.Tags = tags;
        }
    }
}