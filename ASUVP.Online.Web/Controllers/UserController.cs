using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Web.Mvc;
using ASUVP.Core.Web.Security;
using ASUVP.Online.Services;
using ASUVP.Online.Web.Attributes;
using ASUVP.Online.Web.Models;
using ASUVP.Core.DataAccess.Model;
using System.Linq;
using ASUVP.Online.Web.ToExcelSettings;
using DevExpress.Data.Filtering;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.Controllers
{
    [AuthorizePermissions(Permissions = AuthPermissions.UserView)]
    public class UserController : BaseController
    {
        private readonly IUserService _service;
        private readonly IEmployeeService _employeeService;
        private readonly IRoleService _roleService;

        public UserController(IUserService service, IEmployeeService employeeService, IRoleService roleService)
        {
            _service = service;
            _employeeService = employeeService;
            _roleService = roleService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Пользователи";
            var model = _service.GetUserList();
            return View(model);
        }

        public ActionResult UserGridView()
        {
            var model = _service.GetUserList();

            return PartialView("_UserGridView", model);
        }

        public ActionResult UserRowDetails(Guid userid)
        {
            ViewData["UserId"] = userid;
            var model = _service.GetUserDetailsList(userid);
            return PartialView("_UserRowDetails", model);
        }

        [HttpGet]
        [AuthorizePermissions(Permissions = AuthPermissions.UserCreate)]
        public ActionResult Create()
        {
            var userVm = new UserVM();
            ViewBag.Title = "Новый пользователь";
            ViewBag.RootPageDetails = new[] { "Index", "User", "Пользователи" };
            return View("CreateOrEdit", userVm);
        }


        [HttpGet]
        [AuthorizePermissions(Permissions = AuthPermissions.UserEdit)]
        public ActionResult Edit(Guid id)
        {
            var userVm = UserVM.GetUser(id);
            ViewBag.Title = $"Изменение пользователя {userVm.Name}";
            ViewBag.RootPageDetails = new[] { "Index", "User", "Пользователи" };
            return View("CreateOrEdit", userVm);
        }

        [HttpPost]
        [AuthorizePermissions(Permissions = AuthPermissions.UserEdit+","+ AuthPermissions.UserCreate)]
        public ActionResult CreateOrEdit(UserVM model)
        {
            // Добавляется новый пользователь
            if (model.Id == Guid.Empty)
            {
                var result = _service.UserInsert(model.Name, model.Password, model.ContactId.ToString());

                Guid newUserId;
                if (Guid.TryParse(result, out newUserId))
                {
                    if (model.UserCompanies.Count > 0)
                    {
                        foreach (var companyId in model.UserCompanies)
                        {
                            _employeeService.EmployeeInsert(newUserId, companyId.Value);
                        }
                    }
                }
            }
            // Изменяется пользователь
            else
            {
                string result = _service.UserUpdate(model.Id, model.ContactId, model.Name, model.Active);

                if (result == "Пользователь успешно изменен")
                {
                    if (model.UserCompanies != null)
                    {
                        List<UserDetailsList> oldCompanies = _service.GetUserDetailsList(model.Id);

                        foreach (var companyId in model.UserCompanies)
                        {
                            if (!oldCompanies.Exists(c => c.CompanyId == companyId))
                            {
                                // добавляем новую запись
                                _employeeService.EmployeeInsert(model.Id, companyId.Value);
                            }
                        }

                        foreach (var oldCompany in oldCompanies)
                        {
                            if (!model.UserCompanies.Exists(c => c.Value == oldCompany.CompanyId))
                            {
                                // помечаем запись IsDeleted = 1
                                _employeeService.EmployeeDelete(model.Id, oldCompany.CompanyId);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult UserCompanyRoles(Guid companyId, Guid userId)
        {
            ViewData["EmployeeUserId"] = userId;
            ViewData["EmployeeCompanyId"] = companyId;
            var model = _employeeService.EmployeeRolesGet(companyId, userId);
            return PartialView("_UserCompanyRoles", model);
        }

        [HttpGet]
        [AuthorizePermissions(Permissions = AuthPermissions.UserEdit)]
        public ActionResult UserCompanyRolesEdit(Guid? companyId, Guid? userId)
        {
            UserCompanyRolesVM model = new UserCompanyRolesVM();
            if (companyId.HasValue && userId.HasValue)
            {
                model = UserCompanyRolesVM.GetUserCompanyRolesVM(companyId.Value, userId.Value);
                model.Roles = _employeeService.EmployeeRolesGet(companyId.Value, userId.Value).Select(i => i.Id).ToList();
            }
            return View("UserCompanyRolesEdit", model);
        }

        [HttpPost]
        [AuthorizePermissions(Permissions = AuthPermissions.UserEdit)]
        public ActionResult UserCompanyRolesEdit(UserCompanyRolesVM model)
        {
            if (model.Roles == null) model.Roles = new List<Guid?>();

            if (model.CompanyId != Guid.Empty && model.UserId != Guid.Empty)
            {
                List<EmployeeRolesList> oldRoles = _employeeService.EmployeeRolesGet(model.CompanyId, model.UserId).ToList();

                foreach (var roleId in model.Roles)
                {
                    if (!oldRoles.Exists(c => c.Id == roleId))
                    {
                        // добавляем новую запись
                        _employeeService.EmployeeRoleInsert(model.CompanyId, model.UserId, (Guid)roleId);
                    }
                }
                foreach (var oldRole in oldRoles)
                {
                    if (!model.Roles.Exists(c => c.Value == oldRole.Id.Value))
                    {
                        // помечаем запись IsDeleted = 1
                        _employeeService.EmployeeRoleDelete(oldRole.EmployeeId, (Guid)oldRole.Id);
                    }
                }
            }

            return RedirectToAction("Index");
        }

        #region ExportToExcel

        [HttpGet]
        public ActionResult ExportTo(string filterExpression)
        {
            var filteredList = new List<UserList>();
            var models = _service.GetUserList();

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
                    GridViewExtension.WriteXlsx(UserExcelSettings.GetGridSettings(), filteredList, memoryStream);
                    memoryStream.Position = 0;
                    TempData[handle] = memoryStream.ToArray();
                }

                return Json(new
                {
                    success = true,
                    FileGuid = handle,
                    FileName = $"Экспорт пользователей {DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss")}.xlsx"
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