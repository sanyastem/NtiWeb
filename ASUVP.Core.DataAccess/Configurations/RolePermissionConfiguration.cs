using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public class RolePermissionConfiguration : EntityConfiguration<RolePermission>
    {
        public RolePermissionConfiguration()
        {
            ToTable("RolePermission");

            HasRequired(e => e.Role).WithMany(e => e.RolePermissions).HasForeignKey(e => e.RoleId);
            HasRequired(e => e.Permission).WithMany(e => e.RolePermissions).HasForeignKey(e => e.PermissionId);
        }
    }
}