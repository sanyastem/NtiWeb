using System;

namespace ASUVP.Core.Domain.Entities
{
    public class Agreement : Entity
    {
        public DateTime? DateBeg { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime? DateStop { get; set; }
        public virtual Document Document { get; set; }
    }
}