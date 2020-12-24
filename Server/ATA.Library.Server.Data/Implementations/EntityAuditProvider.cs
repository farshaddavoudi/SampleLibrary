using ATA.Library.Server.Model.Entities.AuditLogAssets;
using ATA.Library.Server.Model.Entities.Contracts.AuditLog;
using ATA.Library.Shared.Service.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace ATA.Library.Server.Data.Implementations
{
    public class EntityAuditProvider : IEntityAuditProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuditSourcesProvider _auditSourcesProvider;

        #region Constructor Injections

        public EntityAuditProvider(IHttpContextAccessor httpContextAccessor, IAuditSourcesProvider auditSourcesProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _auditSourcesProvider = auditSourcesProvider;
        }

        #endregion

        public virtual string? GetAuditValues(EntityEventType eventType, object? newEntity, string? previousJsonAudit = null)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            int? userId;

            var user = httpContext.User;

            if (!user.Identity.IsAuthenticated)
                userId = null;
            else
                userId = user.Claims.Where(x => x.Type == "UserID").Select(x => x.Value).First().ToInt();

            var auditSourceValues = _auditSourcesProvider.GetAuditSourceValues();

            var auditJArray = new JArray();

            // Update & Delete
            if (eventType == EntityEventType.Update || eventType == EntityEventType.Delete)
            {
                auditJArray = JArray.Parse(previousJsonAudit!);
            }

            // Delete => No NewValues
            if (eventType == EntityEventType.Delete)
            {
                newEntity = null;
            }

            JObject newAuditJObject = JObject.FromObject(new EntityAudit<object?>
            {
                EventType = eventType,
                ActorUserId = userId,
                ActDateTime = DateTime.Now,
                AuditSourceValues = auditSourceValues,
                NewEntity = newEntity
            }, new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.None
            });

            auditJArray.Add(newAuditJObject);

            return auditJArray.SerializeToJson(true);
        }
    }
}