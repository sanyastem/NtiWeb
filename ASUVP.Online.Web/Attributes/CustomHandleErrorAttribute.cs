using System.Web.Mvc;
using System.Web.Routing;

namespace ASUVP.Online.Web.Attributes
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new {success = false, error = filterContext.Exception.ToString()},
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller", "Error"},
                    {"action", "Internal"}
                });
            }
        }
    }
}