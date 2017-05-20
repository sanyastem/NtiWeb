using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ASUVP.Core.Web.Security;
using System.Web.Configuration;
using System.Configuration;

namespace ASUVP.Online.Web.Attributes
{
    public class AuthorizeSessionAttribute : AuthorizeAttribute
    {
        private const string Controller = "useraccount";
        private const string Action = "usercompanies";

        protected override bool AuthorizeCore(HttpContextBase context)
        {
            if (!AuthManager.IsUserAuthenticated()) return false;
            if (!AuthManager.IsUserAuthorized() && !SkipAuthorize(context)) return false;

            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext context)
        {
            if (AuthManager.IsUserAuthenticated())
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", Controller }, { "action", Action } });
            }
            else
            {
                string authMode = ((AuthenticationSection)ConfigurationManager.GetSection("system.web/authentication")).Mode.ToString();

                if (authMode.ToLowerInvariant().Trim() == "windows")
                {
                    if (!AuthManager.IsWindowsAuthenticated())
                    {
                        base.HandleUnauthorizedRequest(context);
                    }
                    else
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "UserAccount" }, { "action", "WinLogin" } });
                    }
                }
                else
                {
                    base.HandleUnauthorizedRequest(context);
                }
            }
        }

        private bool SkipAuthorize(HttpContextBase context)
        {
            var controller = context.Request.RequestContext.RouteData.Values["controller"].ToString();
            var action = context.Request.RequestContext.RouteData.Values["action"].ToString();
            return controller.ToLower() == Controller.ToLower() && action.ToLower() == Action.ToLower();
        }
    }
}