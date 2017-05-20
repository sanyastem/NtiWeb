using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ASUVP.Online.Web.Extentions
{
    public static class HtmlExtentions
    {
        public static MvcHtmlString GridViewAction(this HtmlHelper htmlHelper, string controller)
        {
            return htmlHelper.Action("GridView", controller);
        }

        public static string IsActive(this HtmlHelper html, string controllers = "", string actions = "",
            string cssClass = "active")
        {
            var viewContext = html.ViewContext;
            var isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            var routeValues = viewContext.RouteData.Values;
            var currentAction = routeValues["action"].ToString();
            var currentController = routeValues["controller"].ToString();

            if (string.IsNullOrEmpty(actions))
                actions = currentAction;

            if (string.IsNullOrEmpty(controllers))
                controllers = currentController;

            var acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
            var acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController)
                ? cssClass
                : string.Empty;
        }
    }
}