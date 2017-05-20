using System.Web.Mvc;
using ASUVP.Online.Web.Attributes;

namespace ASUVP.Online.Web
{
    public class FilterConfig
    {
        public static void Configure(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizeAttribute());
            filters.Add(new AuthorizeSessionAttribute());
            //filters.Add(new AuthorizeWindowsUserAttribute());
            //filters.Add(new CustomHandleErrorAttribute());
            //filters.Add(new HandleErrorAttribute());
        }
    }
}