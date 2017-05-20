using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Web.OData;
using AutoMapper;

namespace ASUVP.Middleware.WebApi.Controllers.Permissions
{
    public class PermissionODataController : BaseODataController<IPermissionManager, Permission, PermissionOData>
    {
        public PermissionODataController(IPermissionManager manager, IMapper mapper) : base(manager, mapper)
        {
        }
    }
}