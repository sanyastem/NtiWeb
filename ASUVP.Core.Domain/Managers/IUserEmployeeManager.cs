using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASUVP.Core.Domain.Managers
{
    public interface IUserEmployeeManager : IDisposable
    {
        Task CreateRange(Guid userId, IList<Guid> companyIds, Guid createdBy);
        Task DeleteRange(Guid userId, IList<Guid> companyIds);
    }
}