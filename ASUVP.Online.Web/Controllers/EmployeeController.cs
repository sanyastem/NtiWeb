using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using ASUVP.Core.Web.Dto;
using ASUVP.Online.OData;
using ASUVP.Online.Services;
using ASUVP.Online.Web.Attributes;
using ASUVP.Core.Web.Security;

namespace ASUVP.Online.Web.Controllers
{
    public class EmployeeController : BaseController
    {
        //private readonly IODataClient _context;
        //private readonly IEmployeeService _service;

        //public EmployeeController(IODataClient context, IEmployeeService service)
        //{
        //    _context = context;
        //    _service = service;
        //}

        //public ActionResult UserEmployeesGridView(Guid userId)
        //{
        //    ViewBag.UserId = userId;
        //    var employees = _context.UserEmployees(userId);
        //    return PartialView("UserEmployeesGridView", employees);
        //}

        //public ActionResult EmployeeRolesGridView(Guid employeeId)
        //{
        //    ViewBag.EmployeeId = employeeId;
        //    return PartialView("EmployeeRolesGridView", _context.EmployeeRoles(employeeId));
        //}

        //public ActionResult AssignRolesToEmployee(Guid employeeId)
        //{
        //    var employee = _service.GetEmployee(employeeId);
        //    if (employee == null) ViewBag.EditError = "Объект на найден в базе данных.";

        //    return PartialView("AssignRolesToEmployee", employee);
        //}

        //[HttpPost]
        //public async Task<ActionResult> Update(EmployeeDto dto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewBag.EditError = "Пожалуйста, исправьте все ошибки.";
        //        return UserEmployeesGridView(dto.UserId);
        //    }

        //    try
        //    {
        //        await _service.UpdateEmployee(dto);
        //    }
        //    catch (Exception)
        //    {
        //        ViewBag.EditError = "Во время обновления произошла ошибка";
        //    }

        //    return UserEmployeesGridView(dto.UserId);
        //}
    }
}