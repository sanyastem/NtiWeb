using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public class EmployeeConfiguration : EntityConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            ToTable("Employee");

            HasRequired(e => e.User).WithMany(e => e.Employees).HasForeignKey(e => e.UserId);
            HasRequired(e => e.Company).WithMany(e => e.Employees).HasForeignKey(e => e.CompanyId);
        }
    }
}