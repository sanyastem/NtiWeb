using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASUVP.Core.Domain.Managers
{
    public interface IRolePermissionManager : IDisposable
    {
        Task CreateRange(Guid roleId, IList<Guid> permissionIds, Guid createdBy);
        Task DeleteRange(Guid roleId, IList<Guid> permissionIds);
    }
}