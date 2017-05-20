using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public class UserConfiguration : EntityConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("UserList");

            HasOptional(e => e.Contact).WithMany(e => e.Users).Map(e => e.MapKey("ContactId"));

            Property(e => e.UserName).IsRequired().HasMaxLength(256);
            Property(e => e.PasswordHash);
            Property(e => e.SecurityStamp);
        }
    }
}