using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Web.OData;
using AutoMapper;

namespace ASUVP.Middleware.WebApi.Controllers.Users
{
    public class UserODataController : BaseODataController<IUserManager, User, UserOData>
    {
        public UserODataController(IUserManager manager, IMapper mapper) : base(manager, mapper)
        {
        }
    }
}