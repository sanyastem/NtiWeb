using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ASUVP.Core.Diagnostics;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Domain.Repositories;
using ASUVP.Core.Exceptions;

namespace ASUVP.Core.Business.Managers
{
    public class RoleManager : EntityManager<Role>, IRoleManager
    {
        private readonly IRolePermissionManager _manager;

        public RoleManager(IRepository repository, IRolePermissionManager manager) : base(repository)
        {
            _manager = manager;
        }

        public IQueryable<Permission> GetRolePermissions(Guid roleId)
        {
            var role = Repository.GetById<Role>(roleId);
            return role?.RolePermissions.Select(e => e.Permission).AsQueryable();
        }

        public async Task Create(Role role, IList<Guid> permissions, Guid createdBy)
        {
            Contract.RequiresParameterNotNull(role);
            Contract.RequiresParameterNotNull(permissions);
            Contract.RequiresParameterNotNull(createdBy);

            var entity = Repository.Find<Role>(e => e.Name.ToUpper() == role.Name.ToUpper()).FirstOrDefault();
            if (entity != null) throw new EntityAlreadyExistsException();

            if (permissions.Any()) role.AddPermissions(permissions, createdBy);
            role.IsCreatedBy(createdBy);

            Repository.Add(role);
            await Repository.SaveChangesAsync();
        }

        public async Task Update(Role role, IList<Guid> permissions, Guid updatedBy)
        {
            Contract.RequiresParameterNotNull(role);
            Contract.RequiresParameterNotNull(permissions);
            Contract.RequiresParameterNotNull(updatedBy);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var entity =
                    Repository.Find<Role>(e => e.Name.ToUpper() == role.Name.ToUpper() && e.Id != role.Id)
                        .FirstOrDefault();
                if (entity != null) throw new EntityAlreadyExistsException();

                entity = Repository.GetById<Role>(role.Id);
                if (entity == null) throw new EntityNotFoundException();

                entity.Name = role.Name;
                entity.IsUpdatedBy(updatedBy);

                await Repository.SaveChangesAsync();

                var rolePermissions = entity.RolePermissions.Select(e => e.PermissionId).ToList();
                var permissionsToAdd = permissions.Except(rolePermissions).ToList();
                var permissionsToRemove = rolePermissions.Except(permissions).ToList();

                await _manager.DeleteRange(entity.Id, permissionsToRemove);
                await _manager.CreateRange(entity.Id, permissionsToAdd, updatedBy);

                scope.Complete();
            }
        }
    }
}