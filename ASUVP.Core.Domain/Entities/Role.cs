using System;
using System.Collections.Generic;
using System.Linq;

namespace ASUVP.Core.Domain.Entities
{
    public class Role : Entity
    {
        public string Name { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public virtual ICollection<EmployeeRole> EmployeeRoles { get; set; }

        public override void IsDeletedBy(Guid deletedBy)
        {
            if (RolePermissions != null && RolePermissions.Any())
            {
                foreach (var permission in RolePermissions)
                {
                    permission.IsDeletedBy(deletedBy);
                }
            }

            base.IsDeletedBy(deletedBy);
        }

        public void AddPermissions(IList<Guid> permissions, Guid createdBy)
        {
            var rolePermissions = permissions.Select(permissionId => new RolePermission
            {
                RoleId = Id,
                PermissionId = permissionId,
                CreatedBy = createdBy,
                CreatedOn = DateTime.UtcNow
            });

            RolePermissions = rolePermissions.ToList();
        }
    }
}