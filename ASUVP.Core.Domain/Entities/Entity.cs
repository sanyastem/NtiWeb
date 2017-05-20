using System;

namespace ASUVP.Core.Domain.Entities
{
    public interface IEntity
    {
        Guid CreatedBy { get; set; }
        DateTime CreatedOn { get; set; }
        Guid? UpdatedBy { get; set; }
        DateTime? UpdatedOn { get; set; }
        bool IsDeleted { get; set; }
    }

    public abstract class Entity : EntityBase, IEntity
    {
        public virtual User CreatedByUser { get; set; }
        public virtual User UpdatedByUser { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }

        public virtual void IsDeletedBy(Guid deletedBy)
        {
            IsDeleted = true;
            IsUpdatedBy(deletedBy);
        }

        public virtual void IsUpdatedBy(Guid updatedBy)
        {
            UpdatedBy = updatedBy;
            UpdatedOn = DateTime.UtcNow;
        }

        public virtual void IsCreatedBy(Guid createdBy)
        {
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
        }
    }
}