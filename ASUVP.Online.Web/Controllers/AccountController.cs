using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Online.Services;
using ASUVP.Online.Web.Models;
using ASUVP.Online.Web.ToExcelSettings;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Web.Mvc;
using ASUVP.Online.Web.Attributes;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Web.Models.Notification;
using ASUVP.Online.Web.Hubs;

namespace ASUVP.Online.Web.Controllers
{
    [AuthorizePermissions(Permissions = AuthPermissions.AccountView)]
    public class AccountController : BaseController
    {
        private readonly IAccountService _service;
        private readonly IUserNotificationService _notificationService;
        public AccountController(IAccountService service, IUserNotificationService notificationService)
        {
            _service = service;
            _notificationService = notificationService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Счета";
            var model = new AccountVM()
            {
                Filter = new FilterModel() { }
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(Guid id)
        {
            var model = _service.GetAccount(id);
            ViewBag.Title = $"Счет №{model.DocNumber} от {model.DocDate.Value.ToShortDateString()}";
            // Для перехода в Breadcrumb
            ViewBag.RootPageDetails = new string[] { "Index", "Account", "Счета" };
            return View("AccountDetails", model);
        }


        public ActionResult AccountViewPartial(string periodType = null, string dateBeg = null, string dateEnd = null,
            string reportPeriod = null, string agreementId = null, string agrManagerId = null,
            string statusId = null, string epStatusId = null)
        {
            var model = new AccountVM()
            {
                Filter = new FilterModel()
                {
                    DocPeriodType = periodType,
                    DocDateBeg = dateBeg,
                    DocDateEnd = dateEnd,
                    ReportPeriod = reportPeriod,
                    Agreement = agreementId,
                    AgrManagerId = agrManagerId,
                    StatusId = statusId,
                    EPStatusId = epStatusId
                },
                Accounts = _service.GetAccountsByParametrs(AuthManager.User.CompanyId, periodType, dateBeg, dateEnd, reportPeriod, agreementId, agrManagerId, statusId, epStatusId)
            };
            return PartialView("_AccountViewPartial", model);
        }

        [HttpPost]
        [AuthorizePermissions(Permissions = AuthPermissions.AccountDelete)]
        public JsonResult DeleteAccounts(Guid[] keys)
        {
            _service.DeleteAccounts(keys);
            foreach (var item in keys)
            {


                var idNotificstion = _notificationService.AddUserNotification(AuthManager.User.UserId, AuthManager.User.CompanyId, AuthManager.User.UserId, AuthManager.User.CompanyId, "Счет " + _service.GetAccount(item).TemplateName + " был удален", "Account|details|", (Guid)_service.GetAccount(item).Id);
                var not = _notificationService.GetUserNotifications(AuthManager.User.UserId, idNotificstion).FirstOrDefault();
                var message = new NotificationVM()
                {
                    Id = not.Id,
                    UserFromId = not.UserFromId,
                    UserToID = not.UserToID,
                    UserFromName = not.UserFromName,
                    UserToName = not.UserToName,
                    CompanyFromName = not.CompanyFromName,
                    CompanyToName = not.CompanyToName,
                    Text = not.Text,
                    CreatedOn = not.CreatedOn.ToString(),
                    ViewDate = not.ViewDate.ToString(),
                    Url = not.Url,
                    ObjectId = not.ObjectId
                };

                var contextHub = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                contextHub.Clients.All.DisplayMessage(AuthManager.User.UserId.ToString(), message);
            }
                return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SigningAccounts(Guid[] keys)
        {
            foreach (var key in keys)
            {
                //delete agreements
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportTo(string filterExpression, string periodType, string dateBeg, string dateEnd,
            string reportPeriod, string agreementId, string agrManagerId,string statusId, string epStatusId)
        {
            var filteredList = new List<AccountList>();
            var models = _service.GetAccountsByParametrs(AuthManager.User.CompanyId, periodType, dateBeg, dateEnd, reportPeriod, agreementId, agrManagerId, statusId, epStatusId);

            if (models != null && models.Count > 0)
            {
                if (!string.IsNullOrEmpty(filterExpression))
                {
                    filteredList.AddRange(from element in models
                                          let ee =
                                              new ExpressionEvaluator(TypeDescriptor.GetProperties(element),
                                                  CriteriaOperator.Parse(filterExpression))
                                          where (bool)ee.Evaluate(element)
                                          select element);
                }
                else
                {
                    filteredList = models;
                }

                if (filteredList.Count == 0)
                    return Json(new { success = false, message = "Нет данных для выгрузки." }, JsonRequestBehavior.AllowGet);

                var handle = Guid.NewGuid().ToString();

                using (var memoryStream = new MemoryStream())
                {
                    GridViewExtension.WriteXlsx(AccountExcelSettings.GetGridSettings(), filteredList, memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }

                return Json(new
                {
                    success = true,
                    FileGuid = handle,
                    FileName = $"Экспорт счетов {DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}.xlsx"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, message = "Нет данных для выгрузки." }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Download(string fileGuid, string fileName)
        {
            if (!string.IsNullOrEmpty(fileGuid) && !string.IsNullOrEmpty(fileName) && TempData[fileGuid] != null)
            {
                var data = (byte[])TempData[fileGuid];
                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                //           redirect to another controller action - whatever fits with your application
                return Json(new { status = "error", message = "Ошибка создания файла." });
            }
        }
    }
}