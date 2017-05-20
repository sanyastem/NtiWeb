using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public class CompanyConfiguration : EntityConfiguration<Company>
    {
        public CompanyConfiguration()
        {
            ToTable("Company");

            HasRequired(e => e.Template).WithOptional().Map(e => e.MapKey("TemplateId"));
            HasOptional(e => e.ParentCompany).WithOptionalDependent().Map(e => e.MapKey("ParentId"));

            HasMany(e => e.ChildCompanies).WithOptional(e => e.ParentCompany);

            Property(e => e.FullName).IsRequired().HasMaxLength(256);
            Property(e => e.ShortName).HasMaxLength(256);
            Property(e => e.ViewName).HasMaxLength(256);
        }
    }
}