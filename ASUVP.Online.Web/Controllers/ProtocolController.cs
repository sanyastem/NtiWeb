using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Services;
using ASUVP.Online.Web.Models;
using ASUVP.Online.Web.ToExcelSettings;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Web.Mvc;
using ASUVP.Online.Web.Attributes;

namespace ASUVP.Online.Web.Controllers
{
    [AuthorizePermissions(Permissions = AuthPermissions.ProtocolView)]
    public class ProtocolController : BaseController
    {
        private readonly IProtocolService _service;

        public ProtocolController(IProtocolService service)
        {
            _service = service;
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Протоколы";
            var model = new ProtocolVM()
            {
                Filter = new FilterModel() { }
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(Guid id)
        {
            var model = _service.GetProtocol(id);
            ViewBag.Title = $"Протокол №{model.DocNumber} от {model.DocDate.Value.ToShortDateString()}";
            // Для перехода в Breadcrumb
            ViewBag.RootPageDetails = new string[] { "Index", "Protocol", "Протоколы" };
            return View("Details", model);
        }

        public ActionResult ProtocolViewPartial(string periodType = null, string dateBeg = null, string dateEnd = null,
            string usePeriod = null, string useDateBeg = null,
            string useDateEnd = null, string agreementId = null,
            string agrManagerId = null, string statusId = null, string epStatusId = null)
        {
            var model = new ProtocolVM()
            {
                Filter = new FilterModel()
                {
                    DocPeriodType = periodType,
                    DocDateBeg = dateBeg,
                    DocDateEnd = dateEnd,
                    UsePeriod = usePeriod,
                    UseDateBeg = useDateBeg,
                    UseDateEnd = useDateEnd,
                    Agreement = agreementId,
                    AgrManagerId = agrManagerId,
                    StatusId = statusId,
                    EPStatusId = epStatusId
                },
                Protocols = _service.GetProtocolsByParametrs(AuthManager.User.CompanyId, periodType, dateBeg, dateEnd, usePeriod, useDateBeg, useDateEnd, agreementId, agrManagerId, statusId, epStatusId)
            };
            return PartialView("_ProtocolViewPartial", model);
        }

        [HttpPost]
        [AuthorizePermissions(Permissions = AuthPermissions.ProtocolDelete)]
        public JsonResult DeleteProtocols(Guid[] keys)
        {
            _service.DeleteProtocols(keys);
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SigningProtocols(Guid[] keys)
        {
            foreach (var key in keys)
            {
                //delete agreements
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportTo(string filterExpression, string periodType, string dateBeg, string dateEnd,
            string usePeriod, string useDateBeg,
            string useDateEnd, string agreementId,
            string agrManagerId, string statusId, string epStatusId)
        {
            //edit protocol list to protocolDetails or add fields to protocol list
            var filteredList = new List<ProtocolList>();
            var models = _service.GetProtocolsByParametrs(AuthManager.User.CompanyId, periodType, dateBeg, dateEnd, usePeriod, useDateBeg, useDateEnd, agreementId, agrManagerId, statusId, epStatusId);


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
                    GridViewExtension.WriteXlsx(ProtocolExcelSettings.GetGridSettings(), filteredList, memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }

                return Json(new
                {
                    success = true,
                    FileGuid = handle,
                    FileName = $"Экспорт протоколов {DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}.xlsx"
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

        public ActionResult RowDetails(Guid protocolId)
        {
            ViewData["ProtocolId"] = protocolId;
            var model = _service.GetProtocolDetails(protocolId);
            return PartialView("_RowProtocolDetails", model);
        }

        public PartialViewResult ProtocolTariffData(Guid id)
        {
            ViewBag.ProtocolId = id;
            var model = _service.GetProtocolDetails(id);
            return PartialView("_ProtocolTariffData", model);
        }
    }
}