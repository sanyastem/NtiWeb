using System;
using System.Web;
using System.Web.Security;
using ASUVP.Core.Web.Session;

namespace ASUVP.Core.Web.Security
{
    public static class AuthManager
    {
        public static string Token
        {
            get
            {
                var token = SessionProvider.Get<AuthToken>(nameof(AuthToken));
                return token?.AccessToken;
            }
        }

        public static AuthUser User => SessionProvider.Get<AuthUser>(nameof(AuthUser));

        public static void SignIn(string username, AuthToken token)
        {
            SessionProvider.Set(nameof(AuthToken), token);
            FormsAuthentication.SetAuthCookie(username, true);
        }

        public static void SignOut()
        {
            SessionProvider.Remove(nameof(AuthToken));
            FormsAuthentication.SignOut();
        }

        public static void Authorize(AuthUser user)
        {
            SessionProvider.Set(nameof(AuthUser), user);
        }

        public static bool IsUserAuthenticated()
        {
            return Token != null
                   && HttpContext.Current.User != null
                   && HttpContext.Current.User.Identity != null
                   && HttpContext.Current.User.Identity.IsAuthenticated;
        }

        public static bool IsUserAuthorized()
        {
            return User != null && User.CompanyId != Guid.Empty;
        }

        public static bool IsWindowsAuthenticated()
        {
            return HttpContext.Current.Request.LogonUserIdentity != null &&
                   HttpContext.Current.Request.LogonUserIdentity.IsAuthenticated;
        }

        public static string DomainUser()
        {
            return HttpContext.Current.Request.LogonUserIdentity != null
                ? HttpContext.Current.Request.LogonUserIdentity.Name
                : string.Empty;
        }
    }
}