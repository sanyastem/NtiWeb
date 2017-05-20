using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Web.Attributes
{
    public class AuthorizePermissionsAttribute : AuthorizeAttribute
    {
        public string Permissions { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User?.Identity == null || !httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            var user = AuthManager.User;

            if (user == null)
            {
                return false;
            }

            var permissions = SplitString(Permissions);
            return user.Permissions.Any(e => permissions.Contains(e));
        }

        internal static List<string> SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
            {
                return new List<string>();
            }

            var split = from piece in original.Split(',')
                let trimmed = piece?.Trim()
                where !string.IsNullOrEmpty(trimmed)
                select trimmed;

            return split.ToList();
        }
    }
}