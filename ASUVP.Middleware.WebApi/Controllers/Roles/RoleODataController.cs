using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Web.OData;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ASUVP.Middleware.WebApi.Controllers.Roles
{
    public class RoleODataController : BaseODataController<IRoleManager, Role, RoleOData>
    {
        public RoleODataController(IRoleManager manager, IMapper mapper) : base(manager, mapper)
        {
        }

        [HttpGet]
        public IQueryable<PermissionOData> GetPermissions([FromODataUri] Guid key)
        {
            return Manager.GetRolePermissions(key).ProjectTo<PermissionOData>(Mapper.ConfigurationProvider);
        }
    }
}