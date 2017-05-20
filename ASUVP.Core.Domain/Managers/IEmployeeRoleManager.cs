using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASUVP.Core.Domain.Managers
{
    public interface IEmployeeRoleManager : IDisposable
    {
        Task CreateRange(Guid employeeId, IList<Guid> roleIds, Guid createdBy);
        Task DeleteRange(Guid employeeId, IList<Guid> roleIds);
    }
}