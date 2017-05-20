using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASUVP.Core.Diagnostics;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Domain.Repositories;

namespace ASUVP.Core.Business.Managers
{
    public class RolePermissionManager : Disposable, IRolePermissionManager
    {
        private readonly IRepository _repository;

        public RolePermissionManager(IRepository repository)
        {
            _repository = repository;
        }

        public async Task CreateRange(Guid roleId, IList<Guid> permissionIds, Guid createdBy)
        {
            Contract.RequiresParameterNotNull(roleId);
            Contract.RequiresParameterNotNull(permissionIds);
            Contract.RequiresParameterNotNull(createdBy);

            var list = permissionIds.Select(permissionId => new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow
            });

            _repository.AddRange(list);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteRange(Guid roleId, IList<Guid> permissionIds)
        {
            Contract.RequiresParameterNotNull(roleId);
            Contract.RequiresParameterNotNull(permissionIds);

            var list =
                await
                    _repository.FindAsync<RolePermission>(
                        e => e.RoleId == roleId && permissionIds.Contains(e.PermissionId));

            _repository.RemoveRange(list);
            await _repository.SaveChangesAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing) return;

            _repository?.Dispose();
        }
    }
}