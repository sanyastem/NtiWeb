using System.Collections.Generic;

namespace ASUVP.Core.Domain.Entities
{
    public class Permission : Entity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Code2 { get; set; }
        public string Module { get; set; }
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}