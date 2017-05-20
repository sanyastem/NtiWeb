using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Domain.Repositories;

namespace ASUVP.Core.Business.Managers
{
    public class ProtocolManager : EntityManager<Protocol>, IProtocolManager
    {
        public ProtocolManager(IRepository repository) : base(repository)
        {
        }
    }
}
