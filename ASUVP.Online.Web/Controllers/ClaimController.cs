using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Services;
using ASUVP.Online.Web.Attributes;
using ASUVP.Online.Web.Models;
using ASUVP.Online.Web.Models.Claim;
using ASUVP.Online.Web.ToExcelSettings;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Web.Mvc;
using ASUVP.Online.Web.Models.Notification;
using ASUVP.Online.Web.Hubs;

namespace ASUVP.Online.Web.Controllers
{
    [AuthorizePermissions(Permissions = AuthPermissions.ClaimView)]
    public class ClaimController : BaseController
    {
        private readonly IClaimService _service;
        private readonly IUserNotificationService _notificationService;
        public ClaimController(IClaimService service, IUserNotificationService notificationService)
        {
            _service = service;
            _notificationService = notificationService;
        }



 

        // GET: Claim
        public ActionResult Index(string period = null, string dateBeg = null, string dateEnd = null,
            string shipment = null, string shipmentBeg = null, string shipmentEnd = null,
            string coordination = null, string signing = null, string agreement = null, string manager = null)
        {
            ViewBag.Title = "Заявки";
            if (!string.IsNullOrEmpty(dateBeg) && !string.IsNullOrEmpty(dateEnd))
            {
                if (Convert.ToDateTime(dateBeg) > Convert.ToDateTime(dateEnd))
                {
                    var x = dateEnd;
                    dateEnd = dateBeg;
                    dateBeg = x;
                }
            }
            if (!string.IsNullOrEmpty(shipmentBeg) && !string.IsNullOrEmpty(shipmentEnd))
            {
                if (Convert.ToDateTime(shipmentBeg) > Convert.ToDateTime(shipmentEnd))
                {
                    var x = shipmentEnd;
                    dateEnd = shipmentBeg;
                    shipmentBeg = x;
                }
            }
            var model = new ClaimVM()
            {
                Filter = new FilterModel()
                {
                    DocPeriodType = period,
                    DocDateBeg = dateBeg,
                    DocDateEnd = dateEnd,
                    ShipmentType = shipment,
                    ShipmentBeg = shipmentBeg,
                    ShipmentEnd = shipmentEnd,
                    StatusId = coordination,
                    EPStatusId = signing,
                    AgrManagerId = manager,
                    Agreement = agreement
                }
            };

            return View(model);
        }

        public ActionResult ClaimViewPartial(string period = null, string dateBeg = null, string dateEnd = null,
            string shipment = null, string shipmentBeg = null, string shipmentEnd = null,
            string coordination = null, string signing = null, string agreement = null, string manager = null)
        {
            var model = new ClaimVM()
            {
                Filter = new FilterModel()
                {
                    DocPeriodType = period,
                    DocDateBeg = dateBeg,
                    DocDateEnd = dateEnd,
                    ShipmentType = shipment,
                    ShipmentBeg = shipmentBeg,
                    ShipmentEnd = shipmentEnd,
                    StatusId = coordination,
                    EPStatusId = signing,
                    AgrManagerId = manager,
                    Agreement = agreement
                },
                Claims = _service.GetClaimsByParametrs(period, dateBeg, dateEnd,
                shipment, shipmentBeg, shipmentEnd,
                coordination, signing, agreement, manager, coordination, signing)
            };

            return PartialView("_ClaimViewPartial", model);
        }
        public ActionResult TemplateAdd()
        {
            var model = _service.GetTemplate();
            return PartialView("_addClaim", model);
        }

        public ActionResult TemplateAddForm()
        {
            var model = _service.GetTemplate();
            return PartialView("_CreateOrEdit", model);
        }


        [HttpGet]
        public ActionResult Details(Guid id)
        {
            var model = new Claim();
            model = _service.GetClaim(id);
            ViewBag.Title = $"Заявка №{model.RegNumber} от {model.DocDate.Value.ToShortDateString()}";
            // Для перехода в Breadcrumb
            ViewBag.RootPageDetails = new string[] { "Index", "Claim", "Заявки" };
            return View("Details/Details", model);
        }

        [AuthorizePermissions(Permissions = AuthPermissions.ClaimDelete)]
        public ActionResult DeleteClaims(Guid[] keys)
        {
            _service.DeleteClaims(keys);
            foreach (var item in keys)
            {


                var idNotificstion = _notificationService.AddUserNotification(AuthManager.User.UserId, AuthManager.User.CompanyId, AuthManager.User.UserId, AuthManager.User.CompanyId, "Заявка " + _service.GetClaim(item).TemplateName + " был удалена", "Claim|details|", (Guid)_service.GetClaim(item).Id);
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

        [HttpGet]
        public ActionResult ExportTo(string filterExpression, string period, string dateBeg, string dateEnd,
            string shipment, string shipmentBeg, string shipmentEnd,
            string coordination, string signing, string agreement, string manager)
        {
            var filteredList = new List<ClaimList>();
            var models = _service.GetClaimsByParametrs(period, dateBeg, dateEnd,
                shipment, shipmentBeg, shipmentEnd,
                coordination, signing, agreement, manager, coordination, signing);

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


                string handle = Guid.NewGuid().ToString();

                using (var memoryStream = new MemoryStream())
                {
                    GridViewExtension.WriteXlsx(ClaimExcelSettings.GetGridSettings(), filteredList, memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }

                return Json(new
                {
                    success = true,
                    FileGuid = handle,
                    FileName = $"Экспорт заявок {DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}.xlsx"
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

        public PartialViewResult ClaimRollingStockData(Guid id)
        {
            var model = _service.GetClaimRollingStock(id);
            return PartialView("Details/_ClaimRollingStockData", model);
        }

        public PartialViewResult ClaimNoteData()
        {
            return PartialView("Details/_ClaimNoteData");
        }

        public PartialViewResult ClaimRouteDetailsData(string stTo, string stFrom)
        {
            if (stTo != null && stFrom != null)
            {
                ViewBag.stTo = stTo;
                ViewBag.stFrom = stFrom;
            }

            var model = _service.GetClaimRouteDetails(stFrom, stTo);

            return PartialView("Details/_ClaimRouteDetailsData", model);
        }

        public PartialViewResult ClaimConditionData(Guid id)
        {
            if (id != null)
            {
                ViewBag.ClaimId = id;
            }

            var model = _service.GetClaimConditions(id);
            return PartialView("Details/_ClaimConditionData", model);
        }

        public PartialViewResult ClaimLoadingScheduleData(Guid id)
        {
            var model = _service.GetClaimLoadingSchedule(id);
            return PartialView("Details/_ClaimLoadingScheduleData", model);
        }
    }
}