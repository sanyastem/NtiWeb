using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASUVP.Core.Domain.Entities;

namespace ASUVP.Core.Domain.Managers
{
    public interface IEmployeeManager : IEntityManager<Employee>
    {
        IQueryable<Role> GetEmployeeRoles(Guid employeeId);
        Task Update(Employee entity, IList<Guid> roles, Guid updatedBy);
    }
}