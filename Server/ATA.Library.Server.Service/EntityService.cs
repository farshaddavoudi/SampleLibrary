using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ATA.Library.Server.Model.Entities.Contracts;
using ATA.Library.Server.Service.Contracts;
using ATA.Library.Shared.Service.Exceptions;
using ATA.Library.Shared.Service.Extensions;

namespace ATA.Library.Server.Service
{
    public abstract class EntityService<TEntity> : IEntityService<TEntity> where TEntity : class, IATAEntity, new()
    {
        protected readonly IUserInfoProvider UserInfoProvider;
        protected readonly IATARepository<TEntity> Repository;
        protected readonly IMapper Mapper;

        #region Constructor Injections

        protected EntityService(IUserInfoProvider userInfoProvider, IATARepository<TEntity> repository, IMapper mapper)
        {
            UserInfoProvider = userInfoProvider;
            Repository = repository;
            Mapper = mapper;
        }

        #endregion Property Injections

        public Expression<Func<TEntity, bool>>? CommonWhere { get; set; }

        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? CommonOrder { get; set; }

        public virtual async Task<TEntity?> GetByIdAsync(long key, CancellationToken cancellationToken)
        {
            TEntity? entity = await Repository.GetByIdAsync(cancellationToken, key);

            return entity;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> entities = Repository.GetAll();

            return entities;
        }

        private IQueryable<TEntity> GetSorted(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy)
        {
            return orderBy != null ? orderBy(query) : (CommonOrder != null ? CommonOrder(query) : query);
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            IQueryable<TEntity> query = Repository.GetAll().AddWhere(CommonWhere).AddWhere(where);

            return GetSorted(query, orderBy).AsEnumerable();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(CancellationToken cancellationToken, Expression<Func<TEntity, bool>>? where = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = (await Repository.GetAllAsync(cancellationToken)).AddWhere(CommonWhere).AddWhere(where);

            return GetSorted(query, orderBy).AsEnumerable();
        }

        public async Task<IQueryable<TEntity>> GetAsync(CancellationToken cancellationToken)
        {
            IQueryable<TEntity> query = await Repository.GetAllAsync(cancellationToken);

            return query;
        }


        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await OnAdding(entity, cancellationToken);

            TEntity addedEntity = await Repository.AddAsync(entity, cancellationToken);

            await OnAdded(addedEntity, cancellationToken);

            return addedEntity;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entitiesToAdd,
            CancellationToken cancellationToken)
        {
            var addedEntities = new List<TEntity>();

            foreach (var entityToAdd in entitiesToAdd)
            {
                var addedEntity = await AddAsync(entityToAdd, cancellationToken);

                addedEntities.Add(addedEntity);
            }

            return addedEntities;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await OnUpdating(entity, cancellationToken);

            TEntity updatedEntity = await Repository.UpdateAsync(entity, cancellationToken);

            await OnUpdated(updatedEntity, cancellationToken);

            return updatedEntity;
        }

        public virtual async Task DeleteAsync(long key, CancellationToken cancellationToken)
        {
            TEntity? entity = await Repository.GetByIdAsync(cancellationToken, key);

            if (entity == null)
                throw new ResourceNotFoundException();

            await DeleteAsync(entity: entity, cancellationToken);
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await OnDeleting(entity, cancellationToken);

            TEntity deletedEntity = await Repository.DeleteAsync(entity, cancellationToken);

            await OnDeleted(deletedEntity, cancellationToken);
        }


        #region CRUD Events

        /// <summary>
        /// Gets called right before Add
        /// </summary>
        protected virtual Task OnAdding(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets called right after Add
        /// </summary>
        protected virtual Task OnAdded(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets called right before delete
        /// </summary>
        protected virtual Task OnDeleting(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets called right after delete
        /// </summary>
        protected virtual Task OnDeleted(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets called right before update
        /// </summary>
        protected virtual Task OnUpdating(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets called right after update
        /// </summary>
        protected virtual Task OnUpdated(TEntity entity, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion CRUD Events
    }

    public enum OperationKind
    {
        Get,
        Add,
        Delete,
        Update
    }


}