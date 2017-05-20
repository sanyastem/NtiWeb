using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Web.OData;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ASUVP.Middleware.WebApi.Controllers.Employees
{
    public class EmployeeODataController : BaseODataController<IEmployeeManager, Employee, EmployeeOData>
    {
        public EmployeeODataController(IEmployeeManager manager, IMapper mapper) : base(manager, mapper)
        {
        }

        [HttpGet]
        public IQueryable<RoleOData> GetRoles([FromODataUri] Guid key)
        {
            return Manager.GetEmployeeRoles(key).ProjectTo<RoleOData>(Mapper.ConfigurationProvider);
        }
    }
}