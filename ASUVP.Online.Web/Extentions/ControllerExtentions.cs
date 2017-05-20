using System.Web.Mvc;

namespace ASUVP.Online.Web.Extentions
{
    public static class ControllerExtentions
    {
        public static string ControllerName(this ControllerContext context)
        {
            return context.RouteData.Values["controller"].ToString().ToLower();
        }
    }
}