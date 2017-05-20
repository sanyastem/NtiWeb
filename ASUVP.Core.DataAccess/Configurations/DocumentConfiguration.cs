using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public class DocumentConfiguration : EntityConfiguration<Document>
    {
        public DocumentConfiguration()
        {
            ToTable("Document");

            HasRequired(e => e.Template).WithOptional().Map(e => e.MapKey("TemplateId"));

            HasOptional(e => e.DocOwner).WithMany().HasForeignKey(e => e.DocOwnerId);
            HasOptional(e => e.DocOwner2).WithMany().HasForeignKey(e => e.DocOwnerId2);

            HasOptional(e => e.TargetCompany).WithMany().HasForeignKey(e => e.TargetCompanyId);
            HasOptional(e => e.CustomerCompany).WithMany().HasForeignKey(e => e.CustomerCompanyId);
            HasOptional(e => e.TargetContact).WithMany().HasForeignKey(e => e.TargetContactId);
            HasOptional(e => e.CustomerContact).WithMany().HasForeignKey(e => e.CustomerContactId);

            HasMany(e => e.OwnerDocuments).WithOptional(e => e.DocOwner);
            HasMany(e => e.Owner2Documents).WithOptional(e => e.DocOwner2);

            Property(e => e.DocNumber).IsRequired().HasMaxLength(50);
            Property(e => e.RegNumber).IsRequired().HasMaxLength(50);

            Property(e => e.Name).IsRequired().HasMaxLength(256);
        }
    }
}