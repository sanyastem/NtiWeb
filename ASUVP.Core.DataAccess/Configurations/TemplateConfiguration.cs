using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public class TemplateConfiguration : EntityConfiguration<Template>
    {
        public TemplateConfiguration()
        {
            ToTable("Template");

            HasRequired(e => e.TemplateGroup).WithOptional().Map(e => e.MapKey("TemplateGroupId"));

            Property(e => e.Name).IsRequired();
        }
    }
}