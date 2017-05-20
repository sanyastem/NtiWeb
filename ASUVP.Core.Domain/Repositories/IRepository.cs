using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ASUVP.Core.Domain.Repositories
{
    public interface IRepository : IDisposable
    {
        IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : class;
        TEntity GetById<TEntity>(Guid id) where TEntity : class;
        Task<TEntity> GetByIdAsync<TEntity>(Guid id) where TEntity : class;
        IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class;
        Task<IEnumerable<TEntity>> GetAllAsync<TEntity>() where TEntity : class;
        IEnumerable<TEntity> Find<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        Task<IEnumerable<TEntity>> FindAsync<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;
        void Add<TEntity>(TEntity entity) where TEntity : class;
        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void Remove<TEntity>(TEntity entity) where TEntity : class;
        void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void SaveChanges();
        Task SaveChangesAsync();
    }
}