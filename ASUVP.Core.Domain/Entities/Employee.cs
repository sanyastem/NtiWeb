using System;
using System.Collections.Generic;

namespace ASUVP.Core.Domain.Entities
{
    public class Employee : Entity
    {
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public virtual User User { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<EmployeeRole> EmployeeRoles { get; set; }
    }
}