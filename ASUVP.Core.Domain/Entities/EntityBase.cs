using System;

namespace ASUVP.Core.Domain.Entities
{
    public interface IEntityBase
    {
        Guid Id { get; set; }
        byte[] Version { get; set; }
        bool IsNew();
    }

    public abstract class EntityBase : IEntityBase
    {
        public Guid Id { get; set; }
        public byte[] Version { get; set; }
        public bool IsNew() => Version == null;
    }
}