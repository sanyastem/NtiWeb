using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public abstract class EntityBaseConfiguration<T> : EntityTypeConfiguration<T> where T : EntityBase
    {
        protected EntityBaseConfiguration()
        {
            HasKey(e => e.Id);

            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnOrder(0);
            Property(e => e.Version).IsRowVersion().HasColumnName("RecTimeStamp");
        }
    }
}