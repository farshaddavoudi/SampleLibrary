using ATA.Library.Server.Model.Entities.Contracts;
using System;
using System.Linq.Expressions;

// ReSharper disable CheckNamespace

namespace Microsoft.EntityFrameworkCore.Metadata.Builders
{
    public static class EntityTypeBuilderExtensions
    {
        public static IndexBuilder HasUniqueIndexArchivable<TEntity>(
            this EntityTypeBuilder<TEntity> builder,
            Expression<Func<TEntity, object?>> indexExpression
        )
            where TEntity : class, IArchivableEntity
        {
            return builder
                    .HasIndex(indexExpression)
                    .HasFilter($"{nameof(IArchivableEntity.IsArchived)} = 0")
                    .IsUnique();
        }
    }
}