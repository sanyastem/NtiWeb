using System.Web.Mvc;
using System.Web.Routing;

namespace ASUVP.Online.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional});
        }
    }
}