using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Services;
using ASUVP.Online.Web.Hubs;
using ASUVP.Online.Web.Models.Notification;

namespace ASUVP.Online.Web.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IUserNotificationService _service;

        public NotificationController(IUserNotificationService service)
        {
            _service = service;
        }


        public ActionResult Index()
        {
            ViewBag.Title = "Уведомления";
            ViewBag.OpenTime = DateTime.Now;
            var model = _service.GetUserNotifications(AuthManager.User.UserId, null);
            return View(model);
        }

        [HttpGet]
        public ActionResult GetNotifications(Guid? id)
        {
            var m = _service.GetUserNotifications(AuthManager.User.UserId, null).Where(x => x.ViewDate == new DateTime(1900, 1, 1));
            var model = new List<NotificationVM>();
            foreach (var el in m)
            {
                model.Add(new NotificationVM()
                {
                    Id = el.Id,
                    UserFromId = el.UserFromId,
                    UserToID = el.UserToID,
                    UserFromName = el.UserFromName,
                    UserToName = el.UserToName,
                    CompanyFromName = el.CompanyFromName,
                    CompanyToName = el.CompanyToName,
                    Text = el.Text,
                    CreatedOn = el.CreatedOn.ToString(),
                    ViewDate = el.ViewDate.ToString(),
                    Url = el.Url,
                    ObjectId = el.ObjectId
                });
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UpdateNotification(Guid[] ids)
        {
            _service.UpdateNotificationState(ids);
            return Json(0, JsonRequestBehavior.AllowGet);
        }


        public ActionResult NotificationGridView(int status = 0, string openTime = null)
        {
            var notifications = _service.GetUserNotifications(AuthManager.User.UserId, null);
            DateTime time;
            DateTime.TryParse(openTime, out time);
            ViewBag.OpenTime = DateTime.Now;
            ViewBag.NotifiStatus = status;
            switch (status)
            {
                case 1:
                    return PartialView("_NotificationGridView", notifications.Where(x => x.ViewDate > new DateTime(1950, 1, 1)).ToList());
                case 2:
                    {
                        var unread = notifications.Where(x => x.ViewDate < new DateTime(1950, 1, 2)).ToList();
                        if (!string.IsNullOrEmpty(openTime))
                        {
                            unread.AddRange(notifications.Where(x => x.ViewDate > time).ToList());
                        }
                        return PartialView("_NotificationGridView", unread.OrderBy(x => x.CreatedOn).ToList());
                    }
                default:
                    return PartialView("_NotificationGridView", notifications);

            }
        }

        public ActionResult NotificationDetails(Guid notificationid, string viewDate)
        {
            UserNotificationList notification;
            DateTime date;
            DateTime.TryParse(viewDate, out date);
            if (date > new DateTime(1990, 1, 1, 0, 0, 0))
            {
                notification = _service.GetUserNotifications(AuthManager.User.UserId, notificationid).FirstOrDefault();
            }
            else
            {
                _service.UpdateNotificationState(new[] { notificationid });
                notification = _service.GetUserNotifications(AuthManager.User.UserId, notificationid).FirstOrDefault();

                var contextHub = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                contextHub.Clients.All.ReadNotifications(AuthManager.User.UserId.ToString(), 1);
            }
            return PartialView("_NotificationDetails", notification);
        }
    }
}