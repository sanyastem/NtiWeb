using System;
using System.Collections.Generic;

namespace ASUVP.Core.Web.Security
{
    public class AuthUser
    {
        public AuthUser()
        {
            Permissions = new List<string>();
        }

        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public List<string> Permissions { get; set; }

        public bool IsInRole(string role)
        {
            return Permissions.Contains(role);
        }
    }
}