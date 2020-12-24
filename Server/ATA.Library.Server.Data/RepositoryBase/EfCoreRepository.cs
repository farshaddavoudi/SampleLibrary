using ATA.Library.Server.Data.Contracts;
using ATA.Library.Server.Model.Entities.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;

namespace ATA.Library.Server.Data.RepositoryBase
{
    public class EfCoreRepository<TDbContext, TEntity> : EfCoreRepositoryBase<TDbContext, TEntity>
        where TEntity : class, IEntity
        where TDbContext : DbContext
    {
        #region Constructor Injections

        private readonly IDataProviderSpecificMethodsProvider _efDataProviderSpecificMethodsProvider;
        public EfCoreRepository(IDataProviderSpecificMethodsProvider dataProviderSpecificMethodsProvider, TDbContext dbContext) : base(dbContext)
        {
            _efDataProviderSpecificMethodsProvider = dataProviderSpecificMethodsProvider;
        }

        #endregion

        public override TEntity? GetById(params object[] ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            return _efDataProviderSpecificMethodsProvider.ApplyWhereByKeys(GetAll(), ids)
                .SingleOrDefault();
        }

        public override async Task<TEntity?> GetByIdAsync(CancellationToken cancellationToken, params object[] ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            return await _efDataProviderSpecificMethodsProvider.ApplyWhereByKeys((await GetAllAsync(cancellationToken).ConfigureAwait(false)), ids)
                .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    //public class EfCoreRepository<TEntity> : EfCoreRepository<PaxitDbContext, TEntity>
    //    where TEntity : class, IEntity
    //{
    //    public EfCoreRepository(IDataProviderSpecificMethodsProvider dataProviderSpecificMethodsProvider) : base(dataProviderSpecificMethodsProvider)
    //    {
    //    }
    //}
}