using System.Web.Mvc;

namespace ASUVP.Online.Web.Extentions
{
    public static class ViewExtentions
    {
        public static string ControllerName(this ViewContext context)
        {
            return context.RouteData.Values["controller"].ToString().ToLower();
        }

        public static string ActionName(this ViewContext context)
        {
            return context.RouteData.Values["action"].ToString().ToLower();
        }
    }
}