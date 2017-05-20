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
using ASUVP.Core.Web.Security;
using ASUVP.Online.Web.Attributes;

namespace ASUVP.Online.Web.Controllers
{
    [AuthorizePermissions(Permissions = AuthPermissions.InstructionView)]
    public class InstructionController : Controller
    {
        private readonly IInstructionService _service;

        public InstructionController(IInstructionService service)
        {
            _service = service;
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Инструкции";

            var model = new InstructionVM()
            {
                Filter = new FilterModel(){}
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Details(Guid id)
        {
            var model = _service.GetInstruction(id);
            ViewBag.Title = $"Инструкция №{model.DocNumber} от {model.DocDate.Value.ToShortDateString()}";
            // Для перехода в Breadcrumb
            ViewBag.RootPageDetails = new string[] { "Index", "Instruction", "Инструкции" };
            return View("InstructionDetails", model);
        }

        public ActionResult InstructionViewPartial(string periodType = null, string dateBeg = null, string dateEnd = null, string stationId = null, string agreementId = null, string agrManagerId = null, string templateId = null, string statusId = null, string epStatusId = null)
        {
            var model = new InstructionVM()
            {
                Filter = new FilterModel()
                {
                    DocPeriodType = periodType,
                    DocDateBeg = dateBeg,
                    DocDateEnd = dateEnd,
                    Station = stationId,
                    Agreement = agreementId,
                    AgrManagerId = agrManagerId,
                    InstructionTemplateId = templateId,
                    StatusId = statusId,
                    EPStatusId = epStatusId
                },
                Instructions = _service.GetInstructionByParametrs(AuthManager.User.CompanyId, periodType, dateBeg, dateEnd, stationId, agreementId, agrManagerId, templateId, statusId, epStatusId)
            };
            return PartialView("_InstructionViewPartial", model);
        }

        [HttpPost]
        [AuthorizePermissions(Permissions = AuthPermissions.InstructionDelete)]
        public JsonResult DeleteInstructions(Guid[] keys)
        {
            if(keys!=null)
            _service.DeleteInstructions(keys);
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SigningInstructions(Guid[] keys)
        {
            foreach (var key in keys)
            {
                //delete agreements
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ExportTo(string filterExpression, string periodType, string dateBeg, string dateEnd, string stationId, string agreementId , string agrManagerId, string templateId, string statusId, string epStatusId)
        {
            var filteredList = new List<InstructionList>();
            var models = _service.GetInstructionByParametrs(AuthManager.User.CompanyId, periodType, dateBeg, dateEnd, stationId, agreementId, agrManagerId, templateId, statusId, epStatusId);

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

                if(filteredList.Count == 0)
                    return Json(new { success = false, message = "Нет данных для выгрузки." }, JsonRequestBehavior.AllowGet);

                var handle = Guid.NewGuid().ToString();

                using (var memoryStream = new MemoryStream())
                {
                    GridViewExtension.WriteXlsx(InstructionExcelSettings.GetGridSettings(), filteredList, memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }

                return Json(new
                {
                    success = true,
                    FileGuid = handle,
                    FileName = $"Экспорт инструкций {DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}.xlsx"
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