using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public class PermissionConfiguration : EntityConfiguration<Permission>
    {
        public PermissionConfiguration()
        {
            ToTable("Permission");

            Property(e => e.Name).IsRequired().HasMaxLength(256);
            Property(e => e.Code).IsRequired().HasMaxLength(256);
            Property(e => e.Code2).IsRequired().HasMaxLength(256);
            Property(e => e.Module).IsRequired().HasMaxLength(128);
        }
    }
}