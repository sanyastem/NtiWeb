using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Domain.Repositories;

namespace ASUVP.Core.Business.Managers
{
    public class AccountManager : EntityManager<Account>, IAccountManager
    {
        public AccountManager(IRepository repository) : base(repository)
        {
        }
    }
}
