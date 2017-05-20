using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public class TemplateGroupConfiguration : EntityConfiguration<TemplateGroup>
    {
        public TemplateGroupConfiguration()
        {
            ToTable("TemplateGroup");

            Property(e => e.Name).IsRequired().HasMaxLength(256);
            Property(e => e.PublicationName).HasMaxLength(256);
        }
    }
}