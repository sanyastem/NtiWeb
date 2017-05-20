using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.Domain.Managers
{
    public interface IRoleManager : IEntityManager<Role>
    {
        IQueryable<Permission> GetRolePermissions(Guid roleId);
        Task Create(Role entity, IList<Guid> permissions, Guid createdBy);
        Task Update(Role entity, IList<Guid> permissions, Guid updatedBy);
    }
}