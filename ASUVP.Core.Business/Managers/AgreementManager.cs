using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Domain.Repositories;

namespace ASUVP.Core.Business.Managers
{
    public class AgreementManager : EntityManager<Agreement>, IAgreementManager
    {
        public AgreementManager(IRepository repository) : base(repository)
        {
        }
    }
}