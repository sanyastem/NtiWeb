using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public class ContactConfiguration : EntityConfiguration<Contact>
    {
        public ContactConfiguration()
        {
            ToTable("Contact");

            Property(e => e.LastName).IsRequired().HasColumnName("F").HasMaxLength(256);
            Property(e => e.FirstName).IsOptional().HasColumnName("I").HasMaxLength(256);
            Property(e => e.MiddleName).IsOptional().HasColumnName("O").HasMaxLength(256);

            Property(e => e.Phone).IsOptional().HasMaxLength(256);
            Property(e => e.Email).IsOptional().HasMaxLength(256);
        }
    }
}