using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Domain.Repositories;

namespace ASUVP.Core.Business.Managers
{
    public class PermissionManager : EntityManager<Permission>, IPermissionManager
    {
        public PermissionManager(IRepository repository) : base(repository)
        {
        }
    }
}