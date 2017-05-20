using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public class EmployeeRoleConfiguration : EntityConfiguration<EmployeeRole>
    {
        public EmployeeRoleConfiguration()
        {
            ToTable("EmployeeRole");

            HasRequired(e => e.Role).WithMany(e => e.EmployeeRoles).HasForeignKey(e => e.RoleId);
            HasRequired(e => e.Employee).WithMany(e => e.EmployeeRoles).HasForeignKey(e => e.EmployeeId);
        }
    }
}