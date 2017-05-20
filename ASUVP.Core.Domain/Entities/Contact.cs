using System;
using System.Collections.Generic;
using System.Linq;

namespace ASUVP.Core.Domain.Entities
{
    public class Contact : Entity
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Guid? CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public override void IsDeletedBy(Guid deletedBy)
        {
            if (Users != null && Users.Any())
            {
                foreach (var user in Users)
                {
                    user.IsDeletedBy(deletedBy);
                }
            }

            base.IsDeletedBy(deletedBy);
        }
    }
}