using System;
using System.Web.Mvc;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Services;
using ASUVP.Online.Web.Attributes;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using ASUVP.Core.DataAccess.Model;
using ASUVP.Online.Web.Models.Contact;
using ASUVP.Online.Web.ToExcelSettings;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.Controllers
{
    [AuthorizePermissions(Permissions = AuthPermissions.ContactView)]
    public class ContactController : BaseController
    {
        private readonly IContactService _service;

        public ContactController(IContactService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Контакты";
            return View(new List<ContactList>());
        }

        public ActionResult ContactViewPartial()
        {
            return PartialView("_ContactViewPartial", _service.ContactListGet());
        }

        [HttpGet]
        public ActionResult Details(Guid id)
        {
            var model = _service.ContactDetails(id);
            ViewBag.Title = "Просмотр контакта " + model.F +" "+ model.I + " " + model.O;

            return View("Details", model);
        }

        [HttpGet]
        [AuthorizePermissions(Permissions = AuthPermissions.ContactCreate)]
        public ActionResult Create()
        {
            var contactEditable = ContactEditable.GetEmptyContact();
            ViewBag.RootPageDetails = new[] { "Index", "Contact", "Контакты" };
            ViewBag.Title = "Добавление контакта";

            return View("CreateOrEdit", contactEditable);
        }

        [HttpGet]
        [AuthorizePermissions(Permissions = AuthPermissions.ContactEdit)]
        public ActionResult Edit(Guid id)
        {
            var contactEditable = ContactEditable.GetContact(id);
            ViewBag.RootPageDetails = new[] { "Index", "Contact", "Контакты" };
            ViewBag.Title = "Редактирование контакта " + contactEditable.F + " " + contactEditable.I + " " + contactEditable.O;

            return View("CreateOrEdit", contactEditable);
        }

        [HttpPost]
        [AuthorizePermissions(Permissions = AuthPermissions.ContactEdit +","+ AuthPermissions.ContactEdit)]
        public ActionResult CreateOrEdit(ContactEditable model)
        {
            if (!ModelState.IsValid)
            {
                if (model.Id == Guid.Empty)
                {
                    ViewBag.Title = "Добавление контакта";
                }
                else
                {
                    ViewBag.Title = "Редактирование контакта " + model.F + " " + model.I + " " + model.O;
                }

                return View("CreateOrEdit", model);
            }
            if (model.Id != Guid.Empty)
            {
                _service.ContactUpdate(model.Id, model.F?.Trim(), model.I?.Trim(), model.O?.Trim(), model.Phone, model.Email, model.CompanyId, AuthManager.User.UserId);
                ViewBag.MessageSuccess = "Контакт успешно изменен";
            }
            else
            {
                _service.ContactCreate(model.F?.Trim(), model.I?.Trim(), model.O?.Trim(), model.Phone, model.Email, model.CompanyId, AuthManager.User.UserId);
                ViewBag.MessageSuccess = "Контакт успешно добавлен";
            }


            return RedirectToAction("Index");
        }


        public ActionResult CompanyLookUp(ContactEditable contactEditable)
        {
            return PartialView("Lookup/CompanyLookUp", contactEditable);
        }


        [HttpPost]
        [AuthorizePermissions(Permissions = AuthPermissions.ContactDelete)]
        public ActionResult DeleteContacts(Guid[] keys)
        {
            _service.DeleteContacts(keys);
            ViewBag.MessageSuccess = "Контакт(ы) успешно добавлен(ы)";

            return RedirectToAction("Index");
        }



        #region ExportToExcel



        [HttpGet]
        public ActionResult ExportTo(string filterExpression)
        {
            var filteredList = new List<ContactList>();
            var models = _service.ContactListGet();

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
                    GridViewExtension.WriteXlsx(ContactExcelSettings.GetGridSettings(), filteredList, memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }

                return Json(new
                {
                    success = true,
                    FileGuid = handle,
                    FileName = $"Экспорт контактов {DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}.xlsx"
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
        #endregion

        public ActionResult ContactGridLookup(Guid? selected)
        {
            ViewBag.Contacts = _service.ContactListGet();
            return PartialView(selected);
        }
    }
}