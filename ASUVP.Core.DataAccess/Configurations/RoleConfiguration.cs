using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public class RoleConfiguration : EntityConfiguration<Role>
    {
        public RoleConfiguration()
        {
            ToTable("Role");

            Property(e => e.Name).IsRequired().HasMaxLength(256);
        }
    }
}