using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ASUVP.Core.Diagnostics;
using ASUVP.Core.Domain.Repositories;

namespace ASUVP.Core.DataAccess.Repositories
{
    public abstract class Repository : Disposable, IRepository
    {
        protected DbContext Context;

        protected Repository(DbContext context)
        {
            Contract.RequiresParameterNotNull(context);

            Context = context;
        }

        public IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : class
        {
            return Context.Set<TEntity>();
        }

        public TEntity GetById<TEntity>(Guid id) where TEntity : class
        {
            return Context.Set<TEntity>().Find(id);
        }

        public Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : class
        {
            return Context.Set<TEntity>().FindAsync(id);
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return Context.Set<TEntity>().ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : class
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }

        public async Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate)
            where TEntity : class
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public void Add<TEntity>(TEntity entity) where TEntity : class
        {
            Contract.RequiresParameterNotNull(entity);
            Context.Set<TEntity>().Add(entity);
        }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Contract.RequiresParameterNotNull(entities);
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            Contract.RequiresParameterNotNull(entity);
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Contract.RequiresParameterNotNull(entities);
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public void SaveChanges()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                RaiseDbEntityValidationException(e);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                RaiseDbEntityValidationException(e);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void RaiseDbEntityValidationException(DbEntityValidationException exception)
        {
            var raise = (
                from validationErrors in exception.EntityValidationErrors
                from validationError in validationErrors.ValidationErrors
                select $"{validationErrors.Entry.Entity}: {validationError.ErrorMessage}")
                .Aggregate<string, Exception>(exception,
                    (current, message) => new InvalidOperationException(message, current));

            throw raise;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;

            Context?.Dispose();
            Context = null;
        }
    }
}