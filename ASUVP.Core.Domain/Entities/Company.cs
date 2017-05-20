using System.Collections.Generic;

namespace ASUVP.Core.Domain.Entities
{
    public class Company : Entity
    {
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string ViewName { get; set; }
        public string Note { get; set; }
        public virtual Template Template { get; set; }
        public virtual Company ParentCompany { get; set; }
        public virtual ICollection<Company> ChildCompanies { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}