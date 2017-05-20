using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Domain.Repositories;

namespace ASUVP.Core.Business.Managers
{
    public class CompanyManager : EntityManager<Company>, ICompanyManager
    {
        public CompanyManager(IRepository repository) : base(repository)
        {
        }
    }
}