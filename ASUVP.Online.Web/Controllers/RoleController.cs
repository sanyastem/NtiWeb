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
    [AuthorizePermissions(Permissions = AuthPermissions.RoleView)]
    public class RoleController : BaseController
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Роли";
            var model = _service.RolesGet();
            return View(model);
        }

        public ActionResult RoleGridView()
        {
            var model = _service.RolesGet();

            return PartialView("_RoleGridView", model);
        }

        public ActionResult RoleRowDetails(Guid roleId)
        {
            ViewData["RoleId"] = roleId;
            var model = _service.RolePermissionsListGet(roleId);
            return PartialView("_RoleRowDetails", model);
        }


        [HttpGet]
        [AuthorizePermissions(Permissions = AuthPermissions.RoleCreate)]
        public ActionResult Create()
        {
            var roleVm = new RoleVM();
            ViewBag.Title = "Новая роль";
            ViewBag.RootPageDetails = new[] { "Index", "Role", "Роли" };
            return View("CreateOrEdit", roleVm);
        }

        [HttpGet]
        [AuthorizePermissions(Permissions = AuthPermissions.RoleEdit)]
        public ActionResult Edit(Guid id)
        {
            var roleVm = new RoleVM();
            roleVm = RoleVM.GetRole(id);
            ViewBag.Title = $"Изменение роли {roleVm.Name}";
            ViewBag.RootPageDetails = new[] { "Index", "Role", "Роли" };
            return View("CreateOrEdit", roleVm);
        }

        [HttpPost]
        [AuthorizePermissions(Permissions = AuthPermissions.RoleEdit+","+ AuthPermissions.RoleCreate)]
        public ActionResult CreateOrEdit(RoleVM model)
        {
            // Добавляется новая роль
            if (model.Id == Guid.Empty)
            {
                var result = _service.RoleInsert(model.Name);

                Guid newRoleId;
                if (Guid.TryParse(result, out newRoleId))
                {
                    if (model.RolePermissionList != null && model.RolePermissionList.Count > 0)
                    {
                        foreach (var permissionId in model.RolePermissionList)
                        {
                            _service.RolePermissionsInsert(newRoleId, permissionId);
                        }
                    }
                }
                else
                {
                    ViewBag.Title = "Новая роль";
                    ViewBag.RootPageDetails = new[] { "Index", "Role", "Роли" };

                    ModelState.AddModelError("Name", result);
                    return View(model);
                }
            }
            // Изменяется роль
            else
            {
                _service.RoleUpdate(model.Id, model.Name);

                //if (result == "Роль успешно изменена")
                {
                    if (model.RolePermissionList != null)
                    {
                        List<PermissionList> oldPermissions = _service.RolePermissionsListGet(model.Id);

                        foreach (var permissionId in model.RolePermissionList)
                        {
                            if (!oldPermissions.Exists(c => c.Id == permissionId))
                            {
                                // добавляем новую запись
                                _service.RolePermissionsInsert(model.Id, permissionId);
                            }
                        }

                        foreach (var oldPermission in oldPermissions)
                        {
                            if (!model.RolePermissionList.Exists(c => c == oldPermission.Id))
                            {
                                // помечаем запись IsDeleted = 1
                                _service.RolePermissionsDelete(model.Id, oldPermission.Id);
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }


        public ActionResult PermissionsGridLookup(List<Guid?> selected)
        {
            ViewBag.Permissions = _service.RolePermissionsListGet(null);
            return PartialView(selected);
        }

        public ActionResult RolesGridLookup(List<Guid?> selected)
        {
            ViewBag.Roles = _service.RolesGet();
            return PartialView(selected);
        }


        #region ExportToExcel

        [HttpGet]
        public ActionResult ExportTo(string filterExpression)
        {
            var filteredList = new List<Role>();
            var models = _service.RolesGet();

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
                    GridViewExtension.WriteXlsx(RoleExcelSettings.GetGridSettings(), filteredList, memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }

                return Json(new
                {
                    success = true,
                    FileGuid = handle,
                    FileName = $"Экспорт ролей {DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}.xlsx"
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

    }
}