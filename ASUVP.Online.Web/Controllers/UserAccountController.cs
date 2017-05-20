using System;
using System.Linq;
using System.Web.Mvc;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Services;
using ASUVP.Online.Web.Hubs;
using ASUVP.Online.Web.Models;

namespace ASUVP.Online.Web.Controllers
{
    public class UserAccountController : BaseController
    {
        private readonly IAuthenticationService _service;

        public UserAccountController(IAuthenticationService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult WinLogin()
        {

            if (AuthManager.IsWindowsAuthenticated() && Authenticate(AuthManager.DomainUser(), string.Empty))
            {
                return RedirectToAction("UserCompanies", "UserAccount");
            }

            return RedirectToAction("Login", "UserAccount");
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!Authenticate(model.UserName, model.Password))
            {
                ModelState.AddModelError("", "Неверное имя пользователя или пароль");
                return View(model);
            }
            return RedirectToAction("UserCompanies", new {returnUrl});
        }

        [HttpGet]
        public ActionResult UserCompanies(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            Session["navMini"] = false;

            var model = new UserCompanyViewModel();

            var companies = _service.Companies();
            Session["CountCompanies"] = companies.Count;
            if (companies == null || !companies.Any()) return View();
            if (companies.Count == 1)
            {
                return UserCompanies(companies.First().Id, returnUrl);
            }

            model.Companies =
                companies.Select(e => new SelectListItem {Value = e.Id.ToString(), Text = e.ShortName}).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserCompanies(Guid companyId, string returnUrl)
        {
            var userInfo = _service.UserInfo(companyId);

            var authUser = new AuthUser
            {
                UserId = userInfo.Id,
                UserName = User.Identity.Name,
                FullName = userInfo.FullName,
                CompanyId = companyId,
                CompanyName = userInfo.CompanyName,
                Permissions = userInfo.Permissions
            };

            AuthManager.Authorize(authUser);
            Session["UserId"] = userInfo.Id;
            if (!string.IsNullOrEmpty(returnUrl)) return Redirect(returnUrl);
            return ToHomePage();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Login", "UserAccount");
        }

        #region Helper Methods

        private bool Authenticate(string username, string password)
        {
            var token = _service.GetToken(username, password);
            if (token == null || !token.IsValid()) return false;

            AuthManager.SignIn(username, token);

            return true;
        }

        #endregion
    }
}