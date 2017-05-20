using System;
using System.Linq;
using System.Threading.Tasks;
using ASUVP.Core.Diagnostics;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Domain.Repositories;
using ASUVP.Core.Exceptions;

namespace ASUVP.Core.Business.Managers
{
    public abstract class EntityManager<TEntity> : Disposable, IEntityManager<TEntity> where TEntity : Entity
    {
        protected readonly IRepository Repository;

        protected EntityManager(IRepository repository)
        {
            Repository = repository;
        }

        public virtual async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await Repository.GetByIdAsync<TEntity>(id);
        }

        public virtual IQueryable<TEntity> GetQueryActive()
        {
            return Repository.AsQueryable<TEntity>().Where(e => !e.IsDeleted);
        }

        public virtual async Task Create(TEntity entity, Guid createdBy)
        {
            Contract.RequiresParameterNotNull(entity);
            Contract.RequiresParameterNotNull(createdBy);

            entity.IsCreatedBy(createdBy);
            Repository.Add(entity);
            await Repository.SaveChangesAsync();
        }

        public virtual async Task Update(TEntity entity, Guid updatedBy)
        {
            Contract.RequiresParameterNotNull(entity);
            Contract.RequiresParameterNotNull(updatedBy);

            var dbEntity = Repository.GetById<TEntity>(entity.Id);
            if (dbEntity == null)
            {
                throw new EntityNotFoundException();
            }

            entity.IsUpdatedBy(updatedBy);
            await Repository.SaveChangesAsync();
        }

        public virtual async Task Delete(Guid id, Guid deletedBy)
        {
            Contract.RequiresParameterNotNull(id);
            Contract.RequiresParameterNotNull(deletedBy);

            var dbEntity = Repository.GetById<TEntity>(id);
            if (dbEntity == null)
            {
                throw new EntityNotFoundException();
            }

            dbEntity.IsDeletedBy(deletedBy);
            await Repository.SaveChangesAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;

            Repository?.Dispose();
        }
    }
}