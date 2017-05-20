using System.Web.Mvc;

namespace ASUVP.Online.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        public RedirectToRouteResult ToHomePage()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}