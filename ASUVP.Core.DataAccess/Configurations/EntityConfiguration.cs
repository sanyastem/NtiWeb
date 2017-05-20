using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.DataAccess.Configurations
{
    public abstract class EntityConfiguration<T> : EntityBaseConfiguration<T> where T : Entity
    {
        protected EntityConfiguration()
        {
            Property(e => e.CreatedBy).IsRequired();
            Property(e => e.CreatedOn).IsRequired();

            Property(e => e.UpdatedOn).IsOptional();

            Property(e => e.IsDeleted).IsRequired();

            HasRequired(e => e.CreatedByUser).WithMany().HasForeignKey(e => e.CreatedBy);
            HasOptional(e => e.UpdatedByUser).WithMany().HasForeignKey(e => e.UpdatedBy);
        }
    }
}