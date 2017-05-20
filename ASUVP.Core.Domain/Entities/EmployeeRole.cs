using System;

namespace ASUVP.Core.Domain.Entities
{
    public class EmployeeRole : Entity
    {
        public Guid RoleId { get; set; }
        public Guid EmployeeId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Employee Employee { get; set; }
    }
}