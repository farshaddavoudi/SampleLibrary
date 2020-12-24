using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using System;

namespace ATA.Library.Server.Api.Infrastructure.OData
{
    public static class ODataEdmModelsConfiguration
    {
        public class Student
        {
            public Guid Id { get; set; }
            public string? Name { get; set; }
            public int Score { get; set; }
        }

        public static IEdmModel GetEdmModels()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Student>("Students");
            return odataBuilder.GetEdmModel();
        }
    }
}