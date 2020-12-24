using ATA.Library.Server.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Data.Implementations
{
    public class EfCoreDataProviderSpecificMethodsProvider : DefaultDataProviderSpecificMethodsProvider, IDataProviderSpecificMethodsProvider
    {
        public override Task<T> FirstOrDefaultAsync<T>(IQueryable<T> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.FirstOrDefaultAsync(cancellationToken);
        }

        public override Task<long> LongCountAsync<T>(IQueryable<T> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source
                .LongCountAsync(cancellationToken);
        }

        public override bool SupportsConstantParameterization()
        {
            return true;
        }

        public override bool SupportsExpand()
        {
            return false;
        }

        public override bool SupportsQueryable<T>(IQueryable source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source is EntityQueryable<T>;
        }

        public override Task<T[]> ToArrayAsync<T>(IQueryable<T> source, CancellationToken cancellationToken)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source
                .ToArrayAsync(cancellationToken);
        }

        public override IQueryable<T> ApplyWhereByKeys<T>(IQueryable<T> source, params object[] keys)
        {
            // TODO: If T is IEntity, then we've to get key columns from entity framework's workspace metadata

            return base.ApplyWhereByKeys(source, keys);
        }
    }
}