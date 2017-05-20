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
using ASUVP.Online.Web.ToExcelSettings;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.Controllers
{
    [AuthorizePermissions(Permissions = AuthPermissions.ActView)]
    public class ActController : Controller
    {
        private readonly IActService _service;

        public ActController(IActService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Акты";
          var model = new ActVM()
            {
                Filter = new FilterModel(){}
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(Guid id)
        {
            var model = _service.GetAct(id);
            ViewBag.Title = $"Акт №{model.DocNumber} от {model.DocDate.Value.ToShortDateString()}";
            // Для перехода в Breadcrumb
            ViewBag.RootPageDetails = new string[] { "Index", "Act", "Акты" };
            return View("ActDetails", model);
        }

        public ActionResult ActViewPartial(string periodType = null, string dateBeg = null, string dateEnd = null, string reportPeriod = null, string agreementId = null, string agrManagerId = null, string statusId = null, string epStatusId = null)
        {
            var model = new ActVM()
            {
                Filter = new FilterModel()
                {
                    DocPeriodType = periodType,
                    DocDateBeg = dateBeg,
                    UseDateEnd = dateEnd,
                    ReportPeriod = reportPeriod,
                    Agreement = agreementId,
                    AgrManagerId = agrManagerId,
                    StatusId = statusId,
                    EPStatusId = epStatusId
                },
                Acts = _service.GetActsByParametrs(AuthManager.User.CompanyId, periodType, dateBeg, dateEnd, reportPeriod, agreementId, agrManagerId, statusId, epStatusId).OrderBy(x=>x.DocName).ToList()
            };
            return PartialView("_ActViewPartial", model);
        }
        public ActionResult RowDetails(Guid actId)
        {
            ViewData["ActId"] = actId;
            var model = _service.GetActDetails(actId);
            return PartialView("_RowActDedails", model);
        }

        public PartialViewResult ActServiceData(Guid id)
        {
            ViewBag.ActId = id;

            var model = _service.GetActDetails(id);
            return PartialView("_ActServiceData", model);
        }

        [HttpPost]
        [AuthorizePermissions(Permissions = AuthPermissions.ActDelete)]
        public JsonResult DeleteActs(Guid[] keys)
        {
            _service.DeleteActs(keys);
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SigningActs(Guid[] keys)
        {
            foreach (var key in keys)
            {
                //delete agreements
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportTo(string filterExpression, string periodType, string dateBeg, string dateEnd, string reportPeriod, string agreementId, string agrManagerId, string statusId, string epStatusId)
        {
            var filteredList = new List<ActList>();
            var models = _service.GetActsByParametrs(AuthManager.User.CompanyId, periodType, dateBeg, dateEnd, reportPeriod, agreementId, agrManagerId, statusId, epStatusId);

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
                    GridViewExtension.WriteXlsx(ActExcelSettings.GetGridSettings(), filteredList, memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }

                return Json(new
                {
                    success = true,
                    FileGuid = handle,
                    FileName = $"Экспорт актов {DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}.xlsx"
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