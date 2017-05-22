using ASUVP.Online.Services;
using System;
using System.Web.Mvc;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Web.Models;

namespace ASUVP.Online.Web.Controllers
{
    public class MailController : BaseController
    {
        private readonly IMailService _service;

        public MailController(IMailService service)
        {
            _service = service;
        }
        
        // GET: Mail
        public ActionResult Index()
        {
            ViewBag.Title = "Почта";
            var sentMailsVm = new MailVM()
            {
                SentMails = _service.GetSentMails(),
                ReceivedMails = _service.GetReceivedMails(),
                Filter = new FilterModel()
            };

            return View(sentMailsVm);
        }

        public ActionResult SentMailViewPartial()
        {
            var model = new MailVM()
            {
                SentMails = _service.GetSentMails(),
                ReceivedMails = _service.GetReceivedMails(),
                Filter = new FilterModel()
            };

            return PartialView("_SentMailViewPartial", model);
        }

        public ActionResult ReceivedMailViewPartial()
        {
            var model = new MailVM()
            {
                SentMails = _service.GetSentMails(),
                ReceivedMails = _service.GetReceivedMails(),
                Filter = new FilterModel()
            };

            return PartialView("_ReceivedMailTabsView", model);
        }

        [HttpGet]
        public ActionResult Details(Guid? id)
        {
            ViewBag.Title = "Просмотр сообщения";
            ViewBag.ID = id;
            var mail = _service.GetMailDetails(id);

            // Обновляем письмо, если оно не было прочитано
            //  происходит только при первом открытии

            bool isUnreaded = false;
            if (mail.ReceiverCompanyId == AuthManager.User.CompanyId)
            {
                
                if (mail.ViewerId == null)
                {
                    isUnreaded = true;
                }
            }

            if (!isUnreaded) return View(mail);

            _service.UpdateUserMailViewTime(id);

            return View(mail);
        }
        public ActionResult UpdateMail(Guid? id)
        {
            ViewBag.Title = "Редактировать письмо";
            var mail = _service.GetMailDetails(id);
            ViewBag.RootPageDetails = new[] { "Index", "Mail", "Почта" };

            var model = new NewMailVM { Body = mail.Body, Subject = mail.Subject };

            return View("ComposeMail", model);
        }

        public ActionResult ComposeMail()
        {
            ViewBag.Title = "Написать письмо";
            ViewBag.RootPageDetails = new [] { "Index", "Mail", "Почта" };

            var model = new NewMailVM { Body = "", Subject = "" };

            return View("ComposeMail", model);
        }

        public PartialViewResult HtmlEditor()
        { 
            return PartialView("_HtmlEditor");
        }

        [HttpPost]
        public ActionResult SendMail(NewMailVM newMail)
        {
            var mail = newMail;

            _service.SendMail(mail.Subject, mail.Body, newMail.CompanyId);

            return Redirect($"{Url.RouteUrl(new {Controller = "Mail", Action = "Index"})}#sent");
        }
    }
}
