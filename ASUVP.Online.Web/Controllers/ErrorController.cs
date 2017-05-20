using System.Web.Mvc;

namespace ASUVP.Online.Web.Controllers
{
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        public ActionResult Internal()
        {
            Response.StatusCode = 500;
            return View("~/Views/Error/500.cshtml");
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("~/Views/Error/404.cshtml");
        }

        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            return View("~/Views/Error/403.cshtml");
        }
    }
}