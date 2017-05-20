using System;
using System.Linq;
using System.Threading.Tasks;
using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.Domain.Managers
{
    public interface IEntityManager<TEntity> : IDisposable where TEntity : Entity
    {
        IQueryable<TEntity> GetQueryActive();
        Task<TEntity> GetByIdAsync(Guid id);
        Task Create(TEntity entity, Guid createdBy);
        Task Update(TEntity entity, Guid updatedBy);
        Task Delete(Guid id, Guid updatedBy);
    }
}